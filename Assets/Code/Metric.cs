using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using static Metric.Archive.SaveFile.MapRecord.SideRecord;
using static Metric.Camera;

namespace Metric
{

    public static class PlayerPhysical
    {

        public static float scale = 5;

        public static class Ground
        {

            //在平地上只按住方向键移动的加速度
            public static float NormalRunAcceleration = 50 / 3.0f * scale;
            //平地上移动的最大水平速度
            public static float MaxHorizentalSpeed = 90 / 1.0f * scale;
            //在平地上不按水平方向键时的减速度
            public static float NormalStopAcceleration = 50 / 3.0f * scale;
            //当水平移动速度超过最大速度时同向移动的减速度
            public static float SameDirectionLimitAcceleration = 20 / 3.0f * scale;
            //当水平移动速度超过最大速度时异向移动的减速度
            public static float DiffDirectionLimitAcceleration = 50 / 3.0f * scale;
            //在平地上蹲下时的水平减速度
            public static float SquatAcceleration = 25 / 3.0f * scale;

        }

        public static class Jump
        {

            //平地跳跃的起跳速度
            public static float NormalJumpSpeed = 200 / 1.0f * scale;
            //平地跳跃的反向加速度
            public static float NormalJumpAcceleration = 50 / 1.0f * scale;
            //墙跳产生的水平和垂直速度
            public static Vector2 JumpWithWallVeiocity = new Vector2(150 * scale, 200 * scale);

        }

        public static class Stagnant
        {

            //在空中时按住水平方向键时的水平加速度
            public static float NormalHorizentalAcceleration = 65 / 6.0f * scale;
            //在空中时不按住水平方向键时的水平减速度
            public static float StopHorizentalAcceleration = 65 / 6.0f * scale;
            //在空中时的最大水平速度
            public static float MaxHorizentalSpeed = 90 / 1.0f * scale;
            //当水平速度超过最大速度时同向移动的减速度
            public static float SameDirectionLimitAcceleration = 13 / 12.0f * scale;
            //当水平速度超过最大速度时异向移动的减速度
            public static float DiffDirectionLimitAcceleration = 65 / 12.0f * scale;
            //在空中时不按住下键的最大下落速度
            public static float MaxDropSpeedWithOutDown = 160 / 1.0f * scale;
            //在空中时按住下键的最大下落速度
            public static float MaxDropSpeedWithDown = 240 / 1.0f * scale;
            //在空时正常状态下的重力加速度
            public static float NormalDropAcceleration = 15 / 1.5f * scale;
            //在空时不正常状态下的重力加速度(不正常状态有 : 当 Madeline 的速度在 [-40,40] 的区间内时, 如果 Madeline是冲刺结束的状态或者按住跳键)
            public static float SpecialDropAcceleration = 15 / 4.0f * scale;
            //在空中的特殊掉落状态的左边界
            public static float SpecialDropRegionLeft = -60 / 1.0f * scale;
            //在空中的特殊掉落状态的右边界
            public static float SpecialDropRegionRight = 60 / 1.0f * scale;
            //在空中时当下落速度超过最大下落速度时产生的反向加速度(不按住下键时最大速度为160,按住为240,但他们的限制反向加速度都为这个)
            public static float VerticalSpeedLimitAcceleration = 15 / 1.0f * scale;
            //在空中时当下落速度超过正常最大下落速度(160)时同时按住下键将继续向下加速的加速度
            public static float VerticalSpeedOutOfNormalMaxSpeedAccelerationWithDown = 5 / 1.0f * scale;

        }

        public static class Catch
        {

            public static float CatchScale = 2f;

            //最大体力值
            public static float MaxStamina = 110 / 1.0f;
            //抓住静止不动时的体力消耗速度
            public static float StopSpeed = 1 / 6.0f;
            //抓住往下爬时的体力消耗速度
            public static float ClimpDownSpeed = 0 / 1.0f;
            //抓住往上爬时的体力消耗速度
            public static float ClimpUpSpeed = 4 / 5.0f;
            //抓墙跳消耗的体力值
            public static float ClimbJumpCost = 55 / 2.0f;
            //抓住往上爬的最大速度
            public static float MaxClimbUpSpeed = 45 / 2.0f * scale;
            //抓住往下爬的最大速度
            public static float MaxClimbDownSpeed = 80 / 2.0f * scale;
            //抓住往上爬的加速度
            public static float ClimbUpAcceleration = 15 / 2.0f * scale * CatchScale;
            //抓住往下爬的加速度
            public static float ClimbDownAcceleration = 15 / 2.0f * scale * CatchScale;
            //抓住往上爬的速度超过最大速度后的反向加速度
            public static float ClimbUpLimitAcceleration = 15 / 2.0f * scale * CatchScale;
            //抓住往下爬的速度超过最大速度后的反向加速度
            public static float ClimbDownLimitAcceleration = 15 / 2.0f * scale * CatchScale;
            //抓住爬行停止的反向加速度
            public static float ClimbStopAcceleration = 45 / 2.0f * scale * CatchScale;

        }

        public static class Dash
        {

            //最大冲刺次数
            public static int MaxDashNum = 1;
            //冲刺开始时的暂停帧数
            public static int DashStartStopFrameNum = 4;
            //冲刺过程保持状态不变的帧数
            public static int DashContinueFramNum = DashStartStopFrameNum + 10;
            //冲刺过后下一次冲刺的最快的帧数
            public static int NextDashFrameNum = DashContinueFramNum + 5;
            //斜下冲刺撞到地面时的水平速度放大倍数
            public static float DashBumpGroundHorizentalSpeedScale = 10 / 5.0f;
            //冲刺初始(合)速度
            public static float DashSpeed = 240 * scale;
            //冲刺结束速度
            public static float DashEndSpeed = 240 * scale;
            //计算二维冲刺速度 abgle 以12点方向为0度,顺时针为正方向
            public static Vector2 CalculateDashSpeed(Vector2 originalSpeed, float angle)
            {
                Vector2 t = new Vector2(
                    DashSpeed * Mathf.Cos(angle / 360 * 2 * Mathf.PI),
                    DashSpeed * -Mathf.Sin(angle / 360 * 2 * Mathf.PI)
                );
                if (originalSpeed.x * t.x > 0 && Mathf.Abs(originalSpeed.x) > Mathf.Abs(t.x))
                {
                    t.x = originalSpeed.x;
                }
                return t;
            }
            //计算冲刺结束后的速度
            public static Vector2 CalculateDashEndSpeed(Vector2 speed, float angle)
            {
                return new Vector2(DashEndSpeed * Mathf.Cos(angle / 360 * 2 * Mathf.PI), speed.y / 2);
            }

        }

    }

    public static class JumpBoardPhysical
    {

        public static Vector2 Speed = new Vector2(0, 1400f);

    }

    public static class MenuControl
    {

        public static KeyCode MenuControlButton = KeyCode.Escape;
        public static KeyCode ConfirmButton = KeyCode.C;
        public static KeyCode CancelButton = KeyCode.X;
        public static KeyCode UpButton = KeyCode.UpArrow;
        public static KeyCode DownButton = KeyCode.DownArrow;
        public static KeyCode LeftButton = KeyCode.LeftArrow;
        public static KeyCode RightButton = KeyCode.RightArrow;

    }

