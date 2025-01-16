using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASICamera
{
    /// <summary>
    /// ASI相机分辨率
    /// </summary>
    public enum ASICameraResolution
    {
        /// <summary>
        /// 1024*768
        /// </summary>
        HD1024_768,

        /// <summary>
        /// 1280*720
        /// </summary>
        HD720P,

        /// <summary>
        /// 1280*960
        /// </summary>
        HD960P,

        /// <summary>
        /// 1920*1080
        /// </summary>
        HD1080P,

        /// <summary>
        /// 最大尺寸
        /// </summary>
        HDMax
    }

    /// <summary>
    /// ASI相机分辨率
    /// </summary>
    public enum ASICameraFps
    {
        /// <summary>
        /// 5帧
        /// </summary>
        Fps_5,

        /// <summary>
        /// 8帧
        /// </summary>
        Fps_8,

        /// <summary>
        /// 15帧
        /// </summary>
        Fps_15,

        /// <summary>
        /// 24帧
        /// </summary>
        Fps_24,

        /// <summary>
        /// 30帧
        /// </summary>
        Fps_30,

        /// <summary>
        /// 35帧
        /// </summary>
        Fps_35,

        /// <summary>
        /// 40帧
        /// </summary>
        Fps_40
    }

    public enum ASICameraFlip
    {
        /// <summary>
        /// 不翻转
        /// </summary>
        None,

        /// <summary>
        /// 水平翻转
        /// </summary>
        Horizontal,

        /// <summary>
        /// 垂直翻转
        /// </summary>
        Vertical,

        /// <summary>
        /// 水平、垂直翻转
        /// </summary>
        Both
    }
}