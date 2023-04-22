using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Unity.VectorGraphics
{
    public class RuntimeSVGImporter
    {
        /// <summary>The number of pixels per Unity units.</summary>
        public float SvgPixelsPerUnit
        {
            get { return m_SvgPixelsPerUnit; }
            set { m_SvgPixelsPerUnit = value; }
        }

        private float m_SvgPixelsPerUnit = 100.0f;

        /// <summary>Maximum resolution for gradient texture.</summary>
        public UInt16 GradientResolution
        {
            get { return m_GradientResolution; }
            set { m_GradientResolution = value; }
        }

        private UInt16 m_GradientResolution = 64;

        /// <summary>The SVG sprite alignement.</summary>
        public VectorUtils.Alignment Alignment
        {
            get { return m_Alignment; }
            set { m_Alignment = value; }
        }

        private VectorUtils.Alignment m_Alignment;

        /// <summary>The custom pivot, when alignement is "Custom".</summary>
        public Vector2 CustomPivot
        {
            get { return m_CustomPivot; }
            set { m_CustomPivot = value; }
        }

        private Vector2 m_CustomPivot;

        /// <summary>Automaticallly generates a physics shape.</summary>
        public bool GeneratePhysicsShape
        {
            get { return m_GeneratePhysicsShape; }
            set { m_GeneratePhysicsShape = value; }
        }

        private bool m_GeneratePhysicsShape;

        /// <summary>Viewport options to use when importing the SVG document.</summary>
        public ViewportOptions ViewportOptions
        {
            get { return m_ViewportOptions; }
            set { m_ViewportOptions = value; }
        }

        private ViewportOptions m_ViewportOptions = ViewportOptions.DontPreserve;

        /// <summary>Preserves the viewport defined in the SVG document.</summary>
        [Obsolete("Use the ViewportOptions property instead")]
        public bool PreserveViewport
        {
            get { return m_PreserveViewport; }
            set { m_PreserveViewport = value; }
        }

        private bool m_PreserveViewport;

        /// <summary>Use advanced settings.</summary>
        public bool AdvancedMode
        {
            get { return m_AdvancedMode; }
            set { m_AdvancedMode = value; }
        }

        private bool m_AdvancedMode;

        /// <summary>The predefined resolution used, when not in advanced mode.</summary>
        public int PredefinedResolutionIndex
        {
            get { return m_PredefinedResolutionIndex; }
            set { m_PredefinedResolutionIndex = value; }
        }

        private int m_PredefinedResolutionIndex = 1;

        /// <summary>The target resolution on which this SVG is displayed.</summary>
        public int TargetResolution
        {
            get { return m_TargetResolution; }
            set { m_TargetResolution = value; }
        }

        private int m_TargetResolution = 1080;

        /// <summary>An additional scale factor on the target resolution.</summary>
        public float ResolutionMultiplier
        {
            get { return m_ResolutionMultiplier; }
            set { m_ResolutionMultiplier = value; }
        }

        private float m_ResolutionMultiplier = 1.0f;

        /// <summary>The uniform step distance used for tessellation.</summary>
        public float StepDistance
        {
            get { return m_StepDistance; }
            set { m_StepDistance = value; }
        }

        private float m_StepDistance = 10.0f;

        /// <summary>Number of samples evaluated on paths.</summary>
        public float SamplingStepDistance
        {
            get { return m_SamplingStepDistance; }
            set { m_SamplingStepDistance = value; }
        }

        private float m_SamplingStepDistance = 100.0f;

        /// <summary>Enables the "max coord deviation" constraint.</summary>
        public bool MaxCordDeviationEnabled
        {
            get { return m_MaxCordDeviationEnabled; }
            set { m_MaxCordDeviationEnabled = value; }
        }

        private bool m_MaxCordDeviationEnabled;

        /// <summary>Distance on the cord to a straight line between two points after which more tessellation will be generated.</summary>
        public float MaxCordDeviation
        {
            get { return m_MaxCordDeviation; }
            set { m_MaxCordDeviation = value; }
        }

        private float m_MaxCordDeviation = 1.0f;

        /// <summary>Enables the "max tangent angle" constraint.</summary>
        public bool MaxTangentAngleEnabled
        {
            get { return m_MaxTangentAngleEnabled; }
            set { m_MaxTangentAngleEnabled = value; }
        }

        private bool m_MaxTangentAngleEnabled;

        /// <summary>Max tangent angle (in degrees) after which more tessellation will be generated.</summary>
        public float MaxTangentAngle
        {
            get { return m_MaxTangentAngle; }
            set { m_MaxTangentAngle = value; }
        }

        private float m_MaxTangentAngle = 5.0f;

        /// <summary>The size of the texture (only used when importing to a texture).</summary>
        public bool KeepTextureAspectRatio
        {
            get { return m_KeepTextureAspectRatio; }
            set { m_KeepTextureAspectRatio = value; }
        }

        private bool m_KeepTextureAspectRatio = true;

        /// <summary>The size of the texture (only used when importing to a texture with "keep aspect ratio").</summary>
        public int TextureSize
        {
            get { return m_TextureSize; }
            set { m_TextureSize = value; }
        }

        private int m_TextureSize = 1024;

        /// <summary>The width of the texture (only used when importing to a texture).</summary>
        public int TextureWidth
        {
            get { return m_TextureWidth; }
            set { m_TextureWidth = value; }
        }

        private int m_TextureWidth = 1024;

        /// <summary>The height of the texture (only used when importing to a texture).</summary>
        public int TextureHeight
        {
            get { return m_TextureHeight; }
            set { m_TextureHeight = value; }
        }

        private int m_TextureHeight = 256;

        /// <summary>The wrap mode of the texture (only used when importing to a texture).</summary>
        public TextureWrapMode WrapMode
        {
            get { return m_WrapMode; }
            set { m_WrapMode = value; }
        }

        private TextureWrapMode m_WrapMode = TextureWrapMode.Repeat;

        /// <summary>The filter mode of the texture (only used when importing to a texture).</summary>
        public FilterMode FilterMode
        {
            get { return m_FilterMode; }
            set { m_FilterMode = value; }
        }

        private FilterMode m_FilterMode = FilterMode.Bilinear;

        /// <summary>The number of samples per pixel (only used when importing to a texture).</summary>
        public int SampleCount
        {
            get { return m_SampleCount; }
            set { m_SampleCount = value; }
        }

        private int m_SampleCount = 4;

        /// <summary>When importing to an SVGImage, preserves the aspect ratio of the generated sprite.</summary>
        public bool PreserveSVGImageAspect
        {
            get { return m_PreserveSVGImageAspect; }
            set { m_PreserveSVGImageAspect = value; }
        }

        private bool m_PreserveSVGImageAspect;

        /// <summary></summary>
        public bool UseSVGPixelsPerUnit
        {
            get { return m_UseSVGPixelsPerUnit; }
            set { m_UseSVGPixelsPerUnit = value; }
        }

        private bool m_UseSVGPixelsPerUnit;

        private List<VectorUtils.Geometry> _Init(string path, out Rect rect)
        {
            UpdateProperties();
            // We're using a hardcoded window size of 100x100. This way, using a pixels per point value of 100
            // results in a sprite of size 1 when the SVG file has a viewbox specified.
            SVGParser.SceneInfo sceneInfo;
            using (var stream = new StreamReader(path))
                sceneInfo = SVGParser.ImportSVG(stream, ViewportOptions, 0, 1, 100, 100);

            if (sceneInfo.Scene == null || sceneInfo.Scene.Root == null)
                throw new Exception("Wowzers!");

            float stepDist = StepDistance;
            float samplingStepDist = SamplingStepDistance;
            float maxCord = MaxCordDeviationEnabled ? MaxCordDeviation : float.MaxValue;
            float maxTangent = MaxTangentAngleEnabled ? MaxTangentAngle : Mathf.PI * 0.5f;

            if (!AdvancedMode)
            {
                // Automatically compute sensible tessellation options from the
                // vector scene's bouding box and target resolution
                ComputeTessellationOptions(sceneInfo, TargetResolution, ResolutionMultiplier, out stepDist, out maxCord,
                    out maxTangent);
            }

            var tessOptions = new VectorUtils.TessellationOptions();
            tessOptions.MaxCordDeviation = maxCord;
            tessOptions.MaxTanAngleDeviation = maxTangent;
            tessOptions.SamplingStepSize = 1.0f / samplingStepDist;
            tessOptions.StepDistance = stepDist;

            rect = Rect.zero;
            if (ViewportOptions == ViewportOptions.PreserveViewport)
                rect = sceneInfo.SceneViewport;
            return VectorUtils.TessellateScene(sceneInfo.Scene, tessOptions, sceneInfo.NodeOpacity);
        }


        public Mesh ImportAsMesh(string assetPath)
        {
            var mesh = new Mesh();
            var geometry = _Init(assetPath, out Rect rect);
            VectorUtils.FillMesh(mesh, geometry, 1.0f);
            return mesh;
        }

        public Sprite ImportAsVectorSprite(string assetPath)
        {
            var name = Path.GetFileNameWithoutExtension(assetPath);
            var geometry = _Init(assetPath, out Rect rect);
            var sprite = BuildSpriteFromGeometry(geometry, rect);
            return GenerateSprite(sprite, name);
        }

        public Texture2D ImportAsTexture(string assetPath)
        {
            var name = Path.GetFileNameWithoutExtension(assetPath);
            var geometry = _Init(assetPath, out Rect rect);
            return GeometryToTexture(geometry, rect, name);
        }

        public Texture2D GeometryToTexture(List<VectorUtils.Geometry> geometry, Rect rect, string name)
        {
            var sprite = BuildSpriteFromGeometry(geometry, rect);
            return GenerateTexture2D(sprite, name);
        }

        private Sprite BuildSpriteFromGeometry(List<VectorUtils.Geometry> geometry, Rect rect)
        {
            return VectorUtils.BuildSprite(geometry, rect, SvgPixelsPerUnit, Alignment, CustomPivot, GradientResolution,
                true);
        }

        private void UpdateProperties()
        {
            // The "PreserveViewport" property is deprecated and should be moved to the "ViewportOptions" property
            if (m_PreserveViewport)
            {
                m_ViewportOptions = ViewportOptions.PreserveViewport;
                m_PreserveViewport = false;
            }
        }

        private void PrepareSpriteAsset(Sprite sprite, string name)
        {
            sprite.name = name + "Sprite";
            if (sprite.texture != null)
                sprite.texture.name = name + "Atlas";
            sprite.hideFlags = HideFlags.None;
        }

        private Sprite GenerateSprite(Sprite sprite, string name)
        {
            PrepareSpriteAsset(sprite, name);
            return sprite;
        }


        private Texture2D GenerateTexture2D(Sprite sprite, string name)
        {
            var tex = BuildTexture(sprite, name);

            if (sprite.texture != null)
                Object.DestroyImmediate(sprite.texture);
            Object.DestroyImmediate(sprite);
            return tex;
        }

        private Texture2D BuildTexture(Sprite sprite, string textureName)
        {
            int textureWidth;
            int textureHeight;
            if (KeepTextureAspectRatio)
            {
                ComputeTextureDimensionsFromBounds(sprite, TextureSize, out textureWidth, out textureHeight);
            }
            else
            {
                textureWidth = TextureWidth;
                textureHeight = TextureHeight;
            }

            Material mat = MaterialForSVGSprite(sprite);

            // Expand edges to avoid bilinear filter "edge outlines" caused by transparent black background.
            // Not necessary when using point filtering with 1 sample.
            bool expandEdges = FilterMode != FilterMode.Point || SampleCount > 1;

            var tex = VectorUtils.RenderSpriteToTexture2D(sprite, textureWidth, textureHeight, mat, SampleCount,
                expandEdges);
            tex.hideFlags = HideFlags.None;
            tex.name = textureName;
            tex.wrapMode = WrapMode;
            tex.filterMode = FilterMode;

            return tex;
        }

        public static Material MaterialForSVG(bool hasTexture)
        {
            const string k_PackagePath = "Packages/com.unity.vectorgraphics";
            string path;
            if (hasTexture)
                // When texture is present, use the VectorGradient shader
                path = k_PackagePath + "/Runtime/Materials/Unlit_VectorGradient";
            else
                path = k_PackagePath + "/Runtime/Materials/Unlit_Vector";

            path += ".mat";

            return AssetDatabase.LoadAssetAtPath<Material>(path);
        }

        private Material MaterialForSVGSprite(Sprite sprite)
        {
            return MaterialForSVG(sprite.texture != null);
        }

        private void ComputeTextureDimensionsFromBounds(Sprite sprite, int textureSize, out int textureWidth,
            out int textureHeight)
        {
            var bounds = sprite.bounds;
            if (bounds.size.y < Mathf.Epsilon)
            {
                textureWidth = textureSize;
                textureHeight = textureSize;
                return;
            }

            float ratio = bounds.size.x / bounds.size.y;
            if (ratio >= 1.0f)
            {
                textureWidth = TextureSize;
                textureHeight = Mathf.RoundToInt(TextureSize / ratio);
            }
            else
            {
                textureWidth = Mathf.RoundToInt(TextureSize * ratio);
                textureHeight = TextureSize;
            }
        }

        private void ComputeTessellationOptions(SVGParser.SceneInfo sceneInfo, int targetResolution, float multiplier,
            out float stepDist, out float maxCord, out float maxTangent)
        {
            // These tessellation options were found by trial and error to find values that made
            // visual sense with a variety of SVG assets.

            // "Pixels per Unit" doesn't make sense for UI Toolkit since it will be displayed in
            // a pixels space.  We adjust the magic values below accordingly.
            float ppu = SvgPixelsPerUnit;
            var bbox = VectorUtils.ApproximateSceneNodeBounds(sceneInfo.Scene.Root);
            float maxDim = Mathf.Max(bbox.width, bbox.height) / ppu;

            // The scene ratio gives a rough estimate of coverage % of the vector scene on the screen.
            // Higher values should result in a more dense tessellation.
            float sceneRatio = maxDim / (targetResolution * multiplier);

            stepDist = float.MaxValue; // No need for uniform step distance
            maxCord = Mathf.Max(0.01f, 75.0f * sceneRatio);
            maxTangent = Mathf.Max(0.1f, 100.0f * sceneRatio);
        }

        internal static Sprite GetImportedSprite(string assetPath)
        {
            return AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
        }

        internal static Sprite GetImportedSprite(Object asset)
        {

            var sprite = asset as Sprite;
            if (sprite != null)
                return sprite;

            var go = asset as GameObject;

            // Try with SpriteRenderer
            var sr = go != null ? go.GetComponent<SpriteRenderer>() : null;
            sprite = sr != null ? sr.sprite : null;
            if (sprite != null)
                return sprite;

            // Try with VectorImage
            var si = go != null ? go.GetComponent<SVGImage>() : null;
            sprite = si != null ? si.sprite : null;
            if (sprite != null)
                return sprite;

            return null;
        }

        internal static Texture2D GetImportedTexture2D(string assetPath)
        {
            return AssetDatabase.LoadMainAssetAtPath(assetPath) as Texture2D;
        }

        internal void TextureSizeForSpriteEditor(Sprite sprite, out int width, out int height)
        {
            var size = ((Vector2)sprite.bounds.size) * SvgPixelsPerUnit;
            width = (int)(size.x + 0.5f);
            height = (int)(size.y + 0.5f);
        }

        private static Material s_VectorMat;
        private static Material s_GradientMat;

        internal static Material CreateSVGSpriteMaterial(Sprite sprite)
        {
            if (sprite == null)
                return null;
            return CreateSVGSpriteMaterial(sprite.texture != null);
        }

        private static Material CreateSVGSpriteMaterial(bool hasTexture)
        {
            Material mat;
            if (hasTexture)
            {
                if (s_GradientMat == null)
                {
                    string gradientMatPath =
                        "Packages/com.unity.vectorgraphics/Runtime/Materials/Unlit_VectorGradient.mat";
                    s_GradientMat = AssetDatabase.LoadMainAssetAtPath(gradientMatPath) as Material;
                }

                mat = new Material(s_GradientMat);
            }
            else
            {
                if (s_VectorMat == null)
                {
                    string vectorMatPath = "Packages/com.unity.vectorgraphics/Runtime/Materials/Unlit_Vector.mat";
                    s_VectorMat = AssetDatabase.LoadMainAssetAtPath(vectorMatPath) as Material;
                }

                mat = new Material(s_VectorMat);
            }

            return mat;
        }
    }
}