    public static class FadeBlock
    {

        public static float ShakeStrength = 0.2f;

        public static float ShakeTime = 1f;

        public static float FadeTime = 0.2f;

        public static float RefreshTime = 3f;

        public static float AppearTime = 0.5f;

    }

    public static class DashCrystal
    {

        public static float RefreshTime = 2f;

    }

    public static class DropBlock
    {

        //掉落加速度
        public static float DropAccelerate = 100f;
        //最大掉落速度
        public static float MaxDropVelocity = 100f;
        //从玩家触发掉落到开始掉落前的抖动时间
        public static float ShakeTime = 0.5f;
        //抖动的强度
        public static float ShakeStrength = 3f;

    }

    public static class Strawberry
    {

        /// <summary>
        /// 连续吃草莓的间隔
        /// </summary>
        public static float EatStrawberryInterval = 0.8f;

    }

    public static class FlyStrawberry
    {

        public static float FlyDelayTime = 0.5f;

        public static float FlyAccelarate = 1000f;

        public static float FlyMaxSpeed = 1000f;

    }

    public static class BlackScreenTransition
    {

        public static float BarLerpDelay = 0.08f;

        public static float HideTime = 0.6f;

        public static float ShowTime = 0.6f;

    }

    public static class MoveBlock
    {

        public static float PlayerSpeedEffectScale = 300.0f;

        public static float ShakeStrength = 0.1f;

        public static float ShakeTime = 0.1f;

    }

    public static class FallingBlock
    {

        public static float ShakeStrength = 10;

        public static float ShakeTime = 0.5f;

    }

    public static class Tools
    {

        public static void Exit()
        {

#if UNITY_EDITOR

            UnityEditor.EditorApplication.isPlaying = false;

#else

                        Application.Quit();

#endif

            Settings.Save();

            Archive.Save();

        }

        public static void PlayAnimation(Animation anime, string animeName, float speed = 1)
        {

            anime[animeName].speed = speed;
            anime[animeName].time = speed > 0 ? 0 : anime[animeName].length;
            anime.Play(animeName);

        }

        public static float Damp(float a, float b, float dt, float lambda = 0.5f)
        {
            return Mathf.Lerp(a, b, 1 - Mathf.Exp(lambda * dt));
        }

    }

    public static class Settings
    {

#if UNITY_EDITOR || UNITY_STANDALONE_WIN

        public static string SettingsPath = $"{Application.dataPath}";

#elif UNITY_ANDROID

#elif UNITY_IOS

#endif

        public static string SettingsFile = "setting.celeste";

        public static string SettingsVersion = "1.0.1";

        public static void Save()
        {

            BinaryWriter bw;

            if (!Directory.Exists(SettingsPath))
            {

                Directory.CreateDirectory(SettingsPath);

            }

            FileStream fs = new FileStream($"{SettingsPath}/{SettingsFile}", FileMode.OpenOrCreate);

            bw = new BinaryWriter(fs);

            bw.Write(SettingsVersion);

            Audio.Save(bw);

            KeyBoard.Save(bw);

            bw.Close();

            fs.Close();

        }

        public static void Read()
        {

            if (!Directory.Exists(SettingsPath))
            {

                Directory.CreateDirectory(SettingsPath);

            }

            if (!File.Exists($"{SettingsPath}/{SettingsFile}"))
            {

                Save();

                return;

            }

            BinaryReader br;

            FileStream fs = new FileStream($"{SettingsPath}/{SettingsFile}", FileMode.Open);

            br = new BinaryReader(fs);

            string fileVersion = br.ReadString();

            switch (fileVersion)
            {

                case "1.0.0":
                    {

                        Audio.Read(br);

                        break;
                    }
                case "1.0.1":
                    {

                        Audio.Read(br);

                        KeyBoard.Read(br);

                        break;
                    }

            }

            br.Close();

            fs.Close();

        }

        public static class Audio
        {

            public static int MasterStrength = 6;

            public static int MusicStrength = 6;

            public static int EffectStrength = 6;

            public static int EnvironmentStrength = 10;

            public static void Save(BinaryWriter writer)
            {

                writer.Write(MasterStrength);
                writer.Write(MusicStrength);
                writer.Write(EffectStrength);
                writer.Write(EnvironmentStrength);

            }

            public static void Read(BinaryReader reader)
            {

                MasterStrength = reader.ReadInt32();
                MusicStrength = reader.ReadInt32();
                EffectStrength = reader.ReadInt32();
                EnvironmentStrength = reader.ReadInt32();

            }

        }

        public static class KeyBoard
        {

            public enum KeyKind
            {

                Game_Left,
                Game_Right,
                Game_Up,
                Game_Down,
                Game_Jump,
                Game_Dash,
                Game_Catch,

                Menu_Left,
                Menu_Right,
                Menu_Up,
                Menu_Down,
                Menu_Confirm,
                Menu_Cancel,
                Menu_Log

            }

            public static KeyCode[] mapping = new KeyCode[(int)KeyKind.Menu_Log + 1]
            {

                KeyCode.LeftArrow,
                KeyCode.RightArrow,
                KeyCode.UpArrow,
                KeyCode.DownArrow,
                KeyCode.C,
                KeyCode.X,
                KeyCode.Z,

                KeyCode.LeftArrow,
                KeyCode.RightArrow,
                KeyCode.UpArrow,
                KeyCode.DownArrow,
                KeyCode.C,
                KeyCode.X,
                KeyCode.Tab

            };

            public static void Save(BinaryWriter writer)
            {

                writer.Write(mapping.Length);

                for(int i = 0; i < mapping.Length; ++i)
                {

                    writer.Write((int)mapping[i]);

                }

            }

            public static void Read(BinaryReader reader)
            {

                mapping = new KeyCode[reader.ReadInt32()];

                for(int i = 0; i < mapping.Length; ++i)
                {

                    mapping[i] = (KeyCode)reader.ReadInt32();

                }

            }

            public static KeyCode Game_Left => mapping[0];
            public static KeyCode Game_Right => mapping[1];
            public static KeyCode Game_Up => mapping[2];
            public static KeyCode Game_Down => mapping[3];
            public static KeyCode Game_Jump => mapping[4];
            public static KeyCode Game_Dash => mapping[5];
            public static KeyCode Game_Catch => mapping[6];

            public static KeyCode Menu_Left => mapping[7];
            public static KeyCode Menu_Right => mapping[8];
            public static KeyCode Menu_Up => mapping[9];
            public static KeyCode Menu_Down => mapping[10];
            public static KeyCode Menu_Confirm => mapping[12];
            public static KeyCode Menu_Cancel => mapping[12];
            public static KeyCode Menu_Log => mapping[13];

        }

    }

    public static class Archive
    {

#if UNITY_EDITOR || UNITY_STANDALONE_WIN

