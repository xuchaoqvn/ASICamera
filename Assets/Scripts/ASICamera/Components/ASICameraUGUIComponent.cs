using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ASICamera
{
    /// <summary>
    /// ASICameraUI显示组件
    /// </summary>
    public class ASICameraUGUIComponent : MaskableGraphic
    {
        /// <summary>
        /// 默认纹理
        /// </summary>
        private enum DefaultTexture
        {
            /// <summary>
            /// 黑色纹理（0.0,0.0,0.0,0.0）
            /// </summary>
            Black,

            /// <summary>
            /// 灰色纹理（0.5,0.5,0.5,0.5）,sRGB颜色空间
            /// </summary>
            Gray,

            /// <summary>
            /// 灰色纹理（0.5,0.5,0.5,0.5）,sRGB线性空间
            /// </summary>
            LinearGray,

            /// <summary>
            /// 法线纹理（0.5,0.5,1.0,1.0）
            /// </summary>
            Normal,

            /// <summary>
            /// 红色纹理（1.0,0.0,0.0,0.0）
            /// </summary>
            Red,

            /// <summary>
            /// 白色纹理（1.0,1.0,1.0,1.0）
            /// </summary>
            White,

            /// <summary>
            /// 自定义
            /// </summary>
            Custom
        }

        #region Field
        /// <summary>
        /// ASI相机
        /// </summary>
        [SerializeField]
        private ASICamera m_ASICamera;

        /// <summary>
        /// 默认纹理
        /// </summary>
        [SerializeField]
        private DefaultTexture m_DefaultTexture;

        /// <summary>
        /// 原始大小
        /// </summary>
        [SerializeField]
        private bool m_NativeSize = false;

        /// <summary>
        /// UV显示区域
        /// </summary>
        [SerializeField]
        private Rect m_UVRect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);

        /// <summary>
        /// 自定义默认纹理
        /// </summary>
        [SerializeField]
        private Texture m_CustomDefaultTexture;

        /// <summary>
        /// 上次纹理显示款度
        /// </summary>
        private int m_LastWidth;

        /// <summary>
        /// 上次纹理显示高度
        /// </summary>
        private int m_LastHeight;
        #endregion

        #region Property
        /// <summary>
        /// 获取主纹理
        /// </summary>
        /// <value>主纹理</value>
        public override Texture mainTexture
        {
            get
            {
                if (this.m_ASICamera != null && this.m_ASICamera.OutputTexture != null)
                    return this.m_ASICamera.OutputTexture;

                Texture defaultTexture = Texture2D.whiteTexture;
                switch (this.m_DefaultTexture)
                {
                    case DefaultTexture.Black:
                        defaultTexture = Texture2D.blackTexture;
                        break;
                    case DefaultTexture.Gray:
                        defaultTexture = Texture2D.grayTexture;
                        break;
                    case DefaultTexture.LinearGray:
                        defaultTexture = Texture2D.linearGrayTexture;
                        break;
                    case DefaultTexture.Normal:
                        defaultTexture = Texture2D.normalTexture;
                        break;
                    case DefaultTexture.Red:
                        defaultTexture = Texture2D.redTexture;
                        break;
                    case DefaultTexture.White:
                        defaultTexture = Texture2D.whiteTexture;
                        break;
                    case DefaultTexture.Custom:
                        {
                            if (this.m_CustomDefaultTexture != null)
                                defaultTexture = this.m_CustomDefaultTexture;
                        }
                        break;
                    default:
                        break;
                }
                return defaultTexture;
            }
        }

        /// <summary>
        /// 获取或设置ASI相机
        /// </summary>
        /// <value>ASI相机</value>
        public ASICamera ASICamera
        {
            get => this.m_ASICamera;
            set
            {
                if (this.m_ASICamera == value)
                    return;

                this.m_ASICamera = value;
                this.SetMaterialDirty();
                if (this.m_NativeSize)
                    this.SetNativeSize();
            }
        }

        /// <summary>
        /// 设置或获取是否设置原始大小
        /// </summary>
        /// <value>是否设置原始大小</value>
        public bool NativeSize
        {
            get => this.m_NativeSize;
            set => this.m_NativeSize = value;
        }

        /// <summary>
        /// 纹理使用的UV矩形
        /// </summary>
        public Rect UVRect
        {
            get => this.m_UVRect;
            set
            {
                if (this.m_UVRect == value)
                    return;

                this.m_UVRect = value;
                this.SetVerticesDirty();
            }
        }
        #endregion

        private void Update()
        {
            if (this.m_NativeSize)
                this.SetNativeSize();

            if (Input.GetKeyDown(KeyCode.Q))
            {
                this.m_ASICamera.Pause();
                //Debug.Log("暂停。");
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                this.m_ASICamera.Resume();
                Debug.Log("继续。");
            }
        }

        #region Function
        /// <summary>
        /// 设置原始大小
        /// </summary>
        [ContextMenu("Set Native Size")]
        public override void SetNativeSize()
        {
            Texture mainTexture = this.mainTexture;
            if (mainTexture != null)
            {
                int width = Mathf.RoundToInt(mainTexture.width * this.m_UVRect.width);
                int height = Mathf.RoundToInt(mainTexture.height * this.m_UVRect.height);
                this.rectTransform.anchorMax = this.rectTransform.anchorMin;
                this.rectTransform.sizeDelta = new Vector2(width, height);
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            Texture tex = mainTexture;
            vh.Clear();
            if (tex != null)
            {
                var r = GetPixelAdjustedRect();
                var v = new Vector4(r.x, r.y, r.x + r.width, r.y + r.height);
                var scaleX = tex.width * tex.texelSize.x;
                var scaleY = tex.height * tex.texelSize.y;
                {
                    var color32 = color;
                    vh.AddVert(new Vector3(v.x, v.y), color32, new Vector2(m_UVRect.xMin * scaleX, m_UVRect.yMin * scaleY));
                    vh.AddVert(new Vector3(v.x, v.w), color32, new Vector2(m_UVRect.xMin * scaleX, m_UVRect.yMax * scaleY));
                    vh.AddVert(new Vector3(v.z, v.w), color32, new Vector2(m_UVRect.xMax * scaleX, m_UVRect.yMax * scaleY));
                    vh.AddVert(new Vector3(v.z, v.y), color32, new Vector2(m_UVRect.xMax * scaleX, m_UVRect.yMin * scaleY));

                    vh.AddTriangle(0, 1, 2);
                    vh.AddTriangle(2, 3, 0);
                }
            }
        }
        #endregion
    }
}