using UnityEngine;
using static ZWOptical.ASISDK.ASICameraDll2;

namespace ASICamera
{
    /// <summary>
    /// ASI相机
    /// </summary>
    public class ASICamera : MonoBehaviour
    {
        #region Field
        /// <summary>
        /// ASI相机设备
        /// </summary>
        private ASICameraDevice m_ASICameraDevice = default;

        /// <summary>
        /// ASI相机索引
        /// </summary>
        [SerializeField]
        [Header("ASI相机索引")]
        [Range(0, 5)]
        private int m_CameraIndex = 0;

        /// <summary>
        /// 帧率
        /// </summary>
        [SerializeField]
        [Header("帧率")]
        private ASICameraFps m_Fps = ASICameraFps.Fps_24;

        /// <summary>
        /// 分辨率
        /// </summary>
        [SerializeField]
        [Header("分辨率")]
        private ASICameraResolution m_Resolution = ASICameraResolution.HD1024_768;

        /// <summary>
        /// 图像翻转
        /// </summary>
        [SerializeField]
        [Header("图像翻转")]
        private ASICameraFlip m_Filp = ASICameraFlip.Both;

        /// <summary>
        /// OnEnasble函数时是否自动打开相机
        /// </summary>
        [SerializeField]
        [Header("OnEnasble函数时是否自动打开相机")]
        private bool m_PlayOnEnable = true;
        #endregion

        #region Property
        /// <summary>
        /// 是否是无效的设备
        /// </summary>
        private bool IsInvalidASICameraDevice => this.m_ASICameraDevice == null;

        /// <summary>
        /// 获取ASI相机是否打开
        /// </summary>
        public bool IsOpen => this.IsInvalidASICameraDevice ? false : this.m_ASICameraDevice.IsOpen;

        /// <summary>
        /// 获取ASI相机信息
        /// </summary>
        /// <value>ASI相机信息</value>
        public ASI_CAMERA_INFO Info => this.IsInvalidASICameraDevice ? new ASI_CAMERA_INFO() : this.m_ASICameraDevice.Info;

        /*
        /// <summary>
        /// 获取ASI相机控制参数
        /// </summary>
        /// <value>ASI相机控制参数</value>
        public ASICameraControl ControlArgs => this.m_ASICameraDevice.ControlArgs;
        */

        /// <summary>
        /// 获取ASI相机名称
        /// </summary>
        /// <value>ASI相机名称</value>
        public string ASICameraName => this.IsInvalidASICameraDevice ? string.Empty : this.m_ASICameraDevice.ASICameraName;

        /// <summary>
        /// 获取ASI相机ID
        /// </summary>
        /// <value>ASI相机ID</value>
        public int ASICameraID => this.IsInvalidASICameraDevice ? -1 : this.m_ASICameraDevice.ASICameraID;

        /// <summary>
        /// 获取ASI相机帧率
        /// </summary>
        /// <value>ASI相机帧率</value>
        public int Fps => this.IsInvalidASICameraDevice ? -1 : this.m_ASICameraDevice.Fps;

        /// <summary>
        /// 获取ASI相机画面最大宽度
        /// </summary>
        /// <value>ASI相机画面最大宽度</value>
        public int MaxWidth => this.IsInvalidASICameraDevice ? 0 : this.m_ASICameraDevice.MaxWidth;

        /// <summary>
        /// 获取ASI相机画面宽度
        /// </summary>
        /// <value>ASI相机画面宽度</value>
        public int Width => this.IsInvalidASICameraDevice ? 0 : this.m_ASICameraDevice.Width;

        /// <summary>
        /// 获取ASI相机画面最大高度
        /// </summary>
        /// <value>ASI相机画面最大高度</value>
        public int MaxHeight => this.IsInvalidASICameraDevice ? 0 : this.m_ASICameraDevice.MaxHeight;

        /// <summary>
        /// 获取ASI相机画面高度
        /// </summary>
        /// <value>ASI相机画面高度</value>
        public int Height => this.IsInvalidASICameraDevice ? 0 : this.m_ASICameraDevice.Height;

        /// <summary>
        /// 获取是否支持机械快门
        /// </summary>
        /// <value>是否支持机械快门</value>
        public bool MechanicalShutter => this.IsInvalidASICameraDevice ? false : this.m_ASICameraDevice.MechanicalShutter;