        public static string SavePath = $"{Application.dataPath}/Save";

#elif UNITY_ANDROID

#elif UNITY_IOS

#endif

        public static int SaveFileNum = 3;

        public static string SaveFileVersion = "1.0.0";

        public static SaveFile[] saveFiles = new SaveFile[SaveFileNum];

        public class SaveFile
        {

            public string SaveName;

            public MapRecord[] mapRecords;
            /// <summary>
            /// 获得草莓数量
            /// </summary>
            public int GetStrawberryNum()
            {

                int Result = 0;
                if (mapRecords != null)
                {

                    for (int i = 0; i < mapRecords.Length; ++i)
                    {

                        Result += mapRecords[i].GetStrawberryNum();

                    }

                }

                return Result;

            }
            /// <summary>
            /// 获得死亡数量
            /// </summary>
            public int GetDeathNum()
            {

                int Result = 0;

                if (mapRecords != null)
                {

                    for (int i = 0; i < mapRecords.Length; ++i)
                    {

                        Result += mapRecords[i].GetDeathNum();

                    }

                }

                return Result;

            }
            /// <summary>
            /// 获得游玩时间
            /// </summary>
            public TimeSpan GetTimeSpent()
            {

                TimeSpan Result = new TimeSpan(0, 0, 0, 0, 0);

                if (mapRecords != null)
                {

                    for (int i = 0; i < mapRecords.Length; ++i)
                    {

                        Result += mapRecords[i].GetTimeSpent();

                    }

                }

                return Result;

            }

            public void Save(BinaryWriter writer)
            {

                writer.Write(SaveName);

                writer.Write(mapRecords.Length);

                foreach (var map in mapRecords)
                {

                    map.Save(writer);

                }

            }

            public enum ReadState
            {

                None,
                Error,
                Success

            }

            public (ReadState, string) Read(BinaryReader reader)
            {

                try
                {

                    string fileVersion = reader.ReadString();

                    switch (fileVersion)
                    {
                        case "1.0.0":
                            {

                                SaveName = reader.ReadString();

                                int mapRecordNum = reader.ReadInt32();

                                mapRecords = new MapRecord[mapRecordNum];

                                for (int i = 0; i < mapRecordNum; ++i)
                                {

                                    mapRecords[i] = new MapRecord();

                                    mapRecords[i].Read(reader);

                                }

                                break;

                            }
                        default:
                            {

                                return (ReadState.Error,"没有对应的存档版本号");

                            }

                    }

                    return (ReadState.Success,"读取成功");

                }
                catch (Exception e)
                {

                    return (ReadState.Error, e.ToString());

                }

            }

            public class MapRecord
            {

                /// <summary>
                /// 地图下标
                /// </summary>
                public int MapIndex;
                /// <summary>
                /// 关卡记录
                /// </summary>
                public SideRecord[] sideRecords;
                /// <summary>
                /// 获得草莓数量
                /// </summary>
                public int GetStrawberryNum()
                {

                    int Result = 0;

                    if (sideRecords != null)
                    {

                        for (int i = 0; i < sideRecords.Length; ++i)
                        {

                            Result += sideRecords[i].GetStrawberryNum();

                        }

                    }

                    return Result;

                }
                /// <summary>
                /// 获得死亡数量
                /// </summary>
                public int GetDeathNum()
                {

                    int Result = 0;

                    if (sideRecords != null)
                    {

                        for (int i = 0; i < sideRecords.Length; ++i)
                        {

                            Result += sideRecords[i].DeathNum;

                        }

                    }

                    return Result;

                }
                /// <summary>
                /// 获得游玩时间
                /// </summary>
                public TimeSpan GetTimeSpent()
                {

                    TimeSpan Result = new TimeSpan(0, 0, 0, 0, 0);

                    if (sideRecords != null)
                    {

                        for (int i = 0; i < sideRecords.Length; ++i)
                        {

                            Result += sideRecords[i].TimeSpent;

                        }

                    }

                    return Result;

                }
                /// <summary>
                /// 
                /// </summary>
                /// <param name="writer"></param>
                public void Save(BinaryWriter writer)
                {

                    writer.Write(MapIndex);

                    writer.Write(sideRecords.Length);

                    foreach (var side in sideRecords)
                    {

                        side.Save(writer);

                    }

                }
                /// <summary>
                /// 
                /// </summary>
                /// <param name="reader"></param>
                public void Read(BinaryReader reader)
                {

                    MapIndex = reader.ReadInt32();

                    int sideRecordNum = reader.ReadInt32();

                    sideRecords = new SideRecord[sideRecordNum];

                    for (int i = 0; i < sideRecordNum; ++i)
                    {

                        sideRecords[i] = new SideRecord();

                        sideRecords[i].Read(reader);

                    }

                }

                public class SideRecord
                {

                    /// <summary>
                    /// 关卡下标
                    /// </summary>
                    public int SideIndex;
                    /// <summary>
                    /// 死亡次数
                    /// </summary>
                    public int DeathNum;
                    /// <summary>
                    /// 度过的时间
                    /// </summary>
                    public TimeSpan TimeSpent;
                    /// <summary>
                    /// 最短用时
                    /// </summary>
                    public TimeSpan MinimumTime;
                    /// <summary>
                    /// 记录点记录
                    /// </summary>
                    public List<CheckPointRecord> checkPointRecords;
                    /// <summary>
                    /// 获得草莓数量
                    /// </summary>
                    public int GetStrawberryNum()
                    {

                        int Result = 0;

                        if (checkPointRecords != null)
                        {

                            for (int i = 0; i < checkPointRecords.Count; ++i)
                            {

                                Result += checkPointRecords[i].Strawberries.Count;

                            }

                        }

                        return Result;

                    }
                    /// <summary>
                    /// 
                    /// </summary>
                    /// <param name="writer"></param>
                    public void Save(BinaryWriter writer)
                    {

                        writer.Write(SideIndex);

                        writer.Write(DeathNum);

                        writer.Write(TimeSpent);

                        writer.Write(MinimumTime);

                        writer.Write(checkPointRecords.Count);

                        foreach (var checkPoint in checkPointRecords)
                        {

                            checkPoint.Save(writer);

                        }

                    }
                    /// <summary>
                    /// 
                    /// </summary>
                    /// <param name="reader"></param>
                    public void Read(BinaryReader reader)
                    {

                        SideIndex = reader.ReadInt32();

                        DeathNum = reader.ReadInt32();

                        TimeSpent = reader.ReadTimeSpan();

                        MinimumTime = reader.ReadTimeSpan();

                        int checkPointRecordNum = reader.ReadInt32();

                        checkPointRecords = new List<CheckPointRecord>();

                        for (int i = 0; i < checkPointRecordNum; ++i)
                        {

                            checkPointRecords.Add(new CheckPointRecord());

                            checkPointRecords[i].Read(reader);

                        }

                    }

                    public class CheckPointRecord
                    {

