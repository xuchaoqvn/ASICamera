using System;
using static ZWOptical.ASISDK.ASICameraDll2;

namespace ASICamera
{
    /// <summary>
    /// ASI相机参数
    /// </summary>
    [Serializable]
    public struct ASICameraArgs
    {
        /// <summary>
        /// 参数值
        /// </summary>
        public int Value;

        /// <summary>
        /// 是否自动
        /// </summary>
        public bool Auto;
    }

    /// <summary>
    /// ASI相机控制参数
    /// </summary>
    [Serializable]
    public class ASICameraControl
    {
        /*
        /// <summary>
        /// 增益
        /// </summary>
        public int Gain;

        /// <summary>
        /// 曝光
        /// </summary>
        public int Exposure;

        /// <summary>
        /// 伽马
        /// </summary>
        public int Gamma;

        /// <summary>
        /// 白平衡R通道
        /// </summary>
        public int WhiteBalance_R;

        /// <summary>
        /// 白平衡B通道
        /// </summary>
        public int WhiteBalance_B;

        /// <summary>
        /// 亮度
        /// </summary>
        public int Brightness;

        /// <summary>
        /// 带宽占比
        /// </summary>
        public int BandwidthOverload;

        /// <summary>
        /// 超频
        /// </summary>
        public int OverClock;

        /// <summary>
        /// 温度
        /// </summary>
        public int Temperature;

        /// <summary>
        /// 翻转
        /// </summary>
        public int Flip;

        /// <summary>
        /// 增益在自动调节时的最大值
        /// </summary>
        public int AutoMaxGain;

        /// <summary>
        /// 曝光在自动调节时的最大值
        /// </summary>
        public int AutoMaxExp;

        /// <summary>
        /// 亮度在自动调节时的最大值
        /// </summary>
        public int AutoMaxBrightness;

        /// <summary>
        /// 硬件合并
        /// </summary>
        public int HardwareBin;

        /// <summary>
        /// 高速模式
        /// </summary>
        public int HighSpeedMode;

        /// <summary>
        /// 制冷功率(仅冷冻相机)
        /// </summary>
        public int CoolerPowerPerc;

        /// <summary>
        /// 目标温度
        /// </summary>
        public int TargetTemp;

        /// <summary>
        /// 打开制冷 (仅冷冻相机)
        /// </summary>
        public int CoolerOn;

        /// <summary>
        /// MonoBin
        /// </summary>
        public int MonoBin;

        /// <summary>
        /// 风扇（？）
        /// </summary>
        public int FanOn;

        /// <summary>
        /// 模式调整（只有1600 黑白相机支持）
        /// </summary>
        public int PatternAdjust;

        /// <summary>
        /// 保护玻璃加热
        /// </summary>
        public int AntiDewHeater;

        /// <summary>
        /// 湿度
        /// </summary>
        public int Humidity;

        /// <summary>
        /// 开启双倍速率
        /// </summary>
        public int EnableDdr;
        */

        /// <summary>
        /// 控制参数名称
        /// </summary>
        /// <value></value>
        private string[] ControlNames = new string[]
        {
        "增益",
        "曝光",
        "伽马",
        "白平衡R通道",
        "白平衡B通道",
        "亮度",
        "带宽占比",
        "超频",
        "温度",
        "翻转",
        "增益在自动调节时的最大值",
        "曝光在自动调节时的最大值",
        "亮度在自动调节时的最大值",
        "硬件合并",
        "高速模式",
        "制冷功率(仅冷冻相机)",
        "目标温度",
        "打开制冷 (仅冷冻相机)",
        "MonoBin",
        "风扇（？）",
        "模式调整（只有1600 黑白相机支持）",
        "保护玻璃加热",
        "湿度",
        "开启双倍速率"
        };

