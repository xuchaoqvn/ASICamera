using System;
using UnityEngine;
using ZWOptical.ASISDK;
using Unity.Collections;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using static ZWOptical.ASISDK.ASICameraDll2;

namespace ASICamera
{
    /// <summary>
    /// 纹理更新器接口
    /// </summary>
    public interface IUpdateTexturerHander : IDisposable
    {
        /// <summary>
        /// 当前帧
        /// </summary>
        /// <returns>帧</returns>
        long Frame
        {
            get;
        }

        /// <summary>
        /// 获取原始纹理
        /// </summary>
        /// <value>原始纹理</value>
        Texture2D SourceTexture
        {
            get;
        }

        /// <summary>
        /// 运行更新纹理任务
        /// </summary>
        /// <param name="cameraID">ASI相机ID</param>
        /// <param name="getexpMs">获取曝光时间</param>
        /// <param name="fps">帧率</param>
        /// <param name="width">纹理宽度</param>
        /// <param name="height">纹理高度</param>
        void RunUpdate(int cameraID, Func<int> getexpMs, int fps = 24, int width = 1280, int height = 720);

        /// <summary>
        /// 暂停
        /// </summary>
        void Pause();

        /// <summary>
        /// 继续
        /// </summary>
        void Resume();

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="deltaTime">间隔时间</param>
        void Update(float deltaTime);
    }

    /// <summary>
    /// 默认纹理更新器
    /// </summary>
    public class DefaultUpdateTexturerHander : IUpdateTexturerHander
    {
        /// <summary>
        /// 纹理更新状态
        /// </summary>
        private enum UpdateState
        {
            /// <summary>
            /// 默认
            /// 等待状态
            /// </summary>
            Default = 0,

            /// <summary>
            /// 可以更新
            /// </summary>
            CanUpdate,

            /// <summary>
            /// 更新中
            /// </summary>
            Updateing,

            /// <summary>
            /// 更新结束
            /// </summary>
            UpdateEnd,

            /// <summary>
            /// 更新失败
            /// </summary>
            UpdateFailed,

            /// <summary>
            /// 暂停更新
            /// </summary>
            Pause,

            /// <summary>
            /// 结束
            /// </summary>
            End
        }

        #region Field
        /// <summary>
        /// 帧率
        /// </summary>
        private int m_Fps = 24;

        /// <summary>
        /// 更新间隔
        /// </summary>
        private float m_Interval = 1.0f / 24;

        /// <summary>
        /// 纹理
        /// </summary>
        private Texture2D m_SourceTexture;

        /// <summary>
        /// 缓冲句柄
        /// </summary>
        private IntPtr m_IntPtr;

        /// <summary>
        /// 缓冲数组
        /// </summary>
        private byte[] m_Datas;

        /// <summary>
        /// 纹理原始数据
        /// </summary>
        private NativeArray<Color32> m_Color32s;

        /// <summary>
        /// 纹理更新状态
        /// </summary>
        private UpdateState m_UpdateState = UpdateState.Default;

        /// <summary>
        /// 帧
        /// </summary>
        private long m_Frame;

        /// <summary>
        /// 计时器
        /// </summary>
        private float m_Timer = default;
        #endregion

        #region Property
        /// <summary>
        /// 获取当前帧
        /// </summary>
        public long Frame => this.m_Frame;

        /// <summary>
        /// 获取原始纹理
        /// </summary>
        public Texture2D SourceTexture => this.m_SourceTexture;
        #endregion

