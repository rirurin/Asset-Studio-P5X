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
    public sealed class MUActorMeshExportInfo
    {
        // LOD specific fields
        private string mHighRootBoneName;
        private string mLodRootBoneName;
        private string[] mHighMeshBoneNames;
        private string[] mLodMeshBoneNames;
        private Material[] mHighMeshMaterials;
        private Material[] mLodMaterials;
        private long[] mHighMeshMaterialIDs;
        private long[] mLodMaterialIDs;
        private Mesh mHighMesh;
        private Mesh mLodMesh;
        private long mHighMeshID;
        private long mLodMeshID;
        // Flags
        public bool mIsSkinnedMeshRender { get; }
        public bool mIsLOD { get; }
        // Public facing fields
        public Mesh mMesh {
            get => mIsLOD ? mLodMesh : mHighMesh;
            set
            {
                if (mIsLOD) mLodMesh = value;
                else mHighMesh = value;
            }
        }
        public long mMeshID { get => mIsLOD ? mLodMeshID : mHighMeshID; }
        public string mRootBoneName { get => mIsLOD ? mLodRootBoneName : mHighRootBoneName; }
        public string[] mBoneNames { get => mIsLOD ? mLodMeshBoneNames : mHighMeshBoneNames; }
        public Material[] mMaterials { 
            get => mIsLOD ? mLodMaterials : mHighMeshMaterials; 
            set
            {
                if (mIsLOD) mLodMaterials = value;
                else mHighMeshMaterials = value;
            }
        }
        public long[] mMaterialIDs { get => mIsLOD ? mLodMaterialIDs : mHighMeshMaterialIDs; }
        /*
        private string[] makeBoneList(List<object> boneListObj)
        {
            string[] arr = new string[boneListObj.Count];
            for (int i = 0; i < boneListObj.Count; i++)
            {
                arr[i] = boneListObj[i].ToString();
            }
            return arr;
        }
        private long[] makeMaterialList(List<object> matListObj)
        {
            long[] arr = new long[matListObj.Count];
            for (int i = 0; i < matListObj.Count; i++)
            {
                arr[i] = setObjectPathId((OrderedDictionary)matListObj[i]);
            }
            return arr;
        }
        */
        private T[] makeArray<T>(object objListA, Func<object, T> insertFunc)
        {
            List<object> objList = (List<object>)objListA;
            T[] arr = new T[objList.Count];
            for (int i = 0; i < objList.Count; i++)
            {
                arr[i] = insertFunc(objList[i]);
            }
            return arr;
        }
        private long setObjectPathId(OrderedDictionary dictInt)
        {
            long val = 0;
            foreach (DictionaryEntry dictEntry in dictInt)
            {
                switch ((string)dictEntry.Key)
                {
                    case "m_PathID":
                        val = (long)dictEntry.Value;
                        break;
                }
            }
            return val;
        }
        public MUActorMeshExportInfo(MonoBehaviour behavior, bool isLOD)
        {
            OrderedDictionary bDict = behavior.ToType();
            mIsLOD = isLOD;
            foreach (DictionaryEntry dictEntry in bDict)
            {
                switch ((string)dictEntry.Key)
                {
                    case "mHighRootBoneName":
                        mHighRootBoneName = (string)dictEntry.Value;
                        break;
                    case "mLodRootBoneName":
                        mLodRootBoneName = (string)dictEntry.Value;
                        break;
                    case "mHighMeshBoneNames":
                        mHighMeshBoneNames = makeArray(dictEntry.Value, x => x.ToString());
                        break;
                    case "mLodMeshBoneNames":
                        mLodMeshBoneNames = makeArray(dictEntry.Value, x => x.ToString());
                        break;
                    case "mHighMeshMaterials":
                        mHighMeshMaterialIDs = makeArray(dictEntry.Value, x => setObjectPathId((OrderedDictionary)x));
                        break;
                    case "mLodMaterials":
                        mLodMaterialIDs = makeArray(dictEntry.Value, x => setObjectPathId((OrderedDictionary)x));
                        break;
                    case "mMesh":
                        mHighMeshID = setObjectPathId((OrderedDictionary)dictEntry.Value);
                        break;
                    case "mLODMesh":
                        mLodMeshID = setObjectPathId((OrderedDictionary)dictEntry.Value);
                        break;
                    case "mIsSkinnedMeshRender":
                        if ((byte)dictEntry.Value == 0)
                        {
                            mIsSkinnedMeshRender = false;
                        } else
                        {
                            mIsSkinnedMeshRender = true;
                        }
                        break;
                }
            }
        }
        /*
        public void BuildSkinnedMeshRender(Transform parent, SkinnedMeshRenderer render)
        {
            if (!mIsSkinnedMeshRender) return;
            //render.m_Mesh = new PPtr<Mesh>(getMesh());
            //render.m_RootBone = new PPtr<Transform>(getRootBoneName());
            FillBones(render, parent);
        }

        private void FillBones(SkinnedMeshRenderer render, Transform parent)
        {
            foreach (string boneName in getMeshBoneNames())
            {

            }
        }
        private Transform FindRootBone(Transform parent, string boneName)
        {

        }
        */
    }
}