        /// <summary>
        /// 控制参数
        /// </summary>
        public ASICameraArgs[] ASICameraArgs = new ASICameraArgs[]
        {
        //增益
        new ASICameraArgs() { Value = 200, Auto = true},
        //曝光
        new ASICameraArgs() { Value = 10, Auto = true},
        //伽马
        new ASICameraArgs() { Value = 0, Auto = false},
        //白平衡R通道
        new ASICameraArgs() { Value = 55, Auto = true},
        //白平衡B通道
        new ASICameraArgs() { Value = 75, Auto = true},
        //亮度
        new ASICameraArgs() { Value = 1, Auto = false},
        //带宽占比
        new ASICameraArgs() { Value = 50, Auto = true},
        //超频
        new ASICameraArgs() { Value = 0, Auto = false},
        //温度
        new ASICameraArgs() { Value = 0, Auto = false},
        //翻转
        new ASICameraArgs() { Value = 3, Auto = false},
        //增益在自动调节时的最大值
        new ASICameraArgs() { Value = 300, Auto = false},
        //曝光在自动调节时的最大值
        new ASICameraArgs() { Value = 100, Auto = false},
        //亮度在自动调节时的最大值
        new ASICameraArgs() { Value = 100, Auto = false},
        //硬件合并
        new ASICameraArgs() { Value = 0, Auto = false},
        //高速模式
        new ASICameraArgs() { Value = 0, Auto = false},
        //制冷功率（仅冷冻相机）
        new ASICameraArgs() { Value = 0, Auto = false},
        //目标温度
        new ASICameraArgs() { Value = 0, Auto = false},
        //打开制冷（仅冷冻相机）
        new ASICameraArgs() { Value = 0, Auto = false},
        //MonoBin
        new ASICameraArgs() { Value = 0, Auto = false},
        //风扇
        new ASICameraArgs() { Value = 0, Auto = false},
        //模式调整（只有1600 黑白相机支持）
        new ASICameraArgs() { Value = 0, Auto = false},
        //保护玻璃加热
        new ASICameraArgs() { Value = 0, Auto = false},
        //湿度
        new ASICameraArgs() { Value = 0, Auto = false},
        //开启双倍速率
        new ASICameraArgs() { Value = 0, Auto = false},
        };

        /// <summary>
        /// 控制参数索引器
        /// </summary>
        /// <value>参数值</value>
        public ASICameraArgs this[ASI_CONTROL_TYPE type]
        {
            get => this.ASICameraArgs[(int)type];
            set => this.ASICameraArgs[(int)type] = value;
        }

        /// <summary>
        /// 获取控制参数名称
        /// </summary>
        /// <param name="type">控制参数 类型</param>
        /// <returns>控制参数名称</returns>
        public string ControlName(ASI_CONTROL_TYPE type) => this.ControlNames[(int)type];