        #region Function
        /// <summary>
        /// 运行更新纹理任务
        /// </summary>
        /// <param name="cameraID">ASI相机ID</param>
        /// <param name="getexpMs">获取曝光时间</param>
        /// <param name="fps">帧率</param>
        /// <param name="width">纹理宽度</param>
        /// <param name="height">纹理高度</param>
        public void RunUpdate(int cameraID, Func<int> getexpMs, int fps = 24, int width = 1280, int height = 720)
        {
            this.m_Fps = fps;
            this.m_Interval = 1.0f / fps;
            int bufferSize = width * height * 3;
            this.m_SourceTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            this.m_IntPtr = Marshal.AllocCoTaskMem(bufferSize);
            this.m_Datas = new byte[bufferSize];
            this.m_UpdateState = UpdateState.Default;
            this.m_Frame = (long)0;
            this.m_Timer = 0.0f;

            Task.Run(() => this.UpdateTexture(cameraID, getexpMs, bufferSize));
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="deltaTime">间隔时间</param>
        public void Update(float deltaTime)
        {
            this.m_Timer += deltaTime;
            if (this.m_Timer < this.m_Interval)
                return;

            switch (this.m_UpdateState)
            {
                case UpdateState.Default:
                    {
                        this.m_Color32s = this.m_SourceTexture.GetRawTextureData<Color32>();
                        this.m_UpdateState = UpdateState.CanUpdate;
                    }
                    break;
                case UpdateState.CanUpdate:
                case UpdateState.Updateing:
                    break;
                case UpdateState.UpdateEnd:
                    {
                        this.m_Timer = 0;
                        this.m_UpdateState = UpdateState.Default;
                        this.m_SourceTexture.Apply();
                        this.m_Frame++;
                    }
                    break;
                case UpdateState.UpdateFailed:
                    {
                        this.m_Timer = 0;
                        this.m_UpdateState = UpdateState.Default;
                        //Debug.LogWarning("更新失败！");
                        this.m_Frame++;
                    }
                    break;
                case UpdateState.Pause:
                    break;
                case UpdateState.End:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause() => this.m_UpdateState = UpdateState.Pause;

        /// <summary>
        /// 继续
        /// </summary>
        public void Resume()
        {
            if (this.m_UpdateState != UpdateState.Pause)
                return;

            this.m_UpdateState = UpdateState.CanUpdate;
        }

        /// <summary>
        /// 更新纹理
        /// </summary>
        private void UpdateTexture(int cameraID, Func<int> getexpMs, int bufferSize)
        {
            while (this.m_UpdateState != UpdateState.End)
            {
                if (this.m_UpdateState == UpdateState.CanUpdate)
                {
                    this.m_UpdateState = UpdateState.Updateing;

                    ASI_ERROR_CODE aSI_ERROR_CODE = ASICameraDll2.ASIGetVideoData(cameraID, this.m_IntPtr, bufferSize, getexpMs());
                    if (aSI_ERROR_CODE == ASI_ERROR_CODE.ASI_SUCCESS)
                    {
                        //1.推荐的方式
                        //Array.Clear(this.m_Datas, 0, this.m_Datas.Length);
                        Marshal.Copy(this.m_IntPtr, this.m_Datas, 0, this.m_Datas.Length);

                        //RGB三通道组成一个像素
                        for (int i = 0; i < bufferSize / 3; i++)
                        {
                            if (this.m_UpdateState == UpdateState.End || this.m_UpdateState == UpdateState.Pause)
                                continue;

                            int index = i * 3;
                            byte r = this.m_Datas[index];
                            byte g = this.m_Datas[index + 1];
                            byte b = this.m_Datas[index + 2];

                            //SDK获取到的原始数据的R通道和B通道是错位的
                            this.m_Color32s[i] = new Color32(b, g, r, 255);
                        }

                        //2.直接加载数据（注：以下代码为伪代码，不应该在非主线程之外调用,在主线程调用帧率过低）
                        /*
                            private void UpdateTexture()
                            {
                                ASI_ERROR_CODE aSI_ERROR_CODE = ASICameraDll2.ASIGetVideoData(this.m_Info.CameraID, this.m_IntPtr, this.m_BufferSize, this.m_ExpMs);
                                if (aSI_ERROR_CODE == ASI_ERROR_CODE.ASI_SUCCESS)
                                {
                                    this.m_Texture2D.LoadRawTextureData(this.m_IntPtr, this.m_BufferSize);
                                    this.m_Texture2D.Apply();
                                }
                            }
                        */

                        //3.逐像素（注：以下代码为伪代码，不应该在非主线程之外调用,在主线程调用帧率过低）
                        /*
                            private void UpdateTexture()
                            {
                                ASI_ERROR_CODE aSI_ERROR_CODE = ASICameraDll2.ASIGetVideoData(this.m_Info.CameraID, this.m_IntPtr, this.m_BufferSize, this.m_ExpMs);
                                if (aSI_ERROR_CODE == ASI_ERROR_CODE.ASI_SUCCESS)
                                {
                                    for (int i = 0; i < 1080; i++)
                                    {
                                        for (int j = 0; j < 1920; j++)
                                        {
                                            int index = i * 1920 + j;

                                            float r = this.m_Datas[index * 3];
                                            float g = this.m_Datas[index * 3 + 1];
                                            float b = this.m_Datas[index * 3 + 2];

                                            this.m_Texture2D.SetPixel(j, i, new Color(b / 255.0f, g / 255.0f, r / 255.0f, 1.0f));
                                        }
                                    }
                                    this.m_Texture2D.Apply();
                                }
                            }
                        */

                        //4.原生代码插件方式（注：以下代码为伪代码，实际未测试成功。)
                        //通过Texture2D.CreateExternalTexture、Texture2D.UpdateExternalTexture和Texture.GetNativeTexturePtr三个函数应该能在底层更新;
                        //https://docs.unity.cn/cn/2020.2/ScriptReference/Texture2D.CreateExternalTexture.html
                        //由于Unity底层函数是外部实现和本人对图形底层不了解，没有成功，但这应该是正确的方法。
                        /*
                            private void UpdateTexture()
                            {
                                this.m_IntPtr = Marshal.AllocCoTaskMem(this.m_Datas.Length);
                                this.m_Texture2D = Texture2D.CreateExternalTexture(1920, 1080, TextureFormat.RGB24, false, true, this.m_IntPtr);
                                ASI_ERROR_CODE aSI_ERROR_CODE = ASICameraDll2.ASIGetVideoData(this.m_Info.CameraID, this.m_IntPtr, this.m_Datas.Length, this.m_ExpMs);
                                if (aSI_ERROR_CODE == ASI_ERROR_CODE.ASI_SUCCESS)
                                {
                                    this.m_Texture2D.UpdateExternalTexture(this.m_IntPtr,);
                                    this.m_Texture2D.Apply();
                                }
                            }
                        */

                        if (this.m_UpdateState == UpdateState.Updateing)
                            this.m_UpdateState = UpdateState.UpdateEnd;
                    }
                    else
                    {
                        if (this.m_UpdateState == UpdateState.Updateing)
                            this.m_UpdateState = UpdateState.UpdateFailed;
                    }
                }
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.m_UpdateState = UpdateState.End;

            Array.Clear(this.m_Datas, 0, this.m_Datas.Length);
            this.m_Datas = null;
            Marshal.FreeCoTaskMem(this.m_IntPtr);
            this.m_IntPtr = default;
            GameObject.Destroy(this.m_SourceTexture);
            this.m_SourceTexture = null;
            this.m_Color32s = default;
            this.m_Fps = default;
            this.m_Interval = default;
            this.m_Frame = default;

            this.m_Timer = 0.0f;
        }
        #endregion
    }
}