                        /// <summary>
                        /// 记录点下标
                        /// </summary>
                        public int CheckPointIndex;
                        /// <summary>
                        /// 获得的草莓下标
                        /// </summary>
                        public List<int> Strawberries;
                        /// <summary>
                        /// 
                        /// </summary>
                        /// <param name="writer"></param>
                        public void Save(BinaryWriter writer)
                        {

                            writer.Write(CheckPointIndex);

                            writer.Write(Strawberries.Count);

                            foreach (int strawberry in Strawberries)
                            {

                                writer.Write(strawberry);

                            }

                        }
                        /// <summary>
                        /// 
                        /// </summary>
                        /// <param name="reader"></param>
                        public void Read(BinaryReader reader)
                        {

                            CheckPointIndex = reader.ReadInt32();

                            int StrawberryNum = reader.ReadInt32();

                            Strawberries = new List<int>(StrawberryNum);

                            for (int i = 0; i < StrawberryNum; ++i)
                            {

                                Strawberries.Add(reader.ReadInt32());

                            }

                        }

                    }

                }

            }

        }

        public static void Save()
        {

            BinaryWriter bw;

            if (!Directory.Exists(SavePath))
            {

                Directory.CreateDirectory(SavePath);

            }

            for (int i = 0; i < SaveFileNum; ++i)
            {

                if (saveFiles[i] == null)
                {

                    if (File.Exists($"{SavePath}/{i + 1}.celeste"))
                    {

                        File.Delete($"{SavePath}/{i + 1}.celeste");

                    }

                    continue;

                }

                FileStream fs = new FileStream($"{SavePath}/{i + 1}.celeste", FileMode.OpenOrCreate);

                bw = new BinaryWriter(fs);

                bw.Write(SaveFileVersion);

                saveFiles[i].Save(bw);

                bw.Close();

                fs.Close();

            }

        }

        public static void Read()
        {

            BinaryReader br;

            for (int i = 0; i < SaveFileNum; ++i)
            {

                if (!File.Exists($"{SavePath}/{i + 1}.celeste"))
                {

                    saveFiles[i] = null;

                    continue;

                }

                FileStream fs = new FileStream($"{SavePath}/{i + 1}.celeste", FileMode.OpenOrCreate);

                br = new BinaryReader(fs);

                saveFiles[i] = new SaveFile();

                var Result = saveFiles[i].Read(br);

                if(Result.Item1 != SaveFile.ReadState.Success)
                {

                    Debug.LogWarning(Result.Item2);

                    saveFiles[i] = null;

                }

                br.Close();

                fs.Close();

            }

        }

    }

    public static class Debug
    {

        public static void Log(object message)
        {

#if UNITY_EDITOR

            UnityEngine.Debug.Log(message);

#endif

        }

        public static void LogWarning(object message)
        {

#if UNITY_EDITOR

            UnityEngine.Debug.LogWarning(message);

#endif

        }

    }

    public static class Camera
    {

        [Serializable]
        public struct CameraTargetState
        {

            public PolarCoordinates polarCoordinates;

            public float height;

            public bool IsRotation;

            public Vector3 EulerAngle;

            public CameraTargetState(Vector3 position, Vector3 eulerAngle, bool isRotation)
            {

                polarCoordinates = position.ToPolarCoordinates();

                height = position.y;

                IsRotation = isRotation;

                EulerAngle = eulerAngle;

            }

            public static CameraTargetState Main_0 = new CameraTargetState(new Vector3(0.000f, 12.000f, 24.000f), new Vector3(7.585f, 165.929f, 0f), false);
            public static CameraTargetState Main_1 = new CameraTargetState(new Vector3(-2.856f, 3.000f, 14.711f), new Vector3(-14.951f, 170.348f, 0f), true);

        }

    }

    public static class Mountain
    {

        public static readonly float[] MainMountainState = new float[3] { 1, 0, 0 };

    }

    public static class MapsInfo
    {

        private static readonly string MapIconsPath = "MainScene/MapIcons/";

        private static readonly string CheckPointsPath = "MainScene/CheckPoints/";