        /*
        /// <summary>
        /// 获取控制参数值
        /// </summary>
        /// <param name="type">参数类型</param>
        /// <returns>参数值</returns>
        private int GetControlValue(ASI_CONTROL_TYPE type)
        {
            int controlValue = default;
            switch (type)
            {
                case ASI_CONTROL_TYPE.ASI_GAIN:
                    controlValue = this.Gain;
                    break;
                case ASI_CONTROL_TYPE.ASI_EXPOSURE:
                    controlValue = this.Exposure;
                    break;
                case ASI_CONTROL_TYPE.ASI_GAMMA:
                    controlValue = this.Gamma;
                    break;
                case ASI_CONTROL_TYPE.ASI_WB_R:
                    controlValue = this.WhiteBalance_R;
                    break;
                case ASI_CONTROL_TYPE.ASI_WB_B:
                    controlValue = this.WhiteBalance_B;
                    break;
                case ASI_CONTROL_TYPE.ASI_BRIGHTNESS:
                    controlValue = this.Brightness;
                    break;
                case ASI_CONTROL_TYPE.ASI_BANDWIDTHOVERLOAD:
                    controlValue = this.BandwidthOverload;
                    break;
                case ASI_CONTROL_TYPE.ASI_OVERCLOCK:
                    controlValue = this.OverClock;
                    break;
                case ASI_CONTROL_TYPE.ASI_TEMPERATURE:
                    controlValue = this.Temperature;
                    break;
                case ASI_CONTROL_TYPE.ASI_FLIP:
                    controlValue = this.Flip;
                    break;
                case ASI_CONTROL_TYPE.ASI_AUTO_MAX_GAIN:
                    controlValue = this.AutoMaxGain;
                    break;
                case ASI_CONTROL_TYPE.ASI_AUTO_MAX_EXP:
                    controlValue = this.AutoMaxExp;
                    break;
                case ASI_CONTROL_TYPE.ASI_AUTO_MAX_BRIGHTNESS:
                    controlValue = this.AutoMaxBrightness;
                    break;
                case ASI_CONTROL_TYPE.ASI_HARDWARE_BIN:
                    controlValue = this.HardwareBin;
                    break;
                case ASI_CONTROL_TYPE.ASI_HIGH_SPEED_MODE:
                    controlValue = this.HighSpeedMode;
                    break;
                case ASI_CONTROL_TYPE.ASI_COOLER_POWER_PERC:
                    controlValue = this.CoolerPowerPerc;
                    break;
                case ASI_CONTROL_TYPE.ASI_TARGET_TEMP:
                    controlValue = this.TargetTemp;
                    break;
                case ASI_CONTROL_TYPE.ASI_COOLER_ON:
                    controlValue = this.CoolerOn;
                    break;
                case ASI_CONTROL_TYPE.ASI_MONO_BIN:
                    controlValue = this.MonoBin;
                    break;
                case ASI_CONTROL_TYPE.ASI_FAN_ON:
                    controlValue = this.FanOn;
                    break;
                case ASI_CONTROL_TYPE.ASI_PATTERN_ADJUST:
                    controlValue = this.PatternAdjust;
                    break;
                case ASI_CONTROL_TYPE.ASI_ANTI_DEW_HEATER:
                    controlValue = this.AntiDewHeater;
                    break;
                case ASI_CONTROL_TYPE.ASI_HUMIDITY:
                    controlValue = this.Humidity;
                    break;
                case ASI_CONTROL_TYPE.ASI_ENABLE_DDR:
                    controlValue = this.EnableDdr;
                    break;
                default:
                    break;
            }

            return controlValue;
        }

        /// <summary>
        /// 设置控制参数
        /// </summary>
        /// <param name="type">参数类型</param>
        /// <param name="value">参数值</param>
        private void SetControlValue(ASI_CONTROL_TYPE type, int value)
        {
            switch (type)
            {
                case ASI_CONTROL_TYPE.ASI_GAIN:
                    this.Gain = value;
                    break;
                case ASI_CONTROL_TYPE.ASI_EXPOSURE:
                    this.Exposure = value;
                    break;
                case ASI_CONTROL_TYPE.ASI_GAMMA:
                    this.Gamma = value;
                    break;
                case ASI_CONTROL_TYPE.ASI_WB_R:
                    this.WhiteBalance_R = value;
                    break;
                case ASI_CONTROL_TYPE.ASI_WB_B:
                    this.WhiteBalance_B = value;
                    break;
                case ASI_CONTROL_TYPE.ASI_BRIGHTNESS:
                    this.Brightness = value;
                    break;
                case ASI_CONTROL_TYPE.ASI_BANDWIDTHOVERLOAD:
                    this.BandwidthOverload = value;
                    break;
                case ASI_CONTROL_TYPE.ASI_OVERCLOCK:
                    this.OverClock = value;
                    break;
                case ASI_CONTROL_TYPE.ASI_TEMPERATURE:
                    this.Temperature = value;
                    break;
                case ASI_CONTROL_TYPE.ASI_FLIP:
                    this.Flip = value;
                    break;
                case ASI_CONTROL_TYPE.ASI_AUTO_MAX_GAIN:
                    this.AutoMaxGain = value;
                    break;
                case ASI_CONTROL_TYPE.ASI_AUTO_MAX_EXP:
                    this.AutoMaxExp = value;
                    break;
                case ASI_CONTROL_TYPE.ASI_AUTO_MAX_BRIGHTNESS:
                    this.AutoMaxBrightness = value;
                    break;
                case ASI_CONTROL_TYPE.ASI_HARDWARE_BIN:
                    this.HardwareBin = value;
                    break;
                case ASI_CONTROL_TYPE.ASI_HIGH_SPEED_MODE:
                    this.HighSpeedMode = value;
                    break;
                case ASI_CONTROL_TYPE.ASI_COOLER_POWER_PERC:
                    this.CoolerPowerPerc = value;
                    break;
                case ASI_CONTROL_TYPE.ASI_TARGET_TEMP:
                    this.TargetTemp = value;
                    break;
                case ASI_CONTROL_TYPE.ASI_COOLER_ON:
                    this.CoolerOn = value;
                    break;
                case ASI_CONTROL_TYPE.ASI_MONO_BIN:
                    this.MonoBin = value;
                    break;
                case ASI_CONTROL_TYPE.ASI_FAN_ON:
                    this.FanOn = value;
                    break;
                case ASI_CONTROL_TYPE.ASI_PATTERN_ADJUST:
                    this.PatternAdjust = value;
                    break;
                case ASI_CONTROL_TYPE.ASI_ANTI_DEW_HEATER:
                    this.AntiDewHeater = value;
                    break;
                case ASI_CONTROL_TYPE.ASI_HUMIDITY:
                    this.Humidity = value;
                    break;
                case ASI_CONTROL_TYPE.ASI_ENABLE_DDR:
                    this.EnableDdr = value;
                    break;
                default:
                    break;
            }
        }
        */
    }
}