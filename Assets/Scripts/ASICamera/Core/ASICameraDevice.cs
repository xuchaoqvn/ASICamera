using System;
using UnityEngine;
using ZWOptical.ASISDK;
using static ZWOptical.ASISDK.ASICameraDll2;

namespace ASICamera
{
    /// <summary>
    /// ASI相机设备
    /// </summary>
    public class ASICameraDevice : IDisposable
    {
        #region Field
        /// <summary>
        /// ASI相机信息
        /// </summary>
        private ASI_CAMERA_INFO m_Info = default;

        /*
        /// <summary>
        /// ASI相机参数
        /// </summary>
        private ASICameraControl m_ControlArgs = default;
        */

        /// <summary>
        /// 纹理更新器
        /// </summary>
        private IUpdateTexturerHander m_UpdateTexturerHander = default;

        /// <summary>
        /// 帧率
        /// </summary>
        private int m_Fps;

        /// <summary>
        /// 画面宽度
        /// </summary>
        private int m_Width;

        /// <summary>
        /// 画面高度
        /// </summary>
        private int m_Height;

        /// <summary>
        /// ASI相机是否打开
        /// </summary>
        private bool m_IsOpen = default;

        /// <summary>
        /// 帧
        /// </summary>
        private long m_Frame;

        /// <summary>
        /// 纹理
        /// </summary>
        private RenderTexture m_OutputTexture;
        #endregion

        #region Property
        /// <summary>
        /// 获取ASI相机是否打开
        /// </summary>
        public bool IsOpen => this.m_IsOpen;

        /// <summary>
        /// 获取ASI相机信息
        /// </summary>
        /// <value>ASI相机信息</value>
        public ASI_CAMERA_INFO Info
        {
            get
            {
                if (!this.m_IsOpen)
                    return new ASI_CAMERA_INFO();

                return this.m_Info;
            }
        }

        /*
        /// <summary>
        /// 获取ASI相机控制参数
        /// </summary>
        /// <value>ASI相机控制参数</value>
        public ASICameraControl ControlArgs
        {
            get
            {
                if (!this.m_IsOpen)
                    return new ASICameraControl();

                return this.m_ControlArgs;
            }
        }
        */

        /// <summary>
        /// 获取ASI相机名称
        /// </summary>
        /// <value>ASI相机名称</value>
        public string ASICameraName
        {
            get
            {
                if (!this.m_IsOpen)
                    return string.Empty;

                return this.m_Info.Name;
            }
        }

        /// <summary>
        /// 获取ASI相机ID
        /// </summary>
        /// <value>ASI相机ID</value>
        public int ASICameraID
        {
            get
            {
                if (!this.m_IsOpen)
                    return -1;

                return this.m_Info.CameraID;
            }
        }

        /// <summary>
        /// 获取ASI相机帧率
        /// </summary>
        /// <value>ASI相机帧率</value>
        public int Fps
        {
            get
            {
                if (!this.m_IsOpen)
                    return -1;

                return this.m_Fps;
            }
        }

        /// <summary>
        /// 获取ASI相机画面最大宽度
        /// </summary>
        /// <value>ASI相机画面最大宽度</value>
        public int MaxWidth
        {
            get
            {
                if (!this.m_IsOpen)
                    return 0;

                return this.m_Info.MaxWidth;
            }
        }

        /// <summary>
        /// 获取ASI相机画面宽度
        /// </summary>
        /// <value>ASI相机画面宽度</value>
        public int Width
        {
            get
            {
                if (!this.m_IsOpen)
                    return 0;

                return this.m_Width;
            }
        }

        /// <summary>
        /// 获取ASI相机画面最大高度
        /// </summary>
        /// <value>ASI相机画面最大高度</value>
        public int MaxHeight
        {
            get
            {
                if (!this.m_IsOpen)
                    return 0;

                return this.m_Info.MaxHeight;
            }
        }

        /// <summary>
        /// 获取ASI相机画面高度
        /// </summary>
        /// <value>ASI相机画面高度</value>
        public int Height
        {
            get
            {
                if (!this.m_IsOpen)
                    return 0;

                return this.m_Height;
            }
        }