        public static readonly Map[] infos = new Map[]
        {

            new Map
            {
                MapName = "序幕",
                MapIndex = "",
                MainColor = new Color(0.3764706f, 0.3333333f, 0.4196079f),
                SecondColor = new Color(0.3803922f, 0.7490196f, 0.882353f),
                MountainMaterialState = new float[3]{ 1,0,0 },
                MoveOnCameraState = new CameraTargetState(new Vector3( 1.374f, 1.224f, 7.971f),new Vector3( 21.254f,-149.927f,0f),false),
                SelectCameraState = new CameraTargetState(new Vector3( 1.390f, 0.784f, 7.593f),new Vector3(  6.861f,-137.653f,0f),false),
                ReturnCameraState = new CameraTargetState(new Vector3( 1.155f, 0.683f, 7.348f),new Vector3(  3.496f,-153.499f,0f),false),
                IconFace = Resources.Load<Sprite>($"{MapIconsPath}lvl0_0"),
                IconBack = Resources.Load<Sprite>($"{MapIconsPath}lvl0_1"),
                Sides = new Side[]
                {
                    new Side
                    {
                        SceneName = "",
                        SideType = SideType.Plot,
                        CheckPoints = new CheckPoint[]
                        {
                            new CheckPoint
                            {
                                CheckPointIcon = null,
                                CheckPointId = "0",
                                CheckPointName = "",
                                StrawBerriesName = null
                            }
                        }
                    }
                }
            },
            new Map
            {

                MapName = "被遗弃的城市",
                MapIndex = "第 1 章",
                MainColor = new Color(0.5019608f, 0.5529412f, 0.7019608f),
                SecondColor = new Color(0.2078432f,0.2862745f,0.4941177f),
                MountainMaterialState = new float[3]{ 1,0,0 },
                MoveOnCameraState = new CameraTargetState(new Vector3( 0.952f, 4.218f, 9.744f),new Vector3( 24.364f,-152.507f,0f),false),
                SelectCameraState = new CameraTargetState(new Vector3( 0.052f, 1.659f, 9.902f),new Vector3(  3.814f,-144.382f,0f),false),
                ReturnCameraState = new CameraTargetState(new Vector3(-1.387f, 1.884f, 7.215f),new Vector3( 29.141f, 179.967f,0f),false),
                IconFace = Resources.Load<Sprite>($"{MapIconsPath}lvl1_0"),
                IconBack = Resources.Load<Sprite>($"{MapIconsPath}lvl1_1"),
                Sides = new Side[]
                {
                    new Side
                    {
                        SceneName = "Lvl1",
                        SideType = SideType.A,
                        CheckPoints = new CheckPoint[]
                        {
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}1_1"),
                                CheckPointId = "1",
                                CheckPointName = "开始",
                                StrawBerriesName = new string[]
                                {
                                    "strawberry_2_0",
                                    "strawberry_3_0",
                                    "FlyStrawberry_3b_0",
                                    "strawberry_5z_0",
                                    "strawberry_5_0",
                                    "strawberry_5a_0"
                                }
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}1_6"),
                                CheckPointId = "6",
                                CheckPointName = "十字路口",
                                StrawBerriesName = new string[]
                                {
                                    "strawberry_7zb_0",
                                    "strawberry_6_0",
                                    "strawberry_s1_0",
                                    "strawberry_7z_0",
                                    "strawberry_8zb_0",
                                    "strawberry_7z_0",
                                    "strawberry_9z_0",
                                    "strawberry_8b_0",
                                    "strawberry_9_0"
                                }
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}1_9b"),
                                CheckPointId = "9b",
                                CheckPointName = "峡谷",
                                StrawBerriesName = new string[]
                                {
                                    "strawberry_10zb_0",
                                    "strawberry_11_0",
                                    "strawberry_9b_0",
                                    "FlyStrawberry_9c_0",
                                    "strawberry_12z_0"
                                }
                            }
                        }
                    },
                    new Side
                    {
                        SceneName = "",
                        SideType = SideType.B,
                        CheckPoints = new CheckPoint[]
                        {
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}1H_00"),
                                CheckPointId = "00",
                                CheckPointName = "开始",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}1H_04"),
                                CheckPointId = "04",
                                CheckPointName = "机器",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}1H_08"),
                                CheckPointId = "08",
                                CheckPointName = "垃圾场",
                                StrawBerriesName = null
                            }
                        }
                    },
                    new Side
                    {
                        SceneName = "",
                        SideType = SideType.C,
                        CheckPoints = new CheckPoint[]
                        {
                            new CheckPoint
                            {
                                CheckPointIcon = null,
                                CheckPointId = "00",
                                CheckPointName = "",
                                StrawBerriesName = null
                            }
                        }
                    }
                }

            },
            new Map
            {

                MapName = "旧址",
                MapIndex = "第 2 章",
                MainColor = new Color(0.1607843f, 0.6156863f, 0.4039216f),
                SecondColor = new Color(0.9764706f,1.0000000f,0.5803922f),
                MountainMaterialState = new float[3]{ 1,0,0 },
                MoveOnCameraState = new CameraTargetState(new Vector3( 3.399f, 5.614f, 3.870f),new Vector3( 36.080f,-134.198f,0f),false),
                SelectCameraState = new CameraTargetState(new Vector3( 3.890f, 3.903f, 3.702f),new Vector3(  2.522f,-111.956f,0f),false),
                ReturnCameraState = new CameraTargetState(new Vector3( 3.362f, 4.315f, 3.335f),new Vector3( 16.923f, -131.44f,0f),false),
                IconFace = Resources.Load<Sprite>($"{MapIconsPath}lvl2_0"),
                IconBack = Resources.Load<Sprite>($"{MapIconsPath}lvl2_1"),
                Sides = new Side[]
                {
                    new Side
                    {
                        SceneName = "",
                        SideType = SideType.A,
                        CheckPoints = new CheckPoint[]
                        {
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}2_0"),
                                CheckPointId = "0",
                                CheckPointName = "开始",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}2_3"),
                                CheckPointId = "3",
                                CheckPointName = "干涉",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}2_end_3"),
                                CheckPointId = "end_3",
                                CheckPointName = "唤醒",
                                StrawBerriesName = null
                            }
                        }
                    },
                    new Side
                    {
                        SceneName = "",
                        SideType = SideType.B,
                        CheckPoints = new CheckPoint[]
                        {
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}2H_start"),
                                CheckPointId = "start",
                                CheckPointName = "开始",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}2H_03"),
                                CheckPointId = "03",
                                CheckPointName = "密码锁",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}2H_08b"),
                                CheckPointId = "08b",
                                CheckPointName = "梦幻祭坛",
                                StrawBerriesName = null
                            }
                        }
                    },
                    new Side
                    {
                        SceneName = "",
                        SideType = SideType.C,
                        CheckPoints = new CheckPoint[]
                        {
                            new CheckPoint
                            {
                                CheckPointIcon = null,
                                CheckPointId = "00",
                                CheckPointName = "",
                                StrawBerriesName = null
                            }
                        }
                    }
                }

            },
            new Map
            {

                MapName = "天空度假山庄",
                MapIndex = "第 3 章",
                MainColor = new Color(0.8156863f, 0.3294118f, 0.3490196f),
                SecondColor = new Color(1.0000000f,0.9725491f,0.4588236f),
                MountainMaterialState = new float[3]{ 0,1,0 },
                MoveOnCameraState = new CameraTargetState(new Vector3(-5.961f, 8.823f, 5.058f),new Vector3( 32.209f, 147.869f,0f),false),
                SelectCameraState = new CameraTargetState(new Vector3(-4.294f, 6.633f, 5.193f),new Vector3( -5.594f,-158.396f,0f),false),
                ReturnCameraState = new CameraTargetState(new Vector3(-5.156f, 6.648f, 2.934f),new Vector3(  5.767f, 177.984f,0f),false),
                IconFace = Resources.Load<Sprite>($"{MapIconsPath}lvl3_0"),
                IconBack = Resources.Load<Sprite>($"{MapIconsPath}lvl3_1"),
                Sides = new Side[]
                {
                    new Side
                    {
                        SceneName = "",
                        SideType = SideType.A,
                        CheckPoints = new CheckPoint[]
                        {
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}3_s0"),
                                CheckPointId = "s0",
                                CheckPointName = "开始",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}3_08-a"),
                                CheckPointId = "08-a",
                                CheckPointName = "乱七八糟",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}3_09-d"),
                                CheckPointId = "09-d",
                                CheckPointName = "电梯井",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}3_00-d"),
                                CheckPointId = "00-d",
                                CheckPointName = "总统套房",
                                StrawBerriesName = null
                            }
                        }
                    },
                    new Side
                    {
                        SceneName = "",
                        SideType = SideType.B,
                        CheckPoints = new CheckPoint[]
                        {
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}3H_00"),
                                CheckPointId = "00",
                                CheckPointName = "开始",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}3H_06"),
                                CheckPointId = "06",
                                CheckPointName = "职工宿舍",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}3H_11"),
                                CheckPointId = "11",
                                CheckPointName = "图书馆",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}3H_16"),
                                CheckPointId = "16",
                                CheckPointName = "屋顶",
                                StrawBerriesName = null
                            }
                        }
                    },
                    new Side
                    {
                        SceneName = "",
                        SideType = SideType.C,
                        CheckPoints = new CheckPoint[]
                        {
                            new CheckPoint
                            {
                                CheckPointIcon = null,
                                CheckPointId = "00",
                                CheckPointName = "",
                                StrawBerriesName = null
                            }
                        }
                    }
                }

            },
            new Map
            {

                MapName = "黄金山脊",
                MapIndex = "第 4 章",
                MainColor = new Color(1.0000000f, 0.6156863f, 0.7137255f),
                SecondColor = new Color(0.5686275f,0.4509804f,0.9176471f),
                MountainMaterialState = new float[3]{ 0,0,1 },
                MoveOnCameraState = new CameraTargetState(new Vector3(-9.626f, 8.824f,-4.140f),new Vector3( 16.978f,  62.846f,0f),false),
                SelectCameraState = new CameraTargetState(new Vector3(-8.429f, 5.837f,-5.086f),new Vector3( -5.221f,  62.521f,0f),false),
                ReturnCameraState = new CameraTargetState(new Vector3(-7.389f, 5.461f,-3.631f),new Vector3(  -6.46f,  52.669f,0f),false),
                IconFace = Resources.Load<Sprite>($"{MapIconsPath}lvl4_0"),
                IconBack = Resources.Load<Sprite>($"{MapIconsPath}lvl4_1"),
                Sides = new Side[]
                {
                    new Side
                    {
                        SceneName = "",
                        SideType = SideType.A,
                        CheckPoints = new CheckPoint[]
                        {
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}4_a-00"),
                                CheckPointId = "a-00",
                                CheckPointName = "开始",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}4_b-00"),
                                CheckPointId = "b-00",
                                CheckPointName = "神迹",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}4_c-00"),
                                CheckPointId = "c-00",
                                CheckPointName = "古道",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}4_d-00"),
                                CheckPointId = "d-00",
                                CheckPointName = "悬崖峭壁",
                                StrawBerriesName = null
                            }
                        }
                    },
                    new Side
                    {
                        SceneName = "",
                        SideType = SideType.B,
                        CheckPoints = new CheckPoint[]
                        {
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}4H_a-00"),
                                CheckPointId = "a-00",
                                CheckPointName = "开始",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}4H_b-00"),
                                CheckPointId = "b-00",
                                CheckPointName = "踏脚石",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}4H_c-00"),
                                CheckPointId = "c-00",
                                CheckPointName = "大风峡谷",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}4H_d-00"),
                                CheckPointId = "d-00",
                                CheckPointName = "暴风之眼",
                                StrawBerriesName = null
                            }
                        }
                    },
                    new Side
                    {
                        SceneName = "",
                        SideType = SideType.C,
                        CheckPoints = new CheckPoint[]
                        {
                            new CheckPoint
                            {
                                CheckPointIcon = null,
                                CheckPointId = "00",
                                CheckPointName = "",
                                StrawBerriesName = null
                            }
                        }
                    }
                }

            },
            new Map
            {

                MapName = "镜之寺庙",
                MapIndex = "第 5 章",
                MainColor = new Color(0.6901961f, 0.2078432f, 0.9333334f),
                SecondColor = new Color(0.9921569f,0.5294118f,1.0000000f),
                MountainMaterialState = new float[3]{ 0,0,1 },
                MoveOnCameraState = new CameraTargetState(new Vector3( 0.963f,10.542f,-5.314f),new Vector3( 28.487f, -26.521f,0f),false),
                SelectCameraState = new CameraTargetState(new Vector3(-1.786f, 8.760f,-5.080f),new Vector3( -1.432f,  40.253f,0f),false),
                ReturnCameraState = new CameraTargetState(new Vector3( 0.079f, 9.274f,-4.312f),new Vector3(  5.425f,  17.215f,0f),false),
                IconFace = Resources.Load<Sprite>($"{MapIconsPath}lvl5_0"),
                IconBack = Resources.Load<Sprite>($"{MapIconsPath}lvl5_1"),
                Sides = new Side[]
                {
                    new Side
                    {
                        SceneName = "",
                        SideType = SideType.A,
                        CheckPoints = new CheckPoint[]
                        {
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}5_a-00b"),
                                CheckPointId = "a-00b",
                                CheckPointName = "开始",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}5_b-00"),
                                CheckPointId = "b-00",
                                CheckPointName = "深渊",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}5_c-00"),
                                CheckPointId = "c-00",
                                CheckPointName = "豁然开朗",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}5_d-00"),
                                CheckPointId = "d-00",
                                CheckPointName = "搜寻",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}5_e-00"),
                                CheckPointId = "e-00",
                                CheckPointName = "营救",
                                StrawBerriesName = null
                            }
                        }
                    },
                    new Side
                    {
                        SceneName = "",
                        SideType = SideType.B,
                        CheckPoints = new CheckPoint[]
                        {
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}5H_start"),
                                CheckPointId = "start",
                                CheckPointName = "开始",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}5H_b-00"),
                                CheckPointId = "b-00",
                                CheckPointName = "中庭",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}5H_c-00"),
                                CheckPointId = "c-00",
                                CheckPointName = "穿过镜子",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}5H_d-00"),
                                CheckPointId = "d-00",
                                CheckPointName = "混音大师",
                                StrawBerriesName = null
                            }
                        }
                    },
                    new Side
                    {
                        SceneName = "",
                        SideType = SideType.C,
                        CheckPoints = new CheckPoint[]
                        {
                            new CheckPoint
                            {
                                CheckPointIcon = null,
                                CheckPointId = "00",
                                CheckPointName = "",
                                StrawBerriesName = null
                            }
                        }
                    }
                }

            },
            new Map
            {

                MapName = "沉思",
                MapIndex = "第 6 章",
                MainColor = new Color(0.3568628f, 0.7098039f, 1.0000000f),
                SecondColor = new Color(0.3019608f,0.4313726f,0.9294118f),
                MountainMaterialState = new float[3]{ 1,0,0 },
                MoveOnCameraState = new CameraTargetState(new Vector3(-1.113f,12.154f, 6.334f),new Vector3( 31.212f, 135.474f,0f),false),
                SelectCameraState = new CameraTargetState(new Vector3(-1.113f,12.154f, 6.334f),new Vector3( 29.306f, 174.472f,0f),false),
                ReturnCameraState = new CameraTargetState(new Vector3( 0.084f,10.304f, 3.209f),new Vector3( 29.505f, 163.272f,0f),false),
                IconFace = Resources.Load<Sprite>($"{MapIconsPath}lvl6_0"),
                IconBack = Resources.Load<Sprite>($"{MapIconsPath}lvl6_1"),
                Sides = new Side[]
                {
                    new Side
                    {
                        SceneName = "",
                        SideType = SideType.A,
                        CheckPoints = new CheckPoint[]
                        {
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}6_start"),
                                CheckPointId = "start",
                                CheckPointName = "开始",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}6_00"),
                                CheckPointId = "00",
                                CheckPointName = "湖",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}6_04"),
                                CheckPointId = "04",
                                CheckPointName = "洞穴",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}6_b-00"),
                                CheckPointId = "b-00",
                                CheckPointName = "深思",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}6_boss-00"),
                                CheckPointId = "boss-00",
                                CheckPointName = "谷底",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}6_after-00"),
                                CheckPointId = "after-00",
                                CheckPointName = "决断",
                                StrawBerriesName = null
                            }
                        }
                    },
                    new Side
                    {
                        SceneName = "",
                        SideType = SideType.B,
                        CheckPoints = new CheckPoint[]
                        {
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}6H_a-00"),
                                CheckPointId = "a-00",
                                CheckPointName = "开始",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}6H_b-00"),
                                CheckPointId = "b-00",
                                CheckPointName = "深思",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}6H_c-00"),
                                CheckPointId = "c-00",
                                CheckPointName = "谷底",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}6H_d-00"),
                                CheckPointId = "d-00",
                                CheckPointName = "暂缓执行",
                                StrawBerriesName = null
                            }
                        }
                    },
                    new Side
                    {
                        SceneName = "",
                        SideType = SideType.C,
                        CheckPoints = new CheckPoint[]
                        {
                            new CheckPoint
                            {
                                CheckPointIcon = null,
                                CheckPointId = "00",
                                CheckPointName = "",
                                StrawBerriesName = null
                            }
                        }
                    }
                }

            },
            new Map
            {

                MapName = "山顶",
                MapIndex = "第 7 章",
                MainColor = new Color(1.0000000f, 0.9333334f, 0.2941177f),
                SecondColor = new Color(0.1176471f,0.5843138f,0.909804f),
                MountainMaterialState = new float[3]{ 0,1,0 },
                MoveOnCameraState = new CameraTargetState(new Vector3(14.620f, 3.606f,19.135f),new Vector3(-14.744f,-129.798f,0f),false),
                SelectCameraState = new CameraTargetState(new Vector3(13.453f, 5.141f,18.179f),new Vector3(-17.761f,-125.727f,0f),false),
                ReturnCameraState = new CameraTargetState(new Vector3( 9.604f, 6.691f,13.034f),new Vector3(-18.687f,-141.104f,0f),false),
                IconFace = Resources.Load<Sprite>($"{MapIconsPath}lvl7_0"),
                IconBack = Resources.Load<Sprite>($"{MapIconsPath}lvl7_1"),
                Sides = new Side[]
                {
                    new Side
                    {
                        SceneName = "",
                        SideType = SideType.A,
                        CheckPoints = new CheckPoint[]
                        {
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}7_a-00-intro"),
                                CheckPointId = "a-00-intro",
                                CheckPointName = "开始",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}7_b-00"),
                                CheckPointId = "b-00",
                                CheckPointName = "500米",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}7_c-00"),
                                CheckPointId = "c-00",
                                CheckPointName = "1000米",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}7_d-00"),
                                CheckPointId = "d-00",
                                CheckPointName = "1500米",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}7_e-00b"),
                                CheckPointId = "e-00b",
                                CheckPointName = "2000米",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}7_f-00"),
                                CheckPointId = "f-00",
                                CheckPointName = "2500米",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}7_g-00"),
                                CheckPointId = "g-00",
                                CheckPointName = "3000米",
                                StrawBerriesName = null
                            }
                        }
                    },
                    new Side
                    {
                        SceneName = "",
                        SideType = SideType.B,
                        CheckPoints = new CheckPoint[]
                        {
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}7H_a-00-intro"),
                                CheckPointId = "a-00-intro",
                                CheckPointName = "开始",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}7H_b-00"),
                                CheckPointId = "b-00",
                                CheckPointName = "500米",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}7H_c-01"),
                                CheckPointId = "c-01",
                                CheckPointName = "1000米",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}7H_d-00"),
                                CheckPointId = "d-00",
                                CheckPointName = "1500米",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}7H_e-00"),
                                CheckPointId = "e-00",
                                CheckPointName = "2000米",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}7H_f-00"),
                                CheckPointId = "f-00",
                                CheckPointName = "2500米",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}7H_g-00"),
                                CheckPointId = "g-00",
                                CheckPointName = "3000米",
                                StrawBerriesName = null
                            }
                        }
                    },
                    new Side
                    {
                        SceneName = "",
                        SideType = SideType.C,
                        CheckPoints = new CheckPoint[]
                        {
                            new CheckPoint
                            {
                                CheckPointIcon = null,
                                CheckPointId = "01",
                                CheckPointName = "",
                                StrawBerriesName = null
                            }
                        }
                    }
                }

            },
            new Map
            {

                MapName = "尾声",
                MapIndex = "",
                MainColor = new Color(0.2352941f, 0.3529412f, 0.4196079f),
                SecondColor = new Color(0.3647059f,0.8000001f,0.8705883f),
                MountainMaterialState = new float[3]{ 1,0,0 },
                MoveOnCameraState = new CameraTargetState(new Vector3( 1.234f, 0.677f, 7.598f),new Vector3( -1.634f,-149.548f,0f),false),
                SelectCameraState = new CameraTargetState(new Vector3( 1.234f, 0.677f, 7.598f),new Vector3( -0.487f,-141.537f,0f),false),
                ReturnCameraState = new CameraTargetState(new Vector3( 1.121f, 0.663f, 7.332f),new Vector3(  2.321f,-154.987f,0f),false),
                IconFace = Resources.Load<Sprite>($"{MapIconsPath}lvl0_0"),
                IconBack = Resources.Load<Sprite>($"{MapIconsPath}lvl0_1"),
                Sides = new Side[]
                {
                    new Side
                    {
                        SceneName = "",
                        SideType = SideType.Plot,
                        CheckPoints = new CheckPoint[]
                        {
                            new CheckPoint
                            {
                                CheckPointIcon = null,
                                CheckPointId = "outside",
                                CheckPointName = "",
                                StrawBerriesName = null
                            }
                        }
                    }
                }

            },
            new Map
            {

                MapName = "核心",
                MapIndex = "第 8 章",
                MainColor = new Color(0.5647059f, 0.1529412f, 0.2235294f),
                SecondColor = new Color(1.0000000f,0.2156863f,0.3098039f),
                MountainMaterialState = new float[3]{ 0,0,1 },
                MoveOnCameraState = new CameraTargetState(new Vector3( 4.473f, 7.158f, 5.463f),new Vector3( 14.418f,-154.202f,0f),false),
                SelectCameraState = new CameraTargetState(new Vector3( 3.404f, 6.677f, 3.846f),new Vector3( 13.743f,-137.546f,0f),false),
                ReturnCameraState = new CameraTargetState(new Vector3( 3.614f, 5.999f, 0.422f),new Vector3( 10.636f, -99.491f,0f),false),
                IconFace = Resources.Load<Sprite>($"{MapIconsPath}lvl8_0"),
                IconBack = Resources.Load<Sprite>($"{MapIconsPath}lvl8_1"),
                Sides = new Side[]
                {
                    new Side
                    {
                        SceneName = "",
                        SideType = SideType.A,
                        CheckPoints = new CheckPoint[]
                        {
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}9_00"),
                                CheckPointId = "00",
                                CheckPointName = "开始",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}9_a-00"),
                                CheckPointId = "a-00",
                                CheckPointName = "深入腹地",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}9_c-00"),
                                CheckPointId = "c-00",
                                CheckPointName = "冰火两重天",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}9_d-00"),
                                CheckPointId = "d-00",
                                CheckPointName = "山之心",
                                StrawBerriesName = null
                            }
                        }
                    },
                    new Side
                    {
                        SceneName = "",
                        SideType = SideType.B,
                        CheckPoints = new CheckPoint[]
                        {
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}9H_00"),
                                CheckPointId = "00",
                                CheckPointName = "开始",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}9H_a-00"),
                                CheckPointId = "a-00",
                                CheckPointName = "深入腹地",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}9H_b-00"),
                                CheckPointId = "b-00",
                                CheckPointName = "山之心",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}9H_c-01"),
                                CheckPointId = "c-01",
                                CheckPointName = "心跳",
                                StrawBerriesName = null
                            }
                        }
                    },
                    new Side
                    {
                        SceneName = "",
                        SideType = SideType.C,
                        CheckPoints = new CheckPoint[]
                        {
                            new CheckPoint
                            {
                                CheckPointIcon = null,
                                CheckPointId = "intro",
                                CheckPointName = "",
                                StrawBerriesName = null
                            }
                        }
                    }
                }

            },
            new Map
            {

                MapName = "再见",
                MapIndex = "第 9 章",
                MainColor = new Color(0.3058824f, 0.1254902f, 0.682353f),
                SecondColor = new Color(1.0000000f,0.5411765f,0.8627452f),
                MountainMaterialState = new float[3]{ 0,0,1 },
                MoveOnCameraState = new CameraTargetState(new Vector3( 6.032f,33.050f, 7.698f),new Vector3(   3.21f,-140.955f,0f),true),
                SelectCameraState = new CameraTargetState(new Vector3(-5.881f,31.525f, 2.871f),new Vector3(  1.261f, 131.938f,0f),false),
                ReturnCameraState = new CameraTargetState(new Vector3(-2.505f,31.069f, 1.248f),new Vector3(  0.172f, 118.551f,0f),false),
                IconFace = Resources.Load<Sprite>($"{MapIconsPath}lvl9_0"),
                IconBack = Resources.Load<Sprite>($"{MapIconsPath}lvl9_1"),
                Sides = new Side[]
                {
                    new Side
                    {
                        SceneName = "",
                        SideType = SideType.A,
                        CheckPoints = new CheckPoint[]
                        {
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}10_intro-00-past"),
                                CheckPointId = "intro-00-past",
                                CheckPointName = "开始",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}10_a-00"),
                                CheckPointId = "a-00",
                                CheckPointName = "一个人",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}10_c-00"),
                                CheckPointId = "c-00",
                                CheckPointName = "能源",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}10_e-00z"),
                                CheckPointId = "e-00z",
                                CheckPointName = "铭记",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}10_f-door"),
                                CheckPointId = "f-door",
                                CheckPointName = "视界线",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}10_h-00b"),
                                CheckPointId = "h-00b",
                                CheckPointName = "决心",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}10_i-00"),
                                CheckPointId = "i-00",
                                CheckPointName = "倔强",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}10_j-00"),
                                CheckPointId = "j-00",
                                CheckPointName = "和谐",
                                StrawBerriesName = null
                            },
                            new CheckPoint
                            {
                                CheckPointIcon = Resources.Load<Sprite>($"{CheckPointsPath}10_j-16"),
                                CheckPointId = "j-16",
                                CheckPointName = "再见",
                                StrawBerriesName = null
                            }
                        }
                    }
                }

            }

        };

        public class Map
        {

            public CameraTargetState MoveOnCameraState;

            public CameraTargetState SelectCameraState;

            public CameraTargetState ReturnCameraState;

            public float[] MountainMaterialState;

            public Color MainColor;

            public Color SecondColor;

            public string MapName;

            public string MapIndex;

            public Sprite IconFace;

            public Sprite IconBack;

            public Side[] Sides;

            private Archive.SaveFile.MapRecord mapRecord;

            public Archive.SaveFile.MapRecord MapRecord
            {

                get
                {

                    return mapRecord;

                }
                set
                {

                    mapRecord = value;

                    if (Sides == null)
                    {

                        return;

                    }

                    for (int i = 0; i < Sides.Length; ++i)
                    {

                        Sides[i].SideRecord = null;

                    }

                    if(mapRecord == null || mapRecord.sideRecords == null)
                    {

                        return;

                    }

                    for (int i = 0; i < mapRecord.sideRecords.Length; ++i)
                    {

                        Sides[mapRecord.sideRecords[i].SideIndex].SideRecord = mapRecord.sideRecords[i];

                    }

                }

            }

        }

        public class Side
        {

            public string SceneName;

            public CheckPoint[] CheckPoints;

            public SideType SideType;

            public int GetStrawberryNum()
            {

                int Result = 0;

                if(CheckPoints != null)
                {

                    for(int i = 0; i < CheckPoints.Length; ++i)
                    {

                        Result += CheckPoints[i].StrawBerriesName.Length;

                    }

                }

                return Result;

            }

            private Archive.SaveFile.MapRecord.SideRecord sideRecord;

            public Archive.SaveFile.MapRecord.SideRecord SideRecord
            {

                get
                {

                    return sideRecord;

                }
                set
                {

                    sideRecord = value;

                    if(CheckPoints == null)
                    {

                        return;

                    }

                    for(int i = 0; i < CheckPoints.Length; ++i)
                    {

                        CheckPoints[i].CheckPointRecord = null;

                    }

                    if (SideRecord == null || SideRecord.checkPointRecords == null)
                    {

                        return;

                    }

                    for (int i = 0; i < SideRecord.checkPointRecords.Count; ++i)
                    {

                        CheckPoints[SideRecord.checkPointRecords[i].CheckPointIndex].CheckPointRecord = SideRecord.checkPointRecords[i];

                    }

                }

            }

        }

        public enum SideType
        {

            A,
            B,
            C,
            Plot

        }

        public class CheckPoint
        {

            public string CheckPointName;

            public Sprite CheckPointIcon;

            public string CheckPointId;

            public string[] StrawBerriesName;

            private CheckPointRecord checkPointRecord;

            public CheckPointRecord CheckPointRecord
            {

                get
                {

                    return checkPointRecord;

                }
                set
                {

                    checkPointRecord = value;

                }

            }

        }

    }

    public static class SceneOnloadVarible
    {

        public static class MainScene
        {



        }

        public static class GameScene
        {

            public static MapsInfo.Map CurrentMap;

            public static MapsInfo.Side CurrentSide;

            public static MapsInfo.CheckPoint CurrentCheckPoint;

            private static Archive.SaveFile currentSave;

            public static Archive.SaveFile CurrentSave
            {

                get
                {

                    return currentSave;

                }
                set
                {

                    currentSave = value;

                    for(int i = 0; i < MapsInfo.infos.Length; ++i)
                    {

                        MapsInfo.infos[i].MapRecord = null;

                    }

                    if(currentSave == null)
                    {

                        return;

                    }

                    for (int i = 0; i < currentSave.mapRecords.Length; ++i)
                    {

                        MapsInfo.infos[currentSave.mapRecords[i].MapIndex].MapRecord = currentSave.mapRecords[i];

                    }

                }

            }

        }

    }

    public static class Extension
    {

        public static void Write(this BinaryWriter writer, TimeSpan timeSpan)
        {

            writer.Write(timeSpan.Days);

            writer.Write(timeSpan.Hours);

            writer.Write(timeSpan.Minutes);

            writer.Write(timeSpan.Seconds);

            writer.Write(timeSpan.Milliseconds);

        }

        public static TimeSpan ReadTimeSpan(this BinaryReader reader)
        {

            return new TimeSpan(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());

        }

    }

}