        /// <summary>
        /// 获取是否有ST4
        /// </summary>
        /// <value>是否有ST4</value>
        public bool ST4Port => this.IsInvalidASICameraDevice ? false : this.m_ASICameraDevice.ST4Port;

        /// <summary>
        /// 获取是否是彩色相机
        /// </summary>
        /// <value>是否是彩色相机</value>
        public bool IsColorCamera => this.IsInvalidASICameraDevice ? false : this.m_ASICameraDevice.IsColorCamera;

        /// <summary>
        /// 获取是否是冷冻相机
        /// </summary>
        /// <value>是否是冷冻相机</value>
        public bool IsCoolerCamera => this.IsInvalidASICameraDevice ? false : this.m_ASICameraDevice.IsCoolerCamera;

        /// <summary>
        /// 获取是否工作为USB3.0
        /// </summary>
        /// <value>是否工作为USB3.0</value>
        public bool IsUSB3Host => this.IsInvalidASICameraDevice ? false : this.m_ASICameraDevice.IsUSB3Host;

        /// <summary>
        /// 获取是否是USB3相机
        /// </summary>
        /// <value>是否是USB3相机</value>
        public bool IsUSB3Camera => this.IsInvalidASICameraDevice ? false : this.m_ASICameraDevice.IsUSB3Camera;

        /// <summary>
        /// 获取ASI相机纹理
        /// </summary>
        /// <value>相机纹理</value>
        public RenderTexture OutputTexture
        {
            get
            {
                if (this.m_ASICameraDevice == null || !this.m_ASICameraDevice.IsOpen)
                    return null;

                return this.m_ASICameraDevice.OutputTexture;
            }

        }
        #endregion

        private void OnEnable()
        {
            if (this.m_PlayOnEnable)
                this.Play(this.m_CameraIndex, this.m_Fps, this.m_Resolution, this.m_Filp);
        }

        private void Update()
        {
            if (this.m_ASICameraDevice == null)
                return;

            this.m_ASICameraDevice.Update(Time.deltaTime);
        }

        private void OnDisable() => this.Stop();

