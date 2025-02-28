using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetStudio
{
    public class ShadowSettings
    {
        public int m_Type;
        public int m_Resolution;
        public int m_CustomResolution;
        public float m_Strength;
        public float m_Bias;
        public float m_NormalBias;
        public float m_NearPlane;
        public Matrix4x4 m_CullingMatrixOverride;
        public bool m_UseCullingMatrixOverride;

        public ShadowSettings(ObjectReader reader)
        {
            m_Type = reader.ReadInt32();
            m_Resolution = reader.ReadInt32();
            m_CustomResolution = reader.ReadInt32();
            m_Strength = reader.ReadSingle();
            m_Bias = reader.ReadSingle();
            m_NormalBias = reader.ReadSingle();
            m_NearPlane = reader.ReadSingle();
            m_CullingMatrixOverride = reader.ReadMatrix();
            m_UseCullingMatrixOverride = reader.ReadUInt32() > 0;
        }
    }
    public class LightBakingOutput
    {
        public int probeOcclusionLightIndex;
        public int occlusionMaskChannel;
        public LightmapBakeMode lightmapBakeMode;
        public bool isBaked;
        public LightBakingOutput(ObjectReader reader)
        {
            probeOcclusionLightIndex = reader.ReadInt32();
            probeOcclusionLightIndex = reader.ReadInt32();
            lightmapBakeMode = new LightmapBakeMode(reader);
            isBaked = reader.ReadUInt32() > 0;
        }
    }
    public class LightmapBakeMode
    {
        public int lightmapBakeType;
        public int mixedLightingMode;
        public LightmapBakeMode(ObjectReader reader)
        {
            lightmapBakeType = reader.ReadInt32();
            mixedLightingMode = reader.ReadInt32();
        }
    }
    public class Light : Component
    {
        public bool m_Enabled = true;
        public int m_Type;
        public int m_Shape;
        public Color m_Color;
        public float m_Intensity;
        public float m_Range;
        public float m_SpotAngle;
        public float m_InnerSpotAngle;
        public float m_CookieSize;
        public ShadowSettings m_Shadows;
        public PPtr<Texture> m_Cookie;
        public bool m_DrawHalo;
        public LightBakingOutput m_BakingOutput;
        public PPtr<GameObject> m_Flare;
        public int m_RenderMode;
        public uint m_CullingMask_Bits;
        public int m_RenderingLayerMask;
        public int m_Lightmapping;
        public int m_LightShadowCasterMode;
        public Vector2 m_AreaSize;
        public float m_BounceIntensity;
        public float m_ColorTemperature;
        public bool m_UseColorTemperature;
        public Vector4 m_BoundingSphereOverride;
        public bool m_UseBoundingSphereOverride;
        public bool m_UseViewFrustrumForShadowCasterCall;
        public Light(ObjectReader reader) : base(reader)
        {
            // Tested with Persona 5 X (2020.3.41f1c1)
            m_Enabled = reader.ReadUInt32() > 0;
            m_Type = reader.ReadInt32();
            m_Shape = reader.ReadInt32();
            m_Color = reader.ReadColor4();
            m_Intensity = reader.ReadSingle();
            m_Range = reader.ReadSingle();
            m_SpotAngle = reader.ReadSingle();
            m_InnerSpotAngle = reader.ReadSingle();
            m_CookieSize = reader.ReadSingle();
            m_Shadows = new ShadowSettings(reader);
            m_Cookie = new PPtr<Texture>(reader);
            m_DrawHalo = reader.ReadUInt32() > 0;
            m_BakingOutput = new LightBakingOutput(reader);
            m_Flare = new PPtr<GameObject>(reader);
            m_RenderMode = reader.ReadInt32();
            m_CullingMask_Bits = reader.ReadUInt32();
            m_RenderingLayerMask = reader.ReadInt32();
            m_Lightmapping = reader.ReadInt32();
            m_LightShadowCasterMode = reader.ReadInt32();
            m_AreaSize = reader.ReadVector2();
            m_BounceIntensity = reader.ReadSingle();
            m_ColorTemperature = reader.ReadSingle();
            m_UseColorTemperature = reader.ReadUInt32() > 0;
            m_BoundingSphereOverride = reader.ReadVector4();
            m_UseBoundingSphereOverride = reader.ReadBoolean();
            m_UseViewFrustrumForShadowCasterCall = reader.ReadBoolean();
        }
    }
}
