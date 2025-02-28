using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetStudio
{
    // From PWRD.MUEngine.dll
    // Used for Persona 5 X
    public sealed class MUMultiSceneObjects : ICustomMonoBehavior
    {
        // Fields
        //public List<Terrain> Terrains;
        public List<MonoBehaviour> DayTimeMgrs { get; } = new List<MonoBehaviour>();
        public List<Camera> Cameras { get; } = new List<Camera>();
        public List<MonoBehaviour> Volumes { get; } = new List<MonoBehaviour>();
        public List<Light> lights { get; } = new List<Light>();
        public List<MonoBehaviour> SceneInitializers { get; } = new List<MonoBehaviour>();
        //public List<LightProbeGroup> LightProbeGroups;
        //public List<ReflectionProbe> ReflectionProbes;
        public List<Renderer> Renders { get; } = new List<Renderer>();
        // public List<Occluder> Occulders;
        public GameObject mTerrainObj { get; set; }
        // Field IDs List
        public List<long> DayTimeMgrIDs { get; private set; }
        public List<long> CameraIDs { get; private set; }
        public List<long> VolumeIDs { get; private set; }
        public List<long> LightIDs { get; private set; }
        public List<long> SceneInitializerIDs { get; private set; }
        public List<long> RenderIDs { get; private set; }
        public long mTerrainObjID { get; private set; }


        public MUMultiSceneObjects(MonoBehaviour behavior)
        {
            ICustomMonoBehavior.FillFields(behavior, (fldKey, fldVal) =>
            {
                switch (fldKey)
                {
                    case "mTerrainObj":
                        mTerrainObjID = ICustomMonoBehavior.GetObjectPathID(fldVal);
                        break;
                    case "DayTimeMgrs":
                        DayTimeMgrIDs = ICustomMonoBehavior.MakeList(fldVal, x => ICustomMonoBehavior.GetObjectPathID(x));
                        break;
                    case "Cameras":
                        CameraIDs = ICustomMonoBehavior.MakeList(fldVal, x => ICustomMonoBehavior.GetObjectPathID(x));
                        break;
                    case "Volumes":
                        VolumeIDs = ICustomMonoBehavior.MakeList(fldVal, x => ICustomMonoBehavior.GetObjectPathID(x));
                        break;
                    case "Lights":
                        LightIDs = ICustomMonoBehavior.MakeList(fldVal, x => ICustomMonoBehavior.GetObjectPathID(x));
                        break;
                    case "SceneInitializers":
                        SceneInitializerIDs = ICustomMonoBehavior.MakeList(fldVal, x => ICustomMonoBehavior.GetObjectPathID(x));
                        break;
                    case "Renders":
                        RenderIDs = ICustomMonoBehavior.MakeList(fldVal, x => ICustomMonoBehavior.GetObjectPathID(x));
                        break;
                }
            });
        }
    }
}
