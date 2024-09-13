using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class MoveBlockWithTrack : MonoBehaviour, IRoomEvent
{

    /// <summary>
    /// �ƶ���
    /// </summary>
    [SerializeField] private MoveBlock moveBlock;
    /// <summary>
    /// �ƶ�·��
    /// </summary>
    [SerializeField] private MoveTrack moveTrack;
    /// <summary>
    /// �ƶ�·����Ƥ��Ԥ����
    /// </summary>
    [SerializeField] private TrackConnect TrackConnectPrefab;
    /// <summary>
    /// �ƶ�·��Ƥ������
    /// </summary>
    [SerializeField] private Transform TrackConnectContainer;

    /// <summary>
    /// �ƶ�·��
    /// </summary>
    [Serializable]
    private struct MoveTrack
    {

        /// <summary>
        /// ���
        /// </summary>
        public MoveTrackPoint start;
        /// <summary>
        /// �յ�
        /// </summary>
        public MoveTrackPoint end;
        /// <summary>
        /// ����������������Ƥ��
        /// </summary>
        public TrackConnect left;
        /// <summary>
        /// ����������Ҳ�����Ƥ��
        /// </summary>
        public TrackConnect right;
        /// <summary>
        /// λ�Ʋ�ֵ
        /// </summary>
        public float positionLerpValue;
        /// <summary>
        /// ·����ʵ�ʾ���
        /// </summary>
        public float RealDistance;

    }
    /// <summary>
    /// �ƶ�·����
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
    /// �ƶ�״̬
    /// </summary>
    private enum MoveState
    {

        Award,
        Back,
        Ready,
        Shake

    }

    /// <summary>
    /// ǰ������ٶ�
    /// </summary>
    [SerializeField] private float AwardMaxSpeed;
    /// <summary>
    /// ǰ�����ٶ�
    /// </summary>
    [SerializeField] private float AwardAcceleration;
    /// <summary>
    /// ��������ٶ�
    /// </summary>
    [SerializeField] private float BackMaxSpeed;
    /// <summary>
    /// ���ؼ��ٶ�
    /// </summary>
    [SerializeField] private float BackAcceleration;
    /// <summary>
    /// �ƶ�״̬
    /// </summary>
    [SerializeField] private MoveState moveState;
    /// <summary>
    /// �ƶ�(��)�ٶ�
    /// </summary>
    [SerializeField] private float velocity;

    /// <summary>
    /// ��ʼ��·��
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
    /// ��ʼ���ƶ���
    /// </summary>
    private void InitBlock()
    {

        moveBlock.gameObject.SetActive(true);

        moveBlock.InitBlock();
        //���ƶ����ƶ������λ��
        moveBlock.Position = moveTrack.start.Position;

    }
    /// <summary>
    /// �������
    /// </summary>
    /// <returns></returns>
    private IEnumerator GoBack()
    {

        GameSceneMusicManager.instance.mapItem.zipMove.OnImpact();

        moveBlock.transform.DOShakePosition(Metric.MoveBlock.ShakeTime, Metric.MoveBlock.ShakeStrength, 20);
        //�ȴ�ʱ��Ϊ�ƶ����յ����ͣ������ʱ��
        yield return new WaitForSeconds(0.5f);
        //
        GameSceneMusicManager.instance.mapItem.zipMove.OnReturn();
        //����Ϊ�������
        moveState = MoveState.Back;

    }
    /// <summary>
    /// <para/>�����ƶ�
    /// <para/>ֻ�е��ƶ��鴦�ڿ�ʼλ��ʱ�ŻῪʼ�ƶ�
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
    /// �������
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

            //λ�ò�ֵ����
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

        //ʵ��λ�ü���
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

        //��ʼ��·��
        InitTrack();
        //��ʼ���ƶ���
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
