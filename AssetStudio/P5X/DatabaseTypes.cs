using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AssetStudio.P5X
{
    public interface IDBTableMarker { }

    public class DatabaseLocation
    {
        public string TableName;
        public string DatabaseName;
        public DatabaseLocation(XmlNode itemNode)
        {
            // <Item ExcelName="TABLE_NAME", DBName="ConfDataDB_NAME" />
            TableName = "Conf" + itemNode.Attributes.GetNamedItem("ExcelName").Value;
            DatabaseName =          itemNode.Attributes.GetNamedItem("DBName").Value;
        }
        public override string ToString() => $"Table Name: {TableName}, DB Name: {DatabaseName}";
    }

    namespace ConfDataBasic
    {
        public class ThirdEyeRender
        {
            public string TimeOfDay;
            public int Value;
            public ThirdEyeRender(string timeOfDay, int value)
            {
                TimeOfDay = timeOfDay;
                Value = value;
            }
            public static ThirdEyeRender[]? CreateThirdEyeRenderArray(string[]? stringData)
            {
                if (stringData == null) return null;
                ThirdEyeRender[] listOut = new ThirdEyeRender[stringData.Length];
                for (int i = 0; i < stringData.Length; i++)
                {
                    var stringComp = stringData[i].Split("|");
                    var timeOfDay = stringComp[0];
                    if (stringComp.Length == 1) timeOfDay = "All";
                    var value = int.Parse(stringComp[stringComp.Length - 1]);
                    listOut[i] = new ThirdEyeRender(timeOfDay, value);
                }
                return listOut;
            }
        }
        public class ConfAINPC : IDBTableMarker
        {
            public long sn;
            public long monsterLv;
            public double patrolSpeed;
            public double chaseSpeed;
            public long patrolType;
            public Vector3[]? patrol;
            public Vector2? restTime;
            public Vector3? sightR;
            public Vector2? feelingR;
            public long alertTime;
            public long chaseR;
            public long battleSn;
            public long alertLevelCondition;
            public Vector2? alertThreshold;
            public bool canView;
            public long patrolWay;
            public double wander;
            public long type;
            public string param1;
            public string param2;
            public long directKillProbability;
            public long star;
            public string[]? soundTag;
            public bool detialShow;
            public bool alwaysShow;
            public bool colliderDetection;

            public ConfAINPC(object[] db)
            {
                sn = (long)db[0];
                monsterLv = (long)db[1];
                patrolSpeed = (double)db[2];
                chaseSpeed = (double)db[3];
                patrolType = (long)db[4];
                patrol = Utility.CreateVector3List((string)db[5]);
                restTime = Utility.GetVector2((string)db[6]);
                sightR = Utility.GetVector3((string)db[7]);
                feelingR = Utility.GetVector2((string)db[8]);
                alertTime = (long)db[9];
                chaseR = (long)db[10];
                battleSn = (long)db[11];
                alertLevelCondition = (long)db[12];
                alertThreshold = Utility.GetVector2((string)db[13]);
                canView = Utility.GetBool((long)db[14]);
                patrolWay = (long)db[15];
                wander = (double)db[16];
                type = (long)db[17];
                param1 = (string)db[18];
                param2 = (string)db[19];

                directKillProbability = (long)db[20];
                star = (long)db[21];
                soundTag = Utility.GetStringList((string)db[22]);
                detialShow = Utility.GetBool((long)db[23]);
                alwaysShow = Utility.GetBool((long)db[24]);
                colliderDetection = Utility.GetBool((long)db[25]);
            }
        }
        public class ConfArea : IDBTableMarker
        {
            public long sn;
            public bool useInRollMap;
            public long scene;
            public long areaType;
            public Vector3? pos;
            public string param;
            public bool showEffect;
            public long objectTraceSn;
            public ConfArea(object[] db)
            {
                sn = (long)db[0];
                useInRollMap = Utility.GetBool((long)db[1]);
                scene = (long)db[2];
                areaType = (long)db[3];
                pos = Utility.GetVector3((string)db[4]);
                param = Utility.FromBase64((string)db[5]);
                showEffect = Utility.GetBool((long)db[6]);
                objectTraceSn = (long)db[7];
            }
        }
        public class ConfAreaLogic : IDBTableMarker
        {
            public long sn;
            public long areaSn;
            public bool triggerReset;
            public long[]? triggerCnt;
            public long triggerActIn;
            public string[]? triggerParamIn;
            public long triggerActLeave;
            public string[]? triggerParamLeave;
            public bool canTeleportTriggerLeave;
            public string jumpLink;
            public long jumpAngle;
            public long forceRotateAngle;
            public long appearType;
            public long[]? appearParam;
            public ConfAreaLogic(object[] db)
            {
                sn = (long)db[0];
                areaSn = (long)db[1];
                triggerReset = Utility.GetBool((long)db[2]);
                triggerCnt = Utility.Get1DArray<long>((string)db[3]);
                triggerActIn = (long)db[4];
                triggerParamIn = Utility.GetStringList((string)db[5]);
                triggerActLeave = (long)db[6];
                triggerParamLeave = Utility.GetStringList((string)db[7]);
                canTeleportTriggerLeave = Utility.GetBool((long)db[8]);
                jumpLink = (string)db[9];
                jumpAngle = (long)db[10];
                forceRotateAngle = (long)db[11];
                appearType = (long)db[12];
                appearParam = Utility.Get1DArray<long>((string)db[13]);
            }
        }
        public class ConfNpc : IDBTableMarker
        {
            public long sn;
            public string name;
            public string desc;
            public long npcType;
            public long Open;
            public long[]? appearType;
            public long[]? appearParam;
            public bool alwaysMapShow;
            public long unlockLevel;
            public string unlockfuncOpens;
            public string OWTimeVisible;
            public long multiPos;
            public string randomPart;
            public long useInRollMap;
            public long stageSn;
            public Vector3? birthPoint;
            public double faceToAngle;
            public string IKtargetPoint;
            public double IKTargetDistance;
            public long teleportSn;
            public string modelSn;
            public string fashionSn;
            public string additiveFashionParts;
            public bool mapShow;
            public string mapIcon;
            public string listIcon;
            public long mapPress;
            public long mapBlockSn;
            public bool initUnlock;
            public double iconScale;
            public Vector3? positionOnMap;
            public long[]? InitializeState;
            public long[]? switchIdle;
            public long thirdEye;
            public long thridEyeSn;
            public string chat;
            public long chatGroup;
            public long[]? funcId;
            public bool Turn;
            public bool blockHostTurn;
            public float[]? distances;
            public string interactiveButton;
            public string interactiveAction;
            public bool interactiveEffect;
            public bool camera;
            public bool stagger;
            public bool alwaysShow;
            public long visDistance;
            public string Emoticon;
            public string strName;
            public string strIcon;
            public string clientAiTree;
            public string commonEffect;
            public string thridEyeEffect;
            public long exploreSceneType;
            public long exploreExp;
            public string audioEffect;
            public string unlockStr;
            public long objectTraceSn;
            public bool isMapGuideIcon;
            public ConfNpc(object[] db)
            {
                sn = (long)db[0];
                name = (string)db[1];
                desc = (string)db[2];
                npcType = (long)db[3];
                Open = (long)db[4];
                appearType = Utility.Get1DArray<long>((string)db[5]);
                appearParam = Utility.Get1DArray<long>((string)db[6]);
                alwaysMapShow = Utility.GetBool((long)db[7]);
                unlockLevel = (long)db[8];
                unlockfuncOpens = (string)db[9];
                OWTimeVisible = (string)db[10];
                multiPos = (long)db[11];
                randomPart = (string)db[12];
                useInRollMap = (long)db[13];
                stageSn = (long)db[14];
                birthPoint = Utility.GetVector3((string)db[15]);
                faceToAngle = (double)db[16];
                IKtargetPoint = (string)db[17];
                IKTargetDistance = (double)db[18];
                teleportSn = (long)db[19];
                modelSn = (string)db[20];
                fashionSn = (string)db[21];
                additiveFashionParts = (string)db[22];
                mapShow = Utility.GetBool((long)db[23]);
                mapIcon = (string)db[24];
                listIcon = (string)db[25];
                mapPress = (long)db[26];
                mapBlockSn = (long)db[27];
                initUnlock = Utility.GetBool((long)db[28]);
                iconScale = (double)db[29];
                positionOnMap = Utility.GetVector3((string)db[30]);
                InitializeState = Utility.Get1DArray<long>((string)db[31]);
                switchIdle = Utility.Get1DArray<long>((string)db[32]);
                thirdEye = (long)db[33];
                thridEyeSn = (long)db[34];
                chat = (string)db[35];
                chatGroup = (long)db[36];
                funcId = Utility.Get1DArray<long>((string)db[37]);
                Turn = Utility.GetBool((long)db[38]);
                blockHostTurn = Utility.GetBool((long)db[39]);
                distances = Utility.Get1DArray<float>((string)db[40]);
                interactiveButton = (string)db[41];
                interactiveAction = (string)db[42];
                interactiveEffect = Utility.GetBool((long)db[43]);
                camera = Utility.GetBool((long)db[44]);
                stagger = Utility.GetBool((long)db[45]);
                alwaysShow = Utility.GetBool((long)db[46]);
                visDistance = (long)db[47];
                Emoticon = (string)db[48];
                strName = (string)db[49];
                strIcon = (string)db[50];
                clientAiTree = (string)db[51];
                commonEffect = (string)db[52];
                thridEyeEffect = (string)db[53];
                exploreSceneType = (long)db[54];
                exploreExp = (long)db[55];
                audioEffect = (string)db[56];
                unlockStr = (string)db[57];
                objectTraceSn = (long)db[58];
                isMapGuideIcon = Utility.GetBool((long)db[59]);
            }
        }
        public class ConfScene : IDBTableMarker
        {
            public long sn;
            public string name;
            public string desc;
            public long scene_type;
            public long loadingType;
            public long dependScene;
            public long worldMapSn;
            public long dependNpc;
            public long palaceAreaSn;
            public string asset;
            public string sceneName;
            public string sceneIconName;
            public string mapOffset;
            public long unlockType;
            public long unlockParam;
            public bool THIEF_UIBOOL;
            public long prefabRollMapSn;
            public long randomRollMapSn;
            public long aStarDataSn;
            public string navMesh;
            public bool canMove;
            public long standtype;
            public bool kaleidoscope;
            public string[]? cameraRes;
            public string[]? cameraResPhone;
            public double width;
            public double height;
            public long cell_size;
            public long scene_model;
            public long[]? scene_fashion;
            public long reviveConfSn;
            public Vector3? enter_point;
            public double enter_point_direction;
            public Vector2? enter_CamXYAxis;
            public Vector2? enter_CamDefaultParam;
            public Vector3? enter_point2;
            public long humanMaxNum;
            public string music;
            public string Amsound;
            public string BossBattleBGM;
            public long[]? obstacle_sn;
            public long linkScene;
            public long relatedActivity;
            public long creatParam;
            public long sceneWeather;
            public bool canUseUmbrella;
            public string dayTime;
            public ThirdEyeRender[]? thirdEyeRender;
            public bool shield_server;
            public bool footEffect;
            public string[]? miniMapSn;
            public long[]? limitPhaseType;
            public long[]? limitWeatherType;
            public long[]? limitDayType;
            public long limitTip;
            public Vector3? dependPos;
            public Vector3? dependOutPos;
            public string bossIcon;
            public double fogRange;
            public long distortionSn;
            public bool teamDisplay;
            public bool BIevent;
            public ConfScene(object[] db)
            {
                sn = (long)db[0];
                name = (string)db[1];
                desc = (string)db[2];
                scene_type = (long)db[3];
                loadingType = (long)db[4];
                dependScene = (long)db[5];
                worldMapSn = (long)db[6];
                dependNpc = (long)db[7];
                palaceAreaSn = (long)db[8];
                asset = (string)db[9];
                sceneName = (string)db[10];
                sceneIconName = (string)db[11];
                mapOffset = (string)db[12];
                unlockType = (long)db[13];
                unlockParam = (long)db[14];
                THIEF_UIBOOL = Utility.GetBool((long)db[15]);
                prefabRollMapSn = (long)db[16];
                randomRollMapSn = (long)db[17];
                aStarDataSn = (long)db[18];
                navMesh = (string)db[19];
                canMove = Utility.GetBool((long)db[20]);
                standtype = (long)db[21];
                kaleidoscope = Utility.GetBool((long)db[22]);
                cameraRes = Utility.Get1DArray<string>((string)db[23]);
                cameraResPhone = Utility.Get1DArray<string>((string)db[24]);
                width = (double)db[25];
                height = (double)db[26];
                cell_size = (long)db[27];
                scene_model = (long)db[28];
                scene_fashion = Utility.Get1DArray<long>((string)db[29]);
                reviveConfSn = (long)db[30];
                enter_point = Utility.GetVector3((string)db[31]);
                enter_point_direction = (double)db[32];
                enter_CamXYAxis = Utility.GetVector2((string)db[33]);
                enter_CamDefaultParam = Utility.GetVector2((string)db[34]);
                enter_point2 = Utility.GetVector3((string)db[35]);
                humanMaxNum = (long)db[36];
                music = (string)db[37];
                Amsound = (string)db[38];
                BossBattleBGM = (string)db[39];
                obstacle_sn = Utility.Get1DArray<long>((string)db[40]);
                linkScene = (long)db[41];
                relatedActivity = (long)db[42];
                creatParam = (long)db[43];
                sceneWeather = (long)db[44];
                canUseUmbrella = Utility.GetBool((long)db[45]);
                dayTime = (string)db[46];
                thirdEyeRender = ThirdEyeRender.CreateThirdEyeRenderArray(Utility.Get1DArray<string>((string)db[47]));
                shield_server = Utility.GetBool((long)db[48]);
                footEffect = Utility.GetBool((long)db[49]);
                miniMapSn = Utility.Get1DArray<string>((string)db[50]);
                limitPhaseType = Utility.Get1DArray<long>((string)db[51]);
                limitWeatherType = Utility.Get1DArray<long>((string)db[52]);
                limitDayType = Utility.Get1DArray<long>((string)db[53]);
                limitTip = (long)db[54];
                dependPos = Utility.GetVector3((string)db[55]);
                dependOutPos = Utility.GetVector3((string)db[56]);
                bossIcon = (string)db[57];
                fogRange = (double)db[58];
                distortionSn = (long)db[59];
                teamDisplay = Utility.GetBool((long)db[60]);
                BIevent = Utility.GetBool((long)db[61]);
            }
            public override string ToString() => $"ID: {sn}, Name: {name}";
            public string[] AsStrings(int start)
            {
                var fields = GetType().GetFields();
                string[] stringsOut = new string[fields.Length - start];
                for (int i = start; i < fields.Length; i++)
                    stringsOut[i - start] = (fields[i].GetValue(this) != null) ? fields[i].GetValue(this).ToString() : "";
                return stringsOut;
                
            }
        }

        public class ConfSoundKeyName : IDBTableMarker
        {
            public string sn;
            public string oldCueName;
            public string cueName;
            public long soundType;
            public string aisacName;

            public ConfSoundKeyName(object[] db)
            {
                sn = (string)db[0];
                oldCueName = (string)db[1];
                cueName = (string)db[2];
                soundType = (long)db[3];
                aisacName = (string)db[4];
            }
        }
    }
    namespace ConfDataText
    {
        public class ConfDialog : IDBTableMarker
        {
            public string sn;
            public string[]? nexts;
            public string alternative;
            public long dialogType;
            public string speaker;
            public string showName;
            public string content;
            public string Head2dSn;
            public double delay;
            public string audio;
            public bool isFullAudio;
            public string[]? anims;
            public string[]? headEmotions;
            public string funcs;
            public ConfDialog(object[] db)
            {
                sn = (string)db[0];
                nexts = Utility.Get1DArray<string>((string)db[1]);
                alternative = (string)db[2];
                dialogType = (long)db[3];
                speaker = (string)db[4];
                showName = (string)db[5];
                content = (string)db[6];
                Head2dSn = (string)db[7];
                delay = (double)db[8];
                audio = (string)db[9];
                isFullAudio = Utility.GetBool((long)db[10]);
                anims = Utility.Get1DArray<string>((string)db[11]);
                headEmotions = Utility.Get1DArray<string>((string)db[12]);
                funcs = (string)db[13];

            }
        }
    }
}