        /// <summary>
        /// 获取是否支持机械快门
        /// </summary>
        /// <value>是否支持机械快门</value>
        public bool MechanicalShutter
        {
            get
            {
                if (!this.m_IsOpen)
                    return false;

                return this.m_Info.MechanicalShutter == ASI_BOOL.ASI_TRUE;
            }
        }

        /// <summary>
        /// 获取是否有ST4
        /// </summary>
        /// <value>是否有ST4</value>
        public bool ST4Port
        {
            get
            {
                if (!this.m_IsOpen)
                    return false;

                return this.m_Info.ST4Port == ASI_BOOL.ASI_TRUE;
            }
        }

        /// <summary>
        /// 获取是否是彩色相机
        /// </summary>
        /// <value>是否是彩色相机</value>
        public bool IsColorCamera
        {
            get
            {
                if (!this.m_IsOpen)
                    return false;

                return this.m_Info.IsColorCam == ASI_BOOL.ASI_TRUE;
            }
        }

        /// <summary>
        /// 获取是否是冷冻相机
        /// </summary>
        /// <value>是否是冷冻相机</value>
        public bool IsCoolerCamera
        {
            get
            {
                if (!this.m_IsOpen)
                    return false;

                return this.m_Info.IsCoolerCam == ASI_BOOL.ASI_TRUE;
            }
        }

        /// <summary>
        /// 获取是否工作为USB3.0
        /// </summary>
        /// <value>是否工作为USB3.0</value>
        public bool IsUSB3Host
        {
            get
            {
                if (!this.m_IsOpen)
                    return false;

                return this.m_Info.IsUSB3Host == ASI_BOOL.ASI_TRUE;
            }
        }

        /// <summary>
        /// 获取是否是USB3相机
        /// </summary>
        /// <value>是否是USB3相机</value>
        public bool IsUSB3Camera
        {
            get
            {
                if (!this.m_IsOpen)
                    return false;

                return this.m_Info.IsUSB3Camera == ASI_BOOL.ASI_TRUE;
            }
        }

        /// <summary>
        /// 获取ASI相机纹理
        /// </summary>
        /// <value>相机纹理</value>
        public RenderTexture OutputTexture
        {
            get
            {
                if (!this.m_IsOpen)
                    return null;

                return this.m_OutputTexture;
            }
        }
        #endregion

