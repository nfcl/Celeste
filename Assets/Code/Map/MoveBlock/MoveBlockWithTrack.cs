using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class MoveBlockWithTrack : MonoBehaviour, IRoomEvent
{

    /// <summary>
    /// 移动块
    /// </summary>
    [SerializeField] private MoveBlock moveBlock;
    /// <summary>
    /// 移动路径
    /// </summary>
    [SerializeField] private MoveTrack moveTrack;
    /// <summary>
    /// 移动路径的皮带预设体
    /// </summary>
    [SerializeField] private TrackConnect TrackConnectPrefab;
    /// <summary>
    /// 移动路径皮带容器
    /// </summary>
    [SerializeField] private Transform TrackConnectContainer;

    /// <summary>
    /// 移动路径
    /// </summary>
    [Serializable]
    private struct MoveTrack
    {

        /// <summary>
        /// 起点
        /// </summary>
        public MoveTrackPoint start;
        /// <summary>
        /// 终点
        /// </summary>
        public MoveTrackPoint end;
        /// <summary>
        /// 相对于起点的左侧连接皮带
        /// </summary>
        public TrackConnect left;
        /// <summary>
        /// 相对于起点的右侧连接皮带
        /// </summary>
        public TrackConnect right;
        /// <summary>
        /// 位移插值
        /// </summary>
        public float positionLerpValue;
        /// <summary>
        /// 路径的实际距离
        /// </summary>
        public float RealDistance;

    }
    /// <summary>
    /// 移动路径点
    /// </summary>
    [Serializable]
    private struct MoveTrackPoint
    {

        public Transform source;

        public ConveyorChiLun ChiLun;

        public Vector2 Position{

            get
            {

                return source.position;

            }

        }

    }

    /// <summary>
    /// 移动状态
    /// </summary>
    private enum MoveState
    {

        Award,
        Back,
        Ready,
        Shake

    }

    /// <summary>
    /// 前进最大速度
    /// </summary>
    [SerializeField] private float AwardMaxSpeed;
    /// <summary>
    /// 前进加速度
    /// </summary>
    [SerializeField] private float AwardAcceleration;
    /// <summary>
    /// 返回最大速度
    /// </summary>
    [SerializeField] private float BackMaxSpeed;
    /// <summary>
    /// 返回加速度
    /// </summary>
    [SerializeField] private float BackAcceleration;
    /// <summary>
    /// 移动状态
    /// </summary>
    [SerializeField] private MoveState moveState;
    /// <summary>
    /// 移动(合)速度
    /// </summary>
    [SerializeField] private float velocity;

    /// <summary>
    /// 初始化路径
    /// </summary>
    private void InitTrack()
    {

        moveTrack.positionLerpValue = 0;

        moveTrack.RealDistance = Vector2.Distance(moveTrack.start.Position, moveTrack.end.Position);

        float euler;

        if(moveTrack.left == null)
        {

            moveTrack.left = GameObject.Instantiate(TrackConnectPrefab, TrackConnectContainer).GetComponent<TrackConnect>();

        }

        euler = -Mathf.Atan(
                (moveTrack.end.Position.x - moveTrack.start.Position.x)
                / (moveTrack.end.Position.y - moveTrack.start.Position.y)
            );

        moveTrack.left.transform.localEulerAngles = new Vector3(
            0,
            0,
            euler * 180 / Mathf.PI
        );

        moveTrack.left.transform.position = (moveTrack.end.Position + moveTrack.start.Position) / 2 + new Vector2(Mathf.Cos(euler), Mathf.Sin(euler)) * -15;

        moveTrack.left.Flip = false;

        moveTrack.left.length = moveTrack.RealDistance / 100f;

        moveTrack.left.LerpValue = 0;

        if(moveTrack.right == null)
        {

            moveTrack.right = GameObject.Instantiate(TrackConnectPrefab, TrackConnectContainer).GetComponent<TrackConnect>();

        }

        moveTrack.right.transform.localEulerAngles = new Vector3(
            0,
            0,
            euler * 180 / Mathf.PI
        );

        moveTrack.right.transform.position = (moveTrack.end.Position + moveTrack.start.Position) / 2 + new Vector2(Mathf.Cos(euler), Mathf.Sin(euler)) * 15;

        moveTrack.right.Flip = true;

        moveTrack.right.length = moveTrack.RealDistance / 100f;

        moveTrack.right.LerpValue = 1;

    }
    /// <summary>
    /// 初始化移动块
    /// </summary>
    private void InitBlock()
    {

        moveBlock.gameObject.SetActive(true);

        moveBlock.InitBlock();
        //将移动块移动至起点位置
        moveBlock.Position = moveTrack.start.Position;

    }
    /// <summary>
    /// 返回起点
    /// </summary>
    /// <returns></returns>
    private IEnumerator GoBack()
    {

        GameSceneMusicManager.instance.mapItem.zipMove.OnImpact();

        moveBlock.transform.DOShakePosition(Metric.MoveBlock.ShakeTime, Metric.MoveBlock.ShakeStrength, 20);
        //等待时间为移动到终点后暂停不动的时间
        yield return new WaitForSeconds(0.5f);
        //
        GameSceneMusicManager.instance.mapItem.zipMove.OnReturn();
        //更改为返回起点
        moveState = MoveState.Back;

    }
    /// <summary>
    /// <para/>尝试移动
    /// <para/>只有当移动块处于开始位置时才会开始移动
    /// </summary>
    public void TryMoveStart()
    {

        if(moveState == MoveState.Ready)
        {

            moveState = MoveState.Shake;

            moveBlock.transform.DOShakePosition(Metric.MoveBlock.ShakeTime, Metric.MoveBlock.ShakeStrength, 20).OnComplete(() => moveState = MoveState.Award);

            GameSceneMusicManager.instance.mapItem.zipMove.OnTouch();

        }

    }
    /// <summary>
    /// 物理更新
    /// </summary>
    public void FixedUpdate()
    {
        
        if(moveState == MoveState.Award)
        {

            velocity += AwardAcceleration * Time.deltaTime;

            if(velocity > AwardMaxSpeed)
            {

                velocity = AwardMaxSpeed;

            }

            //位置插值计算
            moveTrack.positionLerpValue += velocity * Time.fixedDeltaTime;

            if(moveTrack.positionLerpValue > 1)
            {

                moveTrack.positionLerpValue = 1;

                velocity = 0;

                moveState = MoveState.Shake;

                StartCoroutine(GoBack());

            }

        }
        else if(moveState == MoveState.Back)
        {

            velocity += BackAcceleration * Time.deltaTime;

            if (velocity > BackMaxSpeed)
            {

                velocity = BackMaxSpeed;

            }

            moveTrack.positionLerpValue -= velocity * Time.fixedDeltaTime;
            if (moveTrack.positionLerpValue < 0)
            {

                moveTrack.positionLerpValue = 0;

                velocity = 0;

                moveState = MoveState.Shake;

                moveBlock.transform.DOShakePosition(Metric.MoveBlock.ShakeTime, Metric.MoveBlock.ShakeStrength, 20).OnComplete(() => moveState = MoveState.Ready);

                GameSceneMusicManager.instance.mapItem.zipMove.OnReset();

            }

        }

        //实际位置计算
        moveBlock.Position = Vector2.Lerp(
            moveTrack.start.Position,
            moveTrack.end.Position,
            moveTrack.positionLerpValue
        );

        moveTrack.left.LerpValue = moveTrack.positionLerpValue;

        moveTrack.right.LerpValue = 1 - moveTrack.positionLerpValue;

        moveTrack.start.ChiLun.SetClipIndex(moveTrack.positionLerpValue, moveTrack.left.length);

        moveTrack.end.ChiLun.SetClipIndex(moveTrack.positionLerpValue, moveTrack.right.length);

        moveBlock.speed = velocity * 20;

    }

    public Vector2 GetBlockVelocity()
    {

        if(moveState == MoveState.Ready)
        {

            return Vector2.zero;

        }

        Vector2 Result;

        if(moveState == MoveState.Award)
        {

            Result = (moveTrack.end.Position - moveTrack.start.Position).normalized;

        }
        else
        {

            Result = (moveTrack.start.Position - moveTrack.end.Position).normalized;

        }

        Result *= velocity;

        return Result;

    }

    public void Start()
    {

        //初始化路径
        InitTrack();
        //初始化移动块
        InitBlock();

    }

    public void EnterRoomInit()
    {

        StopAllCoroutines();

        moveState = MoveState.Ready;

        velocity = 0;

        moveTrack.positionLerpValue = 0;

        moveTrack.left.LerpValue = moveTrack.positionLerpValue;

        moveTrack.right.LerpValue = 1 - moveTrack.positionLerpValue;

        moveTrack.start.ChiLun.SetClipIndex(moveTrack.positionLerpValue, moveTrack.left.length);

        moveTrack.end.ChiLun.SetClipIndex(moveTrack.positionLerpValue, moveTrack.right.length);

        moveBlock.Position = moveTrack.start.Position;

    }

//#if UNITY_EDITOR

//    public void OnValidate()
//    {

//        InitTrack();

//        moveBlock.transform.position = moveTrack.start.Position;

//    }

//#endif

}