        #region Function
        /// <summary>
        /// 打开ASI相机
        /// </summary>
        /// <param name="index">ASI相机索引</param>
        /// <param name="aSICameraFps">帧率</param>
        /// <param name="aSICameraResolution">分辨率</param>
        /// <param name="aSICameraFlip">翻转</param>
        /// <param name="autoMergedPixel">是否自动合并像素</param>
        /// <returns>是否打开成功</returns>
        public bool Play(int index = 0, ASICameraFps aSICameraFps = ASICameraFps.Fps_24,
                        ASICameraResolution aSICameraResolution = ASICameraResolution.HD720P,
                        ASICameraFlip aSICameraFlip = ASICameraFlip.Both)
        {
            if (this.m_ASICameraDevice != null)
            {
                Debug.LogWarning("ASI相机已打开。");
                return false;
            }

            this.m_ASICameraDevice = new ASICameraDevice();
            int fps = default;
            switch (this.m_Fps)
            {
                case ASICameraFps.Fps_5:
                    fps = 5;
                    break;
                case ASICameraFps.Fps_8:
                    fps = 8;
                    break;
                case ASICameraFps.Fps_15:
                    fps = 15;
                    break;
                case ASICameraFps.Fps_24:
                    fps = 24;
                    break;
                case ASICameraFps.Fps_30:
                    fps = 30;
                    break;
                case ASICameraFps.Fps_35:
                    fps = 35;
                    break;
                case ASICameraFps.Fps_40:
                    fps = 40;
                    break;
                default:
                    break;
            }
            int width = default;
            int height = default;
            switch (this.m_Resolution)
            {
                case ASICameraResolution.HD1024_768:
                    {
                        width = 1024;
                        height = 768;
                    }
                    break;
                case ASICameraResolution.HD720P:
                    {
                        width = 1280;
                        height = 720;
                    }
                    break;
                case ASICameraResolution.HD960P:
                    {
                        width = 1280;
                        height = 960;
                    }
                    break;
                case ASICameraResolution.HD1080P:
                    {
                        width = 1920;
                        height = 1080;
                    }
                    break;
                case ASICameraResolution.HDMax:
                    {
                        width = 3552;
                        height = 3552;
                    }
                    break;
                default:
                    break;
            }

            //ASICameraControl asiCameraControl = new ASICameraControl();
            bool isOpen = this.m_ASICameraDevice.OpenASICamera(index, fps, width, height);
            this.SetFlip((int)this.m_Filp);
            this.SetExposure(10000, false);

            Debug.Log("Open ASICamera Finsih.");
            return isOpen;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause() => this.m_ASICameraDevice?.Pause();

        /// <summary>
        /// 继续
        /// </summary>
        public void Resume() => this.m_ASICameraDevice?.Resume();

        /// <summary>
        /// 设置ASI相机增益属性
        /// </summary>
        /// <param name="value">待设置的增益值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetGain(int value, bool auto = true) => this.IsInvalidASICameraDevice ? false : this.m_ASICameraDevice.SetGain(value, auto);

        /// <summary>
        /// 设置ASI相机曝光属性
        /// 单位微秒
        /// </summary>
        /// <param name="value">待设置的曝光值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetExposure(int value, bool auto = true) => this.IsInvalidASICameraDevice ? false : this.m_ASICameraDevice.SetExposure(value, auto);

        /// <summary>
        /// 设置ASI相机伽玛属性
        /// 范围1~100
        /// </summary>
        /// <param name="value">待设置的伽玛值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetGamma(int value, bool auto = true) => this.IsInvalidASICameraDevice ? false : this.m_ASICameraDevice.SetGamma(Mathf.Clamp(value, 1, 100), auto);

        /// <summary>
        /// 设置ASI相机白平衡的R通道属性
        /// </summary>
        /// <param name="value">待设置的白平衡的R通道值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetWhiteBalance_R(int value, bool auto = true) => this.IsInvalidASICameraDevice ? false : this.m_ASICameraDevice.SetWhiteBalance_R(value, auto);

        /// <summary>
        /// 设置ASI相机白平衡的B通道属性
        /// </summary>
        /// <param name="value">待设置的白平衡的B通道值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetWhiteBalance_B(int value, bool auto = true) => this.IsInvalidASICameraDevice ? false : this.m_ASICameraDevice.SetWhiteBalance_B(value, auto);

        /// <summary>
        /// 设置ASI相机亮度属性
        /// </summary>
        /// <param name="value">待设置的亮度值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetBrightness(int value, bool auto = true) => this.IsInvalidASICameraDevice ? false : this.m_ASICameraDevice.SetBrightness(value, auto);

        /// <summary>
        /// 设置ASI相机带宽占比属性
        /// </summary>
        /// <param name="value">待设置的带宽占比值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetBandwidthOverload(int value, bool auto = true) => this.IsInvalidASICameraDevice ? false : this.m_ASICameraDevice.SetBandwidthOverload(value, auto);

        /// <summary>
        /// 设置ASI相机超频属性
        /// </summary>
        /// <param name="value">待设置的超频值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetOverClock(int value, bool auto = true) => this.IsInvalidASICameraDevice ? false : this.m_ASICameraDevice.SetOverClock(value, auto);

        /// <summary>
        /// 设置ASI相机温度属性
        /// </summary>
        /// <param name="value">待设置的温度值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetTemperature(int value, bool auto = true) => this.IsInvalidASICameraDevice ? false : this.m_ASICameraDevice.SetTemperature(value, auto);

        /// <summary>
        /// 设置ASI相机图片翻转属性
        /// </summary>
        /// <param name="value">待设置的图片翻转值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetFlip(int value) => this.IsInvalidASICameraDevice ? false : this.m_ASICameraDevice.SetFlip(value);

        /// <summary>
        /// 设置ASI相机增益在自动调节时的最大值属性
        /// </summary>
        /// <param name="value">待设置的增益在自动调节时的最大值值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetAutoMaxGain(int value, bool auto = true) => this.IsInvalidASICameraDevice ? false : this.m_ASICameraDevice.SetAutoMaxGain(value, auto);

        /// <summary>
        /// 设置ASI相机曝光在自动调节时的最大值属性
        /// 单位毫秒
        /// </summary>
        /// <param name="value">待设置的曝光在自动调节时的最大值值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetAutoMaxExp(int value, bool auto = true) => this.IsInvalidASICameraDevice ? false : this.m_ASICameraDevice.SetAutoMaxExp(value, auto);

        /// <summary>
        /// 设置ASI相机亮度在自动调节时的最大值属性
        /// </summary>
        /// <param name="value">待设置的亮度在自动调节时的最大值值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetAutoMaxBrightness(int value, bool auto = true) => this.IsInvalidASICameraDevice ? false : this.m_ASICameraDevice.SetAutoMaxBrightness(value, auto);

        /// <summary>
        /// 设置ASI相机硬件合并属性
        /// </summary>
        /// <param name="value">待设置的硬件合并值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetHardwareBin(int value, bool auto = true) => this.IsInvalidASICameraDevice ? false : this.m_ASICameraDevice.SetHardwareBin(value, auto);

        /// <summary>
        /// 设置ASI相机高速模式属性
        /// </summary>
        /// <param name="value">待设置的高速模式值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetHighSpeedMode(int value, bool auto = true) => this.IsInvalidASICameraDevice ? false : this.m_ASICameraDevice.SetHighSpeedMode(value, auto);

        /// <summary>
        /// 设置ASI相机制冷功率(仅冷冻相机)属性
        /// </summary>
        /// <param name="value">待设置的制冷功率(仅冷冻相机)值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetCoolerPowerPerc(int value, bool auto = true) => this.IsInvalidASICameraDevice ? false : this.m_ASICameraDevice.SetCoolerPowerPerc(value, auto);

        /// <summary>
        /// 设置ASI相机目标温度属性
        /// </summary>
        /// <param name="value">待设置的目标温度值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetTargetTemp(int value, bool auto = true) => this.IsInvalidASICameraDevice ? false : this.m_ASICameraDevice.SetTargetTemp(value, auto);

        /// <summary>
        /// 设置ASI相机打开制冷 (仅冷冻相机)属性
        /// </summary>
        /// <param name="value">待设置的打开制冷 (仅冷冻相机)值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetCoolerOn(int value, bool auto = true) => this.IsInvalidASICameraDevice ? false : this.m_ASICameraDevice.SetCoolerOn(value, auto);

        /// <summary>
        /// 设置ASI相机MonoBin属性
        /// </summary>
        /// <param name="value">待设置的MonoBin值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetMonoBin(int value, bool auto = true) => this.IsInvalidASICameraDevice ? false : this.m_ASICameraDevice.SetMonoBin(value, auto);

        /// <summary>
        /// 设置ASI相机模式调整（只有1600 黑白相机支持）属性
        /// </summary>
        /// <param name="value">待设置的模式调整（只有1600 黑白相机支持）值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetPatternAdjust(int value, bool auto = true) => this.IsInvalidASICameraDevice ? false : this.m_ASICameraDevice.SetPatternAdjust(value, auto);

        /// <summary>
        /// 设置ASI相机保护玻璃加热属性
        /// </summary>
        /// <param name="value">待设置的保护玻璃加热值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetAntiDewHeater(int value, bool auto = true) => this.IsInvalidASICameraDevice ? false : this.m_ASICameraDevice.SetAntiDewHeater(value, auto);

        /// <summary>
        /// 设置ASI相机增益属性
        /// </summary>
        public int GetGain() => this.IsInvalidASICameraDevice ? -1 : this.m_ASICameraDevice.GetGain();

        /// <summary>
        /// 获取ASI相机曝光属性值
        /// 单位微秒
        /// </summary>
        public int GetExposure() => this.IsInvalidASICameraDevice ? -1 : this.m_ASICameraDevice.GetExposure();

        /// <summary>
        /// 获取ASI相机伽玛属性值
        /// 范围1~100
        /// </summary>
        public int GetGamma() => this.IsInvalidASICameraDevice ? -1 : this.m_ASICameraDevice.GetGamma();

        /// <summary>
        /// 获取ASI相机白平衡的R通道属性值
        /// </summary>
        public int GetWhiteBalance_R() => this.IsInvalidASICameraDevice ? -1 : this.m_ASICameraDevice.GetWhiteBalance_R();

        /// <summary>
        /// 获取ASI相机白平衡的B通道属性值
        /// </summary>
        public int GetWhiteBalance_B() => this.IsInvalidASICameraDevice ? -1 : this.m_ASICameraDevice.GetWhiteBalance_B();

        /// <summary>
        /// 获取ASI相机亮度属性值
        /// </summary>
        public int GetBrightness() => this.IsInvalidASICameraDevice ? -1 : this.m_ASICameraDevice.GetBrightness();

        /// <summary>
        /// 获取ASI相机带宽占比属性值
        /// </summary>
        public int GetBandwidthOverload() => this.IsInvalidASICameraDevice ? -1 : this.m_ASICameraDevice.GetBandwidthOverload();

        /// <summary>
        /// 获取ASI相机超频属性值
        /// </summary>
        public int GetOverClock() => this.IsInvalidASICameraDevice ? -1 : this.m_ASICameraDevice.GetOverClock();

        /// <summary>
        /// 获取ASI相机温度属性值
        /// </summary>
        public int GetTemperature() => this.IsInvalidASICameraDevice ? -1 : this.m_ASICameraDevice.GetTemperature();

        /// <summary>
        /// 获取ASI相机图片翻转属性值
        /// </summary>
        public int GetFlip() => this.IsInvalidASICameraDevice ? -1 : this.m_ASICameraDevice.GetFlip();

        /// <summary>
        /// 获取ASI相机增益在自动调节时的最大值属性值
        /// </summary>
        public int GetAutoMaxGain() => this.IsInvalidASICameraDevice ? -1 : this.m_ASICameraDevice.GetAutoMaxGain();

        /// <summary>
        /// 获取ASI相机曝光在自动调节时的最大值属性值
        /// 单位毫秒
        /// </summary>
        public int GetAutoMaxExp() => this.IsInvalidASICameraDevice ? -1 : this.m_ASICameraDevice.GetAutoMaxExp();

        /// <summary>
        /// 获取ASI相机亮度在自动调节时的最大值属性值
        /// </summary>
        public int GetAutoMaxBrightness() => this.IsInvalidASICameraDevice ? -1 : this.m_ASICameraDevice.GetAutoMaxBrightness();

        /// <summary>
        /// 获取ASI相机硬件合并属性值
        /// </summary>
        public int GetHardwareBin() => this.IsInvalidASICameraDevice ? -1 : this.m_ASICameraDevice.GetHardwareBin();

        /// <summary>
        /// 获取ASI相机高速模式属性值
        /// </summary>
        public int GetHighSpeedMode() => this.IsInvalidASICameraDevice ? -1 : this.m_ASICameraDevice.GetHighSpeedMode();

        /// <summary>
        /// 获取ASI相机制冷功率(仅冷冻相机)属性值
        /// </summary>
        public int GetCoolerPowerPerc() => this.IsInvalidASICameraDevice ? -1 : this.m_ASICameraDevice.GetCoolerPowerPerc();

        /// <summary>
        /// 获取ASI相机目标温度属性值
        /// </summary>
        public int GetTargetTemp() => this.IsInvalidASICameraDevice ? -1 : this.m_ASICameraDevice.GetTargetTemp();

        /// <summary>
        /// 获取ASI相机打开制冷 (仅冷冻相机)属性值
        /// </summary>
        public int GetCoolerOn() => this.IsInvalidASICameraDevice ? -1 : this.m_ASICameraDevice.GetCoolerOn();

        /// <summary>
        /// 获取ASI相机MonoBin属性值
        /// </summary>
        public int GetMonoBin() => this.IsInvalidASICameraDevice ? -1 : this.m_ASICameraDevice.GetMonoBin();

        /// <summary>
        /// 获取ASI相机模式调整（只有1600 黑白相机支持）属性值
        /// </summary>
        public int GetPatternAdjust() => this.IsInvalidASICameraDevice ? -1 : this.m_ASICameraDevice.GetPatternAdjust();

        /// <summary>
        /// 获取ASI相机保护玻璃加热属性值
        /// </summary>
        public int GetAntiDewHeater() => this.IsInvalidASICameraDevice ? -1 : this.m_ASICameraDevice.GetAntiDewHeater();

        /// <summary>
        /// 获取ASI相机指定序号的控制内容
        /// </summary>
        /// <param name="controlIndex">控件序号</param>
        /// <returns>控制内容</returns>
        public ASI_CONTROL_CAPS GetASIControlCaps(int controlIndex) => this.IsInvalidASICameraDevice ? new ASI_CONTROL_CAPS() : this.m_ASICameraDevice.GetASIControlCaps(controlIndex);

        /// <summary>
        /// 关闭ASI相机
        /// </summary>
        public void Stop()
        {
            this.m_ASICameraDevice?.Dispose();
            this.m_ASICameraDevice = null;

            Debug.Log("Close ASICamera Finsih.");
        }
        #endregion
    }
}