        #region Function
        /// <summary>
        /// 打开ASI相机
        /// </summary>
        /// <param name="asiCameraIndex">ASI相机设备的索引</param>
        /// <param name="fps">ASI相机画面帧率</param>
        /// <param name="width">ASI相机画面宽度</param>
        /// <param name="height">ASI相机画面高度</param>
        /// <returns>是否打开成功</returns>
        public bool OpenASICamera(int asiCameraIndex, int fps, int width, int height)
        {
            if (this.m_IsOpen)
            {
                Debug.LogError("ASI相机已打开，请勿重复打开！");
                return false;
            }

            //获取当前链接的ASI相机数量
            int nums = ASICameraDll2.ASIGetNumOfConnectedCameras();
            if (nums <= 0 || asiCameraIndex >= nums)
            {
                Debug.LogError("未查找到可用ASI相机或找不到指定索引的ASI相机。");
                return false;
            }

            //获取ID为asiCameraIndex的ASI相机的信息
            ASI_ERROR_CODE code = ASICameraDll2.ASIGetCameraProperty(out this.m_Info, asiCameraIndex);
            if (code != ASI_ERROR_CODE.ASI_SUCCESS)
            {
                Debug.LogError("获取ASI相机的信息失败！");
                return false;
            }

            //相机ID
            int cameraID = this.m_Info.CameraID;
            //最大宽度
            int maxWidth = this.m_Info.MaxWidth;
            //最大高度
            int maxHeight = this.m_Info.MaxHeight;

            //打开相机
            code = ASICameraDll2.ASIOpenCamera(cameraID);
            if (code != ASI_ERROR_CODE.ASI_SUCCESS)
            {
                Debug.LogError("打开ASI相机失败！");
                return false;
            }

            //初始化相机
            code = ASICameraDll2.ASIInitCamera(cameraID);
            if (code != ASI_ERROR_CODE.ASI_SUCCESS)
            {
                Debug.LogError("初始化ASI相机失败！");
                return false;
            }

            //检测图片尺寸
            if (width > maxWidth || height > maxHeight)
            {
                Debug.LogError("设置的分辨率超出最大尺寸！");
                return false;
            }
            this.m_Width = width;
            this.m_Height = height;

            //设置图像尺寸和格式
            code = ASICameraDll2.ASISetROIFormat(cameraID, this.m_Width, this.m_Height, 1, ASI_IMG_TYPE.ASI_IMG_RGB24);
            if (code != ASI_ERROR_CODE.ASI_SUCCESS)
            {

                Debug.LogError("设置ASI相机的图像尺寸和格式失败！");
                return false;
            }

            //设置图像起始位置
            int startX = (maxWidth - this.m_Width) / 2;
            int startY = (maxHeight - this.m_Height) / 2;
            code = ASICameraDll2.ASISetStartPos(cameraID, startX, startY);
            if (code != ASI_ERROR_CODE.ASI_SUCCESS)
            {
                Debug.LogError("设置ASI相机的图像起始位置失败！");
                return false;
            }

            //设置控制参数
            //this.InitControlValue(cameraID, control);

            //开始捕捉画面
            code = ASICameraDll2.ASIStartVideoCapture(cameraID);
            if (code != ASI_ERROR_CODE.ASI_SUCCESS)
            {
                Debug.LogError("开始捕捉ASI相机的画面失败！");
                return false;
            }

            this.m_IsOpen = true;
            this.m_OutputTexture = RenderTexture.GetTemporary(this.m_Width, this.m_Height);
            this.m_OutputTexture.filterMode = FilterMode.Point;
            this.m_OutputTexture.wrapMode = TextureWrapMode.Repeat;
            this.m_UpdateTexturerHander = new DefaultUpdateTexturerHander();
            this.m_Fps = fps;
            //曝光时间(在画面捕捉函数中推荐的参数为：曝光时间（单位：微秒） * 2 + 500毫秒)
            this.m_UpdateTexturerHander.RunUpdate(cameraID, () => this.GetExposure() * 2 + 500, this.m_Fps, width, height);

            return true;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="deltaTime">帧时间</param>
        public void Update(float deltaTime)
        {
            if (!this.m_IsOpen)
                return;

            this.m_UpdateTexturerHander.Update(deltaTime);

            long frame = this.m_UpdateTexturerHander.Frame;
            if (this.m_Frame != frame)
            {
                this.m_Frame = frame;
                this.UpdateTexture();
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause() => this.m_UpdateTexturerHander.Pause();

        /// <summary>
        /// 继续
        /// </summary>
        public void Resume() => this.m_UpdateTexturerHander.Resume();

        /// <summary>
        /// 更新纹理
        /// </summary>
        private void UpdateTexture() => Graphics.Blit(this.m_UpdateTexturerHander.SourceTexture, this.m_OutputTexture);

        /// <summary>
        /// 初始化ASI相机的控制参数
        /// </summary>
        /// <param name="cameraID">ASI相机ID</param>
        /// <param name="control">SI相机的控制参数</param>
        private void InitControlValue(int cameraID, ASICameraControl control)
        {
            Array array = Enum.GetValues(typeof(ASI_CONTROL_TYPE));
            for (int i = 0; i < array.Length; i++)
            {
                ASI_CONTROL_TYPE type = (ASI_CONTROL_TYPE)array.GetValue(i);
                ASICameraArgs args = control[type];
                ASICameraDll2.ASISetControlValue(cameraID, type, args.Value, args.Auto ? ASI_BOOL.ASI_TRUE : ASI_BOOL.ASI_FALSE);
            }
        }

        /// <summary>
        /// 设置ASI相机增益属性
        /// </summary>
        /// <param name="value">待设置的增益值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetGain(int value, bool auto = true) => this.SetControlValue(ASI_CONTROL_TYPE.ASI_GAIN, value, auto);

        /// <summary>
        /// 设置ASI相机曝光属性
        /// 单位微秒
        /// </summary>
        /// <param name="value">待设置的曝光值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetExposure(int value, bool auto = true) => this.SetControlValue(ASI_CONTROL_TYPE.ASI_EXPOSURE, value, auto);

        /// <summary>
        /// 设置ASI相机伽玛属性
        /// 范围1~100
        /// </summary>
        /// <param name="value">待设置的伽玛值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetGamma(int value, bool auto = true) => this.SetControlValue(ASI_CONTROL_TYPE.ASI_GAMMA, Mathf.Clamp(value, 1, 100), auto);

        /// <summary>
        /// 设置ASI相机白平衡的R通道属性
        /// </summary>
        /// <param name="value">待设置的白平衡的R通道值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetWhiteBalance_R(int value, bool auto = true) => this.SetControlValue(ASI_CONTROL_TYPE.ASI_WB_R, value, auto);

        /// <summary>
        /// 设置ASI相机白平衡的B通道属性
        /// </summary>
        /// <param name="value">待设置的白平衡的B通道值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetWhiteBalance_B(int value, bool auto = true) => this.SetControlValue(ASI_CONTROL_TYPE.ASI_WB_B, value, auto);

        /// <summary>
        /// 设置ASI相机亮度属性
        /// </summary>
        /// <param name="value">待设置的亮度值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetBrightness(int value, bool auto = true) => this.SetControlValue(ASI_CONTROL_TYPE.ASI_BRIGHTNESS, value, auto);

        /// <summary>
        /// 设置ASI相机带宽占比属性
        /// </summary>
        /// <param name="value">待设置的带宽占比值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetBandwidthOverload(int value, bool auto = true) => this.SetControlValue(ASI_CONTROL_TYPE.ASI_BANDWIDTHOVERLOAD, value, auto);

        /// <summary>
        /// 设置ASI相机超频属性
        /// </summary>
        /// <param name="value">待设置的超频值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetOverClock(int value, bool auto = true) => this.SetControlValue(ASI_CONTROL_TYPE.ASI_OVERCLOCK, value, auto);

        /// <summary>
        /// 设置ASI相机温度属性
        /// </summary>
        /// <param name="value">待设置的温度值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetTemperature(int value, bool auto = true) => this.SetControlValue(ASI_CONTROL_TYPE.ASI_TEMPERATURE, value, auto);

        /// <summary>
        /// 设置ASI相机图片翻转属性
        /// </summary>
        /// <param name="value">待设置的图片翻转值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetFlip(int value, bool auto = false) => this.SetControlValue(ASI_CONTROL_TYPE.ASI_FLIP, value, auto);

        /// <summary>
        /// 设置ASI相机增益在自动调节时的最大值属性
        /// </summary>
        /// <param name="value">待设置的增益在自动调节时的最大值值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetAutoMaxGain(int value, bool auto = true) => this.SetControlValue(ASI_CONTROL_TYPE.ASI_AUTO_MAX_GAIN, value, auto);

        /// <summary>
        /// 设置ASI相机曝光在自动调节时的最大值属性
        /// 单位毫秒
        /// </summary>
        /// <param name="value">待设置的曝光在自动调节时的最大值值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetAutoMaxExp(int value, bool auto = true) => this.SetControlValue(ASI_CONTROL_TYPE.ASI_AUTO_MAX_EXP, value * 1000, auto);

        /// <summary>
        /// 设置ASI相机亮度在自动调节时的最大值属性
        /// </summary>
        /// <param name="value">待设置的亮度在自动调节时的最大值值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetAutoMaxBrightness(int value, bool auto = true) => this.SetControlValue(ASI_CONTROL_TYPE.ASI_AUTO_MAX_BRIGHTNESS, value, auto);

        /// <summary>
        /// 设置ASI相机硬件合并属性
        /// </summary>
        /// <param name="value">待设置的硬件合并值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetHardwareBin(int value, bool auto = true) => this.SetControlValue(ASI_CONTROL_TYPE.ASI_HARDWARE_BIN, value, auto);

        /// <summary>
        /// 设置ASI相机高速模式属性
        /// </summary>
        /// <param name="value">待设置的高速模式值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetHighSpeedMode(int value, bool auto = true) => this.SetControlValue(ASI_CONTROL_TYPE.ASI_HIGH_SPEED_MODE, value, auto);

        /// <summary>
        /// 设置ASI相机制冷功率(仅冷冻相机)属性
        /// </summary>
        /// <param name="value">待设置的制冷功率(仅冷冻相机)值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetCoolerPowerPerc(int value, bool auto = true) => this.SetControlValue(ASI_CONTROL_TYPE.ASI_COOLER_POWER_PERC, value, auto);

        /// <summary>
        /// 设置ASI相机目标温度属性
        /// </summary>
        /// <param name="value">待设置的目标温度值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetTargetTemp(int value, bool auto = true) => this.SetControlValue(ASI_CONTROL_TYPE.ASI_TARGET_TEMP, value, auto);

        /// <summary>
        /// 设置ASI相机打开制冷 (仅冷冻相机)属性
        /// </summary>
        /// <param name="value">待设置的打开制冷 (仅冷冻相机)值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetCoolerOn(int value, bool auto = true) => this.SetControlValue(ASI_CONTROL_TYPE.ASI_COOLER_ON, value, auto);

        /// <summary>
        /// 设置ASI相机MonoBin属性
        /// </summary>
        /// <param name="value">待设置的MonoBin值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetMonoBin(int value, bool auto = true) => this.SetControlValue(ASI_CONTROL_TYPE.ASI_MONO_BIN, value, auto);

        /// <summary>
        /// 设置ASI相机模式调整（只有1600 黑白相机支持）属性
        /// </summary>
        /// <param name="value">待设置的模式调整（只有1600 黑白相机支持）值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetPatternAdjust(int value, bool auto = true) => this.SetControlValue(ASI_CONTROL_TYPE.ASI_PATTERN_ADJUST, value, auto);

        /// <summary>
        /// 设置ASI相机保护玻璃加热属性
        /// </summary>
        /// <param name="value">待设置的保护玻璃加热值</param>
        /// <param name="auto">是否自动调节</param>
        public bool SetAntiDewHeater(int value, bool auto = true) => this.SetControlValue(ASI_CONTROL_TYPE.ASI_ANTI_DEW_HEATER, value, auto);

        /// <summary>
        /// 设置ASI相机属性值
        /// </summary>
        /// <param name="type">属性类型</param>
        /// <param name="value">待设置的值</param>
        private bool SetControlValue(ASI_CONTROL_TYPE type, int value, bool auto)
        {
            if (!this.m_IsOpen)
                return false;

            ASI_ERROR_CODE code = ASICameraDll2.ASISetControlValue(this.m_Info.CameraID, type, value, auto ? ASI_BOOL.ASI_TRUE : ASI_BOOL.ASI_FALSE);
            if (code != ASI_ERROR_CODE.ASI_SUCCESS)
                return false;

            //this.m_ControlArgs[type] = new ASICameraArgs() { Value = value, Auto = auto };

            return true;
        }

        /// <summary>
        /// 设置ASI相机增益属性
        /// </summary>
        public int GetGain() => this.GetControlValue(ASI_CONTROL_TYPE.ASI_GAIN);

        /// <summary>
        /// 获取ASI相机曝光属性值
        /// 单位微秒
        /// </summary>
        public int GetExposure() => this.GetControlValue(ASI_CONTROL_TYPE.ASI_EXPOSURE);

        /// <summary>
        /// 获取ASI相机伽玛属性值
        /// 范围1~100
        /// </summary>
        public int GetGamma() => this.GetControlValue(ASI_CONTROL_TYPE.ASI_GAMMA);

        /// <summary>
        /// 获取ASI相机白平衡的R通道属性值
        /// </summary>
        public int GetWhiteBalance_R() => this.GetControlValue(ASI_CONTROL_TYPE.ASI_WB_R);

        /// <summary>
        /// 获取ASI相机白平衡的B通道属性值
        /// </summary>
        public int GetWhiteBalance_B() => this.GetControlValue(ASI_CONTROL_TYPE.ASI_WB_B);

        /// <summary>
        /// 获取ASI相机亮度属性值
        /// </summary>
        public int GetBrightness() => this.GetControlValue(ASI_CONTROL_TYPE.ASI_BRIGHTNESS);

        /// <summary>
        /// 获取ASI相机带宽占比属性值
        /// </summary>
        public int GetBandwidthOverload() => this.GetControlValue(ASI_CONTROL_TYPE.ASI_BANDWIDTHOVERLOAD);

        /// <summary>
        /// 获取ASI相机超频属性值
        /// </summary>
        public int GetOverClock() => this.GetControlValue(ASI_CONTROL_TYPE.ASI_OVERCLOCK);

        /// <summary>
        /// 获取ASI相机温度属性值
        /// </summary>
        public int GetTemperature() => this.GetControlValue(ASI_CONTROL_TYPE.ASI_TEMPERATURE);

        /// <summary>
        /// 获取ASI相机图片翻转属性值
        /// </summary>
        public int GetFlip() => this.GetControlValue(ASI_CONTROL_TYPE.ASI_FLIP);

        /// <summary>
        /// 获取ASI相机增益在自动调节时的最大值属性值
        /// </summary>
        public int GetAutoMaxGain() => this.GetControlValue(ASI_CONTROL_TYPE.ASI_AUTO_MAX_GAIN);

        /// <summary>
        /// 获取ASI相机曝光在自动调节时的最大值属性值
        /// 单位毫秒
        /// </summary>
        public int GetAutoMaxExp() => this.GetControlValue(ASI_CONTROL_TYPE.ASI_AUTO_MAX_EXP);

        /// <summary>
        /// 获取ASI相机亮度在自动调节时的最大值属性值
        /// </summary>
        public int GetAutoMaxBrightness() => this.GetControlValue(ASI_CONTROL_TYPE.ASI_AUTO_MAX_BRIGHTNESS);

        /// <summary>
        /// 获取ASI相机硬件合并属性值
        /// </summary>
        public int GetHardwareBin() => this.GetControlValue(ASI_CONTROL_TYPE.ASI_HARDWARE_BIN);

        /// <summary>
        /// 获取ASI相机高速模式属性值
        /// </summary>
        public int GetHighSpeedMode() => this.GetControlValue(ASI_CONTROL_TYPE.ASI_HIGH_SPEED_MODE);

        /// <summary>
        /// 获取ASI相机制冷功率(仅冷冻相机)属性值
        /// </summary>
        public int GetCoolerPowerPerc() => this.GetControlValue(ASI_CONTROL_TYPE.ASI_COOLER_POWER_PERC);

        /// <summary>
        /// 获取ASI相机目标温度属性值
        /// </summary>
        public int GetTargetTemp() => this.GetControlValue(ASI_CONTROL_TYPE.ASI_TARGET_TEMP);

        /// <summary>
        /// 获取ASI相机打开制冷 (仅冷冻相机)属性值
        /// </summary>
        public int GetCoolerOn() => this.GetControlValue(ASI_CONTROL_TYPE.ASI_COOLER_ON);

        /// <summary>
        /// 获取ASI相机MonoBin属性值
        /// </summary>
        public int GetMonoBin() => this.GetControlValue(ASI_CONTROL_TYPE.ASI_MONO_BIN);

        /// <summary>
        /// 获取ASI相机模式调整（只有1600 黑白相机支持）属性值
        /// </summary>
        public int GetPatternAdjust() => this.GetControlValue(ASI_CONTROL_TYPE.ASI_PATTERN_ADJUST);

        /// <summary>
        /// 获取ASI相机保护玻璃加热属性值
        /// </summary>
        public int GetAntiDewHeater() => this.GetControlValue(ASI_CONTROL_TYPE.ASI_ANTI_DEW_HEATER);

        /// <summary>
        /// 获取ASI相机属性值
        /// </summary>
        /// <param name="type">属性类型</param>
        /// <returns>属性值</returns>
        private int GetControlValue(ASI_CONTROL_TYPE type)
        {
            if (!this.m_IsOpen)
            {
                Debug.LogWarning("ASI相机未打开，获取失败！");
                return default;
            }

            int controlValue = ASICameraDll2.ASIGetControlValue(this.m_Info.CameraID, type);
            /* if (controlValue != this.m_ControlArgs[type].Value)
            {
                ASICameraArgs args = this.m_ControlArgs[type];
                this.m_ControlArgs[type] = new ASICameraArgs() { Value = controlValue, Auto = args.Auto };
            } */
            return controlValue;
        }

        /// <summary>
        /// 获取ASI相机指定序号的控制内容
        /// </summary>
        /// <param name="controlIndex">控件序号</param>
        /// <returns>控制内容</returns>
        public ASI_CONTROL_CAPS GetASIControlCaps(int controlIndex)
        {
            if (!this.m_IsOpen)
            {
                Debug.LogWarning("ASI相机未打开，获取失败！");
                return new ASI_CONTROL_CAPS();
            }

            ASI_ERROR_CODE code = ASICameraDll2.ASIGetNumOfControls(this.m_Info.CameraID, out int numControls);
            if (code != ASI_ERROR_CODE.ASI_SUCCESS)
            {
                Debug.LogError("获取ASI相机指定类型的控制内容失败！");
                return new ASI_CONTROL_CAPS();
            }

            if (controlIndex < 0 || controlIndex >= numControls)
            {
                Debug.LogError("未查找到指定序号的控件内容！");
                return new ASI_CONTROL_CAPS();
            }

            code = ASICameraDll2.ASIGetControlCaps(this.m_Info.CameraID, controlIndex, out ASI_CONTROL_CAPS controlCaps);
            if (code != ASI_ERROR_CODE.ASI_SUCCESS)
            {
                Debug.LogError("获取ASI相机指定类型的控制内容失败！");
                return new ASI_CONTROL_CAPS();
            }

            return controlCaps;
        }

        /// <summary>
        /// 关闭默认的ASI相机
        /// </summary>
        /// <returns>是否关闭成功</returns>
        public bool CloseASICamera()
        {
            if (!this.m_IsOpen)
            {
                Debug.LogWarning("默认的ASI相机未打开。");
                return false;
            }

            int cameraID = this.m_Info.CameraID;

            //停止默认ASI相机画面捕捉
            ASI_ERROR_CODE code = ASICameraDll2.ASIStopVideoCapture(cameraID);
            if (code != ASI_ERROR_CODE.ASI_SUCCESS)
            {
                Debug.LogError("停止捕捉默认ASI相机的画面失败！");
                return false;
            }

            //关闭默认ASI相机
            code = ASICameraDll2.ASICloseCamera(cameraID);
            if (code != ASI_ERROR_CODE.ASI_SUCCESS)
            {
                Debug.LogError("关闭默认ASI相机失败！");
                return false;
            }

            this.m_Info = default;
            this.m_IsOpen = false;
            /* this.m_ExpMs = default;
            GameObject.Destroy(this.m_Texture2D);
            this.m_Texture2D = null;
            Marshal.FreeCoTaskMem(this.m_IntPtr);
            this.m_IntPtr = default;
            this.m_BufferSize = default; */

            return true;
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            if (!this.m_IsOpen)
                return;

            int cameraID = this.m_Info.CameraID;

            //停止默认ASI相机画面捕捉
            ASI_ERROR_CODE code = ASICameraDll2.ASIStopVideoCapture(cameraID);
            if (code != ASI_ERROR_CODE.ASI_SUCCESS)
                Debug.LogError("停止捕捉默认ASI相机的画面失败！");

            //关闭默认ASI相机
            code = ASICameraDll2.ASICloseCamera(cameraID);
            if (code != ASI_ERROR_CODE.ASI_SUCCESS)
            {
                Debug.LogError("关闭默认ASI相机失败！");
                return;
            }

            this.m_IsOpen = false;
            this.m_Width = default;
            this.m_Height = default;
            this.m_Info = default;
            //this.m_ControlArgs = default;
            RenderTexture.ReleaseTemporary(this.m_OutputTexture);
            this.m_OutputTexture = null;
            this.m_UpdateTexturerHander.Dispose();
            this.m_UpdateTexturerHander = null;
        }
        #endregion
    }
}