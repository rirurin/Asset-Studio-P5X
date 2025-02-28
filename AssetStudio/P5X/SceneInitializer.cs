using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetStudio
{
    // From PWRD.TerrainX.dll
    // Used for Persona 5 X
    // Sets a bunch of environment related params - basically ENVs in P5R
    public sealed class SceneInitializer
    {
        // Fields
        public bool volumeFogOverWrite;
        public bool volumeFogEnableOverWrite;
        private static bool isFirstInit;
        private static SceneInitializer.ExpandBloomQuality m_ExpandBloomQuality;
        private static SceneInitializer _instance;
        public Material NPCDepthMaterial;
        // There is (to my knowledge) nothing functionally similar to PostProcessVolumes in P5R:
        // The environmental settings for a field remain constant throughout it, and there are no
        // Flowscript commands to change ENV parameters at a location to achieve the same effect
        //public PostProcessVolume[] sceneStaticPPVs;
        //private PostProcessVolume currentPPV;
        public Camera sceneMainCamera;
        public int CameraForwardRendererID;
        public float _fakeSpecForBakedObjThreshold = -0.995f;
        public float _fakeSpecForBakedObjIntensity = 1f;
        public float _fakeSpecForBakedObjColorLevel = 1f;
        public float _HighlightIntensity = 1f;
        public float _IBLInShadow = 1f;
        public float _SunDirXOffset; // P5R_ENV->FieldModel.LightPosition
        public float _SunDirYOffset;
        public float _SunDirZOffset;
        public float _DiffuseNLFix = 1f;
        public Vector4 _SceneColorAdjustParam = new Vector4(50f, 100f, 1f, 1f);
        public float _WindIntensity = 0.1f; // P5R_ENV->Physics.WindIntensity
        public float _WindFrequency = 1f; // P5R_ENV->Physics.WindDuration
        public Texture2D _WindNoiseTexture;
        public float _WindNoiseTextureTiling = 1f;
        public float _DebugSaturationValue = 0.5f;
        public Color _RoleIndirectDiffEnd = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        public float _RoleIndirectDiffRange = 1f;
        public Color _RoleIndirectSpecEnd = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        public float _RoleIndirectSpecRange = 1f;
        public Color _RoleDirectDiffEnd = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        public float _RoleDirectDiffRange = 1f;
        public Color _RoleDirectSpecEnd = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        public float _RoleDirectSpecRange = 1f;
        public Transform _RoleDirectSpecFix;
        public float _RoleDirectViewFix;
        public float _RoleRecieveShadowStrength = 1f;
        public bool _RoleRelationWithDOT = true;
        public float _RoleOutlineWidthGlobalRate = 1f;
        public Color _RoleRimLightAdjustColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        public float _RoleRimLightAdjustScale = 1f;
        public float _RoleDistanceRimMask = 1f;
        public float _SHIntensity = 1f;
        public float _CBT2Saturation;
        public float _CBT2Gamma = 1f;
        public float _RoleGISatLevel = 0.75f;
        public Color _DCColorTint = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        public float _DCScale = 1f;
        public bool _RoleRelationWithSun = true;
        public float _RoleSunSatLevel = 0.35f;
        public Color _RoleSunlightColorTint = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        public float _RoleSunlightScale = 1f;
        public float _SceneBloomIntensity = 1f;
        public float _RoleBloomIntensity = 1f;
        public float _RoleRimLightBloomIntensity = 1f;
        public float _ParticleBloomIntensity = 1f;
        public float _SkyBloomIntensity = 1f;
        public float _RoleAOFix = -1f;
        public float _BlueLightAlphaFix_Global = 1f;
        public float _DirLightmapIntensity = 1f;
        public bool _AdvScreenLitOnOff;
        public float _AdvScreenGIBlend = 0.5f;
        public float _AdvScreenGIIntensity = 1f;
        public float _AdvScreenEmissionIntensity = 1f;
        public Vector4 _PannerLightPos = new Vector4(0f, 0f, 0f, 0f);
        public Color _PannerShadowColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        public float _PannerShadowFalloff = 0.2f;
        public float _PannerHeight;
        public int _FilmFlashON;
        public Color _FilmFlashTint = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        public float _FilmFlashSpeed = 0.5f;
        public float _FilmFlashTemp = 1f;
        public float _FilmRole = 0.5f;
        //private static Dictionary<PostProcessEffectSettings, bool> aoaCachedEfxSettings;
        //private UniversalRenderPipelineAsset urpPipAsset;
        public bool m_VolumetricFogSceneEnable;
        public float m_FogDiffuse = 0.75f;
        public float m_FogDensity = 0.75f;
        public float m_FogDensityInShadow;
        public float m_GodRayThreshold = 0.99f;
        public float m_GodRayDensity = 1f;
        public float m_DepthThreshold;
        public float m_RayMatchingRange = 200f;
        public int m_Step_MAX = 64;
        public Color m_FogColor = new Color(1.0f, 1.0f, 1.0f, 1.0f); // P5R_ENV->Fog.FogColor
        public float m_FogTimming = 1f;
        //public Texture3D m_FogNoise3D;
        public Vector4 m_Noise3DTilling = new Vector4(0.05f, 0.05f, 0.05f, 1f);
        public Vector4 m_NosieSpeed = new Vector4(0f, 0f, 0.05f, 1f);
        public float m_NosieMipOffset = 0.5f;
        public Vector4 m_GroundOffset = new Vector4(1f, 0f, 1f, 1f);
        public float m_FogPointLightPow = 1f;
        public float m_FogRTScale = 0.5f;
        //public BlendMode m_FogSrcBlend = BlendMode.One;
        //public BlendMode m_FogDstBlend = BlendMode.One;
        // Volumetrics lmao
        public bool m_VolumetricLightPassEnable;
        public bool m_VolumetricLightShadowEnable;
        public bool m_VolumetricLightBlurEnable;
        public bool m_VolumetricLightTimeEnable;
        public int m_VolumetricLightMaxStep = 128;
        public bool m_VolumetricLightUseYCocg = true;
        public bool m_VolumetricLightUseClipping = true;
        public bool m_VolumetricLightUseOptimizations = true;
        public bool m_VolumetricLightUseVarianceClip = true;
        public float m_VolumetricLightVarianceCoe = 1f;
        public float m_VolumetricLightFeedbackMin = 0.88f;
        public float m_VolumetricLightFeedbackMax = 0.97f;
        public bool m_VolumetricLightNoiseEnable;
        public List<Texture2D> m_VolumetricLightNoises;
        private Vector3 _SunDirOffset = Vector3.Zero;
        public static bool isSimulateMobile;
        private SceneInitializer lastInstance;
        public enum ExpandBloomQuality
        {
            high,
            low
        }
    }
}
