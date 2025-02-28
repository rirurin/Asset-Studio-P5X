using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetStudio
{
    public enum CameraClearFlags
    {
        Skybox = 1,
        Color = 2
    }
    public class Camera : Component
    {
        // FbxCamera->Position in Transform->LocalPosition
        // FbxCamera->UpVector in Transform
        // FbxCamera->InterestPosition in Transform
        // FbxCamera->Roll in Transform->LocalRotation
        public bool m_Enabled = true;
        public CameraClearFlags m_ClearFlags;
        public Color m_BackgroundColor; // FbxCamera->BackgroundColor
        public int m_projectionMatrixMode;
        public int m_GateFitMode; // FbxCamera->GateFit
        public Vector2 m_SensorSize;
        public Vector2 m_LensShift;
        public float m_FocalLength; // FbxCamera->FocalLength
        public Rectf m_NormalizedViewPortRec;
        public float m_nearClipPlane; // FbxCamera->NearPlane
        public float m_farClipPlane; // FbxCamera->FarPlane
        public float m_fov; // FbxCamera->FieldOfView
        public bool m_orthographic; // true = FbxCamera->ProjectionType = eOrthogonal, else ePerspective
        public float m_orthographicSize; // FbxCamera->OrthoZoom
        public float m_depth;
        public uint m_CullingMask;
        public int m_RenderingPath;
        public PPtr<GameObject> m_TargetTexture;
        public int m_TargetDisplay;
        public int m_TargetEye;
        public bool m_HDR;
        public bool m_AllowMSAA;
        public bool m_AllowDynamicResolution;
        public bool m_ForceIntoRT;
        public bool m_OcclusionCulling;
        public float m_StereoConvergence;
        public float m_StereoSeparation;
        public Camera(ObjectReader reader) : base(reader)
        {
            // Tested with Persona 5 X (2020.3.41f1c1)
            m_Enabled = reader.ReadUInt32() > 0;
            m_ClearFlags = (CameraClearFlags)reader.ReadUInt32();
            m_BackgroundColor = reader.ReadColor4();
            m_projectionMatrixMode = reader.ReadInt32();
            m_GateFitMode = reader.ReadInt32();
            m_SensorSize = reader.ReadVector2();
            m_LensShift = reader.ReadVector2();
            m_FocalLength = reader.ReadSingle();
            m_NormalizedViewPortRec = new Rectf(reader);
            m_nearClipPlane = reader.ReadSingle();
            m_farClipPlane = reader.ReadSingle();
            m_fov = reader.ReadSingle();
            m_orthographic = reader.ReadUInt32() > 0;
            m_orthographicSize = reader.ReadSingle();
            m_depth = reader.ReadSingle();
            m_CullingMask = reader.ReadUInt32();
            m_RenderingPath = reader.ReadInt32();
            m_TargetTexture = new PPtr<GameObject>(reader);
            m_TargetDisplay = reader.ReadInt32();
            m_TargetEye = reader.ReadInt32();
            m_HDR = reader.ReadBoolean();
            m_AllowMSAA = reader.ReadBoolean();
            m_AllowDynamicResolution = reader.ReadBoolean();
            m_ForceIntoRT = reader.ReadBoolean();
            m_OcclusionCulling = reader.ReadBoolean();
            reader.AlignStream();
            m_StereoConvergence = reader.ReadSingle();
            m_StereoSeparation = reader.ReadSingle();
        }
    }
}
