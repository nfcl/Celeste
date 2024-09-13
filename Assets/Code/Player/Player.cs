using Managers;
using Map;
using Metric;
using n_Player.PlayerEventInterface;
using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace n_Player
{

    public class Player : MonoBehaviour, JumpBoard.IJumpBoardJumpAble, IFollowedable
    {

        #region ����
        private struct StateInfo
        {

            public bool Past;
            public bool Current;

        }
        /// <summary>
        /// ʵ�����뷽��
        /// </summary>
        private bool[] RealDirection;
        /// <summary>
        /// ��һ֡��������������
        /// </summary>
        private bool[] LastDirectionState;
        /// <summary>
        /// ��ǰ֡��������������
        /// </summary>
        private bool[] CurrentDirectionState;
        /// <summary>
        /// �����Ƿ���
        /// </summary>
        private bool isTowardLeft;
        /// <summary>
        /// �����Ƿ���(ֻ���ڷ�����ж�)
        /// </summary>
        private bool isTowardUp;
        /// <summary>
        /// �Ƿ�ӵ�
        /// </summary>
        [SerializeField] private bool IsOnGround;
        /// <summary>
        /// �ƶ�״̬
        /// </summary>
        [SerializeField] private MoveState moveState;
        /// <summary>
        /// ��Ծ״̬
        /// </summary>
        [SerializeField] private JumpState jumpState;
        /// <summary>
        /// ץǽ״̬
        /// </summary>
        [SerializeField] private CatchState CurrentCatchState;
        [SerializeField] private CatchState LastCatchState;
        /// <summary>
        /// ����״̬
        /// </summary>
        [SerializeField] private ClimbState climbState;
        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private DashState dashState;
        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private StunState stunState;
        /// <summary>
        /// �ɿ��Ƶ�
        /// </summary>
        private bool isControlable;
        /// <summary>
        /// ��ǰ֡��Ծ��״̬
        /// </summary>
        private bool CurrentJumpState;
        /// <summary>
        /// ��һ֡��Ծ��״̬
        /// </summary>
        private bool LastJumpState;
        /// <summary>
        /// �����Ϳ�״̬
        /// </summary>
        private bool SpecialStagnantCondition;
        /// <summary>
        /// ��ǰ֡ץȡ״̬
        /// </summary>
        private bool CurrentCatchKeyState;
        /// <summary>
        /// ��һ֡ץȡ״̬
        /// </summary>
        private bool LastCatchKeyState;
        /// <summary>
        /// 
        /// </summary>
        private bool CurrentDashState;
        /// <summary>
        /// 
        /// </summary>
        private bool LastDashState;
        /// <summary>
        /// ��ǰ֡��ǽ��Ϣ
        /// </summary>
        private WallBesides CurrentWallInfo;
        /// <summary>
        /// ��һ֡��ǽ��Ϣ
        /// </summary>
        private WallBesides LastWallInfo;
        /// <summary>
        /// 
        /// </summary>
        private StateInfo StunButtonState = new StateInfo();
        /// <summary>
        /// �����뿪ƽ̨��֡ʱ��
        /// </summary>
        private int WolfTime;
        /// <summary>
        /// ���̿�ʼ��֡ʱ��
        /// </summary>
        private int DashTime;
        /// <summary>
        /// ʣ��ɳ�̴���
        /// </summary>
        private int LastDashNum;
        /// <summary>
        /// 
        /// </summary>
        public bool IsControlable
        {

            get
            {

                return isControlable;

            }

        }
        [Serializable]
        struct WallBesides
        {

            public GameObject LeftTop;
            public GameObject LeftBottom;
            public GameObject RightTop;
            public GameObject RightBottom;

            /// <summary>
            /// ������ǽ
            /// </summary>
            public bool IsBesideLeft
            {

                get
                {

                    return LeftTop != null || LeftBottom != null;

                }

            }
            /// <summary>
            /// ������ǽ
            /// </summary>
            public bool IsBesideRight
            {

                get
                {

                    return RightTop != null || RightBottom != null;

                }

            }
            /// <summary>
            /// ��ǽ������
            /// </summary>
            public GameObject LeftObject
            {

                get
                {

                    if (LeftBottom == null)
                    {

                        return LeftTop;

                    }

                    return LeftBottom;

                }

            }
            /// <summary>
            /// ��ǽ������
            /// </summary>
            public GameObject RightObject
            {

                get
                {

                    if (RightBottom == null)
                    {

                        return RightTop;

                    }

                    return RightBottom;

                }

            }

        }

        private enum MoveState
        {

            Left,
            Right,
            Ignore

        }

        private enum JumpState
        {

            Jump,
            Fall,
            Ignore

        }

        private enum CatchState
        {

            Left,
            Right,
            Ignore

        }

        private enum ClimbState
        {

            Up,
            Down,
            Ignore

        }

        private enum DashState
        {

            Up = 0,
            RightUp = 1,
            Right = 2,
            RightBottom = 3,
            Bottom = 4,
            LeftBottom = 5,
            Left = 6,
            LeftUp = 7,
            Ignore = 8

        }

        private enum StunState
        {

            Ignore,
            Stun

        }

        #endregion

        #region ���

        /// <summary>
        /// ����
        /// </summary>
        [SerializeField] private Rigidbody2D rb;
        /// <summary>
        /// ����״̬��
        /// </summary>
        [SerializeField] private Animator animator;
        /// <summary>
        /// ������ײ��
        /// </summary>
        [SerializeField] private BoxCollider2D bodyCollider;
        /// <summary>
        /// 
        /// </summary>
        [SerializeField] private SpriteRenderer sprite;
        /// <summary>
        /// �������
        /// </summary>
        [SerializeField] private CameraControl cameraControl;
        /// <summary>
        /// ��ǰ���ڵķ���
        /// </summary>
        [SerializeField] private Room CurrentRoom;
        /// <summary>
        /// ����������
        /// </summary>
        [SerializeField] private Follow follow;
        /// <summary>
        /// �����β
        /// </summary>
        [SerializeField] private ParticleTrail dashTrail;

        [SerializeField] private Effect effect;

        [Serializable]
        public class Effect
        {

            [SerializeField] private AudioSource Jump;

            [SerializeField] private AudioSource Death;

            [SerializeField] private AudioSource Born;

            [SerializeField] private AudioSource[] Dash;

            public void OnBorn()
            {

                Born.Play();

            }

            public void OnDeath()
            {

                Death.Play();

            }

            public void OnJump()
            {

                Jump.Play();

            }

            public void OnDash(bool isLeft)
            {

                Dash[isLeft ? 0 : 1].Play();

            }

        }

        #endregion

        #region ��������

        /// <summary>
        /// ������ٶ�
        /// </summary>
        private Vector2 acceleration;
        /// <summary>
        /// �����ٶ�
        /// </summary>
        private Vector2 veiocity;

        #endregion

        public int DeathNum { get; set; }

        public void Start()
        {

            Init();

            DeathNum = 0;

            LastEatStrawberryTime = 0;

            if (SceneOnloadVarible.GameScene.CurrentCheckPoint != null)
            {

            var roomList = GameObject.Find("Map/Rooms").GetComponentsInChildren<Room>();

#if UNITY_EDITOR

                bool __hasFindCheckPoint = false;

#endif

                for (int i = 0; i < roomList.Length; ++i)
                {

                    if (roomList[i].name == Metric.SceneOnloadVarible.GameScene.CurrentCheckPoint.CheckPointId)
                    {

                        CurrentRoom = roomList[i];

                        cameraControl.EnterRoom(CurrentRoom);

#if UNITY_EDITOR

                        __hasFindCheckPoint = true;

#endif

                        break;

                    }

                }

#if UNITY_EDITOR

                if (__hasFindCheckPoint == false)
                {

                    Metric.Debug.Log("δ�ҵ���Ӧ���ƵĴ浵��,ʹ��Ĭ�ϴ浵�㸴��");

                }

#endif

            }

            CurrentRoom.EnterRoomInit();

            GameSceneManager.instance.cutScene.ToggleChange(false, CutScene.Kind.Lvl1, 2, delegate { isControlable = true; });

            //StartCoroutine(Born());

        }
        /// <summary>
        /// ��ʼ��
        /// </summary>
        private void Init()
        {

            InitDirection();

            InitStates();

            acceleration = new Vector2(0, 0);

            DashTime = 1000;

            LastEatStrawberryTime = 0;

            LastDashNum = PlayerPhysical.Dash.MaxDashNum;

            cameraControl.EnterRoom(CurrentRoom);

            transform.SetParent(null);

        }
        /// <summary>
        /// 
        /// </summary>
        private void InitStates()
        {

            moveState = MoveState.Ignore;
            jumpState = JumpState.Ignore;
            CurrentCatchState = CatchState.Ignore;
            LastCatchState = CatchState.Ignore;
            climbState = ClimbState.Ignore;
            dashState = DashState.Ignore;
            stunState = StunState.Ignore;

        }
        /// <summary>
        /// �������ʼ��
        /// </summary>
        private void InitDirection()
        {

            RealDirection = new bool[4] { false, false, false, false };
            LastDirectionState = new bool[4] { false, false, false, false };
            CurrentDirectionState = new bool[4] { false, false, false, false };
            isTowardLeft = false;
            isTowardUp = false;

        }
        /// <summary>
        /// ���뷽����
        /// </summary>
        private void DirectionProcess()
        {

            CurrentDirectionState[0] = Input.GetKey(Metric.Settings.KeyBoard.Game_Left);
            CurrentDirectionState[1] = Input.GetKey(Metric.Settings.KeyBoard.Game_Up);
            CurrentDirectionState[2] = Input.GetKey(Metric.Settings.KeyBoard.Game_Right);
            CurrentDirectionState[3] = Input.GetKey(Metric.Settings.KeyBoard.Game_Down);

            if (CurrentDirectionState[0] == false && CurrentDirectionState[2] == false)
            {
                RealDirection[0] = false;
                RealDirection[2] = false;
            }
            else if (LastDirectionState[0] != CurrentDirectionState[0] || LastDirectionState[2] != CurrentDirectionState[2])
            {
                if (LastDirectionState[0] == true)
                {
                    if (LastDirectionState[2] == true)
                    {
                        if (CurrentDirectionState[0] == true)
                        {
                            //�ſ��Ҽ����ԭ��������Ҫ����
                            if (isTowardLeft == false)
                            {
                                isTowardLeft = true;
                                RealDirection[0] = true;
                                RealDirection[2] = false;
                            }
                        }
                        else
                        {
                            //�ſ�������ԭ��������Ҫ����
                            if (isTowardLeft == true)
                            {
                                isTowardLeft = false;
                                RealDirection[0] = false;
                                RealDirection[2] = true;
                            }
                        }
                    }
                    else
                    {
                        //�����л�����
                        isTowardLeft = false;
                        RealDirection[0] = false;
                        RealDirection[2] = true;
                    }
                }
                else
                {
                    if (LastDirectionState[2] == true)
                    {
                        //�����л�����
                        isTowardLeft = true;
                        RealDirection[0] = true;
                        RealDirection[2] = false;
                    }
                    else
                    {
                        if (CurrentDirectionState[0] == true)
                        {
                            if (CurrentDirectionState[2] == true)
                            {
                                //ͬʱ���� ���
                                if (UnityEngine.Random.Range(0, 2) == 0)
                                {
                                    isTowardLeft = true;
                                    RealDirection[0] = true;
                                    RealDirection[2] = false;
                                }
                                else
                                {
                                    isTowardLeft = false;
                                    RealDirection[0] = false;
                                    RealDirection[2] = true;
                                }
                            }
                            else
                            {
                                //������³���
                                isTowardLeft = true;
                                RealDirection[0] = true;
                                RealDirection[2] = false;
                            }
                        }
                        else
                        {
                            //�Ҽ����³���
                            isTowardLeft = false;
                            RealDirection[0] = false;
                            RealDirection[2] = true;
                        }
                    }
                }
            }

            if (CurrentDirectionState[1] == false && CurrentDirectionState[3] == false)
            {
                RealDirection[1] = false;
                RealDirection[3] = false;
            }
            else if (LastDirectionState[1] != CurrentDirectionState[1] || LastDirectionState[3] != CurrentDirectionState[3])
            {
                if (LastDirectionState[1] == true)
                {
                    if (LastDirectionState[3] == true)
                    {
                        if (CurrentDirectionState[1] == true)
                        {
                            //�ſ��¼����ԭ��������Ҫ����
                            if (isTowardUp == false)
                            {
                                isTowardUp = true;
                                RealDirection[1] = true;
                                RealDirection[3] = false;
                            }
                        }
                        else
                        {
                            //�ſ��ϼ����ԭ��������Ҫ����
                            if (isTowardUp == true)
                            {
                                isTowardUp = false;
                                RealDirection[1] = false;
                                RealDirection[3] = true;
                            }
                        }
                    }
                    else
                    {
                        //�����л�����
                        isTowardUp = false;
                        RealDirection[1] = false;
                        RealDirection[3] = true;
                    }
                }
                else
                {
                    if (LastDirectionState[3] == true)
                    {
                        //�����л�����
                        isTowardUp = true;
                        RealDirection[1] = true;
                        RealDirection[3] = false;
                    }
                    else
                    {
                        if (CurrentDirectionState[1] == true)
                        {
                            if (CurrentDirectionState[3] == true)
                            {
                                //ͬʱ���� ���
                                if (UnityEngine.Random.Range(0, 3) == 0)
                                {
                                    isTowardUp = true;
                                    RealDirection[1] = true;
                                    RealDirection[3] = false;
                                }
                                else
                                {
                                    isTowardUp = false;
                                    RealDirection[1] = false;
                                    RealDirection[3] = true;
                                }
                            }
                            else
                            {
                                //�ϼ����³���
                                isTowardUp = true;
                                RealDirection[1] = true;
                                RealDirection[3] = false;
                            }
                        }
                        else
                        {
                            //�¼����³���
                            isTowardUp = false;
                            RealDirection[1] = false;
                            RealDirection[3] = true;
                        }
                    }
                }
            }

            for (int i = 0; i < 4; ++i)
            {

                LastDirectionState[i] = CurrentDirectionState[i];

            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerator Born()
        {

            animator.SetBool("Dead", false);

            isControlable = false;

            BornPoint point = CurrentRoom.GetBornPoint();

            transform.position = point.GetBornPosition();
            transform.localScale = point.GetBornScale();

            animator.SetBool("Born", true);

            CurrentRoom.EnterRoomInit();

            effect.OnBorn();

            yield return new WaitForSeconds(24 / 60f);

            GameSceneManager.instance.cutScene.ToggleChange(false);

            yield return new WaitForSeconds(24 / 60f);

            Init();

            animator.SetBool("Born", false);

            isControlable = true;

        }
        /// <summary>
        /// 
        /// </summary>
        private void ClearAnimatorBool()
        {

            animator.SetBool("Run", false);
            animator.SetBool("Jump", false);
            animator.SetBool("Fall", false);
            animator.SetBool("OnWall", false);
            animator.SetBool("OnWallLook", false);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerator Dead()
        {

            dashTrail.TriggleOff();

            DeathNum += 1;

            transform.SetParent(null);

            isControlable = false;

            ClearAnimatorBool();

            animator.speed = 1;

            animator.SetBool("Dead", true);

            follow.OnPlayerDead(this);

            effect.OnDeath();

            yield return new WaitForSeconds(24 / 60f);

            GameSceneManager.instance.cutScene.ToggleChange(true);

            yield return new WaitForSeconds(24 / 60f);

            animator.SetBool("Dead", false);

            StartCoroutine(Born());

        }

        private RaycastHit2D[] BesideWallCheckResult;
        private (Vector2 origin, Vector2 direction)[] BesideWallCheckRays;
         
        /// <summary>
        /// ����Ƿ���ǽ
        /// </summary>
        private void CheckWallBesides()
        {

            if (BesideWallCheckRays == null)
            {

                BesideWallCheckRays = new (Vector2 origin, Vector2 direction)[]
                {
                (new Vector2(bodyCollider.bounds.min.x, bodyCollider.bounds.max.y - 8),Vector2.left),
                (new Vector2(bodyCollider.bounds.max.x, bodyCollider.bounds.max.y - 8),Vector2.right),
                (new Vector2(bodyCollider.bounds.min.x, bodyCollider.bounds.min.y + 8),Vector2.left),
                (new Vector2(bodyCollider.bounds.max.x, bodyCollider.bounds.min.y + 8),Vector2.right),
                };

            }
            else
            {

                BesideWallCheckRays[0].origin.x = bodyCollider.bounds.min.x;
                BesideWallCheckRays[0].origin.y = bodyCollider.bounds.max.y - 8;

                BesideWallCheckRays[1].origin.x = bodyCollider.bounds.max.x;
                BesideWallCheckRays[1].origin.y = bodyCollider.bounds.max.y - 8;

                BesideWallCheckRays[2].origin.x = bodyCollider.bounds.min.x;
                BesideWallCheckRays[2].origin.y = bodyCollider.bounds.min.y + 8;

                BesideWallCheckRays[3].origin.x = bodyCollider.bounds.max.x;
                BesideWallCheckRays[3].origin.y = bodyCollider.bounds.min.y + 8;

            }


            BesideWallCheckResult = new RaycastHit2D[4];

            for (int i = 0; i < 4; ++i)
            {

                BesideWallCheckResult[i] = Physics2D.Raycast(
                    BesideWallCheckRays[i].origin,
                    BesideWallCheckRays[i].direction,
                    4,
                    ~((1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Room")) | (1 << LayerMask.NameToLayer("MapItem_BackGround")))
                );

                //if (BesideWallCheckResult[i].collider != null)
                //{
                //
                //    Debug.DrawLine(rays[i].origin, BesideWallCheckResult[i].point, Color.red);
                //
                //}

            }

            CurrentWallInfo.LeftTop = BesideWallCheckResult[0].collider != null ? BesideWallCheckResult[0].collider.gameObject : null;
            CurrentWallInfo.RightTop = BesideWallCheckResult[1].collider != null ? BesideWallCheckResult[1].collider.gameObject : null;
            CurrentWallInfo.LeftBottom = BesideWallCheckResult[2].collider != null ? BesideWallCheckResult[2].collider.gameObject : null;
            CurrentWallInfo.RightBottom = BesideWallCheckResult[3].collider != null ? BesideWallCheckResult[3].collider.gameObject : null;

        }

        private RaycastHit2D[] OnGroundCheckResult;

        private (Vector2 origin, Vector2 direction)[] OnGroundCheckRays;

        private float LastEatStrawberryTime;

        /// <summary>
        /// ����Ƿ��ڵ���
        /// </summary>
        private void CheckOnGround()
        {

            if (OnGroundCheckRays == null)
            {

                OnGroundCheckRays = new (Vector2 origin, Vector2 direction)[]
                {
                    (new Vector2(),Vector2.down),
                    (new Vector2(),Vector2.down),
                    (new Vector2(),Vector2.down)
                };

                OnGroundCheckResult = new RaycastHit2D[3];


            }

            OnGroundCheckRays[0].origin.x = Mathf.Lerp(bodyCollider.bounds.min.x, bodyCollider.bounds.max.x, 0.0f);
            OnGroundCheckRays[1].origin.x = Mathf.Lerp(bodyCollider.bounds.min.x, bodyCollider.bounds.max.x, 0.5f);
            OnGroundCheckRays[2].origin.x = Mathf.Lerp(bodyCollider.bounds.min.x, bodyCollider.bounds.max.x, 1.0f);
            OnGroundCheckRays[0].origin.y = bodyCollider.bounds.min.y;
            OnGroundCheckRays[1].origin.y = bodyCollider.bounds.min.y;
            OnGroundCheckRays[2].origin.y = bodyCollider.bounds.min.y;

            CurrentGround = null;

            for (int i = 0; i < OnGroundCheckRays.Length; ++i)
            {

                OnGroundCheckResult[i] = Physics2D.Raycast(
                    OnGroundCheckRays[i].origin,
                    OnGroundCheckRays[i].direction,
                    4,
                    ~((1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Room")) | (1 << LayerMask.NameToLayer("Ci")) | (1 << LayerMask.NameToLayer("MapItem_BackGround")))
                );

                if (CurrentGround == null && OnGroundCheckResult[i].collider != null)
                {

                    CurrentGround = OnGroundCheckResult[i].collider.gameObject;

                }

#if UNITY_EDITOR

                UnityEngine.Debug.DrawLine(OnGroundCheckRays[i].origin, OnGroundCheckRays[i].origin + OnGroundCheckRays[i].direction * 4, Color.red);

#endif

            }

            if (CurrentGround != null)
            {

                IsOnGround = true;

                WolfTime = 0;

            }
            else
            {

                IsOnGround = false;

                if (jumpState == JumpState.Jump)
                {

                    WolfTime = 1000;

                }

            }

            if (LastGround == null)
            {

                if (CurrentGround != null)
                {

                    CurrentGround.GetComponent<IStepOnEvent>()?.OnStepOn(this);

                }

            }
            else
            {

                if (CurrentGround == null)
                {

                    LastGround.GetComponent<IStepOnEvent>()?.OnStepLeave(this);

                }
                else if (LastGround != CurrentGround)
                {

                    LastGround.GetComponent<IStepOnEvent>()?.OnStepLeave(this);

                    CurrentGround.GetComponent<IStepOnEvent>()?.OnStepOn(this);

                }
                else
                {

                    CurrentGround.GetComponent<IStepOnEvent>()?.OnStepStay(this);

                }

            }

        }
        /// <summary>
        /// ��������
        /// </summary>
        private void AnimationProcess()
        {

            Vector2 scale = new Vector2(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y));

            if (IsOnGround)
            {
                jumpState = JumpState.Ignore;
            }
            else if (veiocity.y < 0)
            {

                jumpState = JumpState.Fall;

            }
            else if (veiocity.y > 0)
            {

                jumpState = JumpState.Jump;

            }

            if (isTowardLeft)
            {

                transform.localScale = new Vector3(-scale.x, scale.y);

            }
            else
            {

                transform.localScale = new Vector3(scale.x, scale.y);

            }

            animator.SetBool("Run", false);
            animator.SetBool("Jump", false);
            animator.SetBool("Fall", false);
            animator.SetBool("OnWall", false);
            animator.SetBool("OnWallLook", false);

            animator.speed = 1;

            if (CurrentCatchState != CatchState.Ignore)
            {

                if (climbState == ClimbState.Up)
                {

                    animator.speed = 1;

                    animator.SetBool("OnWall", true);

                    transform.localScale = new Vector3((CurrentCatchState == CatchState.Left ? -1 : 1) * scale.x, scale.y);

                }
                else if (climbState == ClimbState.Down)
                {

                    animator.StartPlayback();

                    animator.speed = -1;

                    animator.SetBool("OnWall", true);

                    transform.localScale = new Vector3((CurrentCatchState == CatchState.Left ? -1 : 1) * scale.x, scale.y);

                }
                else
                {

                    animator.speed = 0;

                    if (CurrentCatchState == CatchState.Left)
                    {

                        if (RealDirection[2] == true)
                        {

                            animator.SetBool("OnWallLook", true);

                            transform.localScale = new Vector3(scale.x, scale.y);

                        }
                        else
                        {

                            animator.SetBool("OnWall", true);

                            transform.localScale = new Vector3(-scale.x, scale.y);

                        }

                    }
                    else if (CurrentCatchState == CatchState.Right)
                    {

                        if (RealDirection[0] == true)
                        {

                            animator.SetBool("OnWallLook", true);

                            transform.localScale = new Vector3(-scale.x, scale.y);

                        }
                        else
                        {

                            animator.SetBool("OnWall", true);

                            transform.localScale = new Vector3(scale.x, scale.y);

                        }

                    }

                }

            }
            else if (jumpState == JumpState.Jump)
            {

                animator.SetBool("Jump", true);

            }
            else if (jumpState == JumpState.Fall)
            {

                animator.SetBool("Fall", true);

            }
            else if (moveState != MoveState.Ignore)
            {

                animator.SetBool("Run", true);

            }

        }
        [SerializeField] private GameObject LastGround;
        [SerializeField] private GameObject CurrentGround;
        /// <summary>
        /// ���˺����ڲ�
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerStay2D(Collider2D collision)
        {

            if (collision.gameObject.layer == LayerMask.NameToLayer("Room"))
            {

                var newRoom = collision.transform.GetComponent<Room>();

                if (newRoom != null && newRoom != CurrentRoom)
                {

                    CurrentRoom = newRoom;

                    cameraControl.EnterRoom(CurrentRoom);

                }

            }
            else if (collision.gameObject.layer != LayerMask.NameToLayer("MapItem_BackGround"))
            {

                if (collision.gameObject.layer == LayerMask.NameToLayer("Ci"))
                {

                    bool flag = true;

                    if (collision.transform.localEulerAngles.z == 0)
                    {

                        if (rb.velocity.y > 0)
                        {

                            flag = false;

                        }

                    }
                    else if (collision.transform.localEulerAngles.z == 90)
                    {

                        if (rb.velocity.x < 0)
                        {

                            flag = false;

                        }

                    }
                    else if (collision.transform.localEulerAngles.z == 180)
                    {

                        if (rb.velocity.y < 0)
                        {

                            flag = false;

                        }

                    }
                    else if (collision.transform.localEulerAngles.z == -90)
                    {

                        if (rb.velocity.x > 0)
                        {

                            flag = false;

                        }

                    }

                    if (flag && IsControlable)
                    {

                        TryDead();

                    }

                }
                if (collision.gameObject.layer == LayerMask.NameToLayer("DeadSpace") && IsControlable)
                {

                    TryDead();

                }

            }

        }
        /// <summary>
        /// ������ײ��
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter2D(Collision2D collision)
        {

            if (isControlable)
            {

                //LastPoint = collision.contacts;

                //��Ե���

                //var direction = collision.GetContact(0).normal;

                //const float FitValue = 8f;

                //if (direction.x != 0)
                //{
                //    //ˮƽ

                //    float PTop = bodyCollider.bounds.max.y;
                //    float PBottom = bodyCollider.bounds.min.y;
                //    float OTop = collision.collider.bounds.max.y;
                //    float OBottom = collision.collider.bounds.min.y;

                //    if (OTop - PBottom <= FitValue)
                //    {

                //        rb.position = new Vector2
                //        {

                //            x = rb.position.x,
                //            y = OTop + bodyCollider.size.y / 2

                //        };

                //    }
                //    else if (PTop - OBottom <= FitValue)
                //    {

                //        rb.position = new Vector2
                //        {

                //            x = rb.position.x,
                //            y = OBottom - bodyCollider.size.y / 2

                //        };

                //    }

                //}
                //if (direction.y != 0)
                //{
                //    //��ֱ

                //    float PLeft = bodyCollider.bounds.min.x;
                //    float PRIght = bodyCollider.bounds.max.x;
                //    float OLeft = collision.collider.bounds.min.x;
                //    float ORight = collision.collider.bounds.max.x;

                //    if (ORight - PLeft <= FitValue)
                //    {

                //        rb.position = new Vector2
                //        {
                //            x = ORight + (bodyCollider.size.x / 2),
                //            y = rb.position.y

                //        };

                //    }
                //    else if (PRIght - OLeft <= FitValue)
                //    {

                //        rb.position = new Vector2
                //        {
                //            x = OLeft - (bodyCollider.size.x / 2),
                //            y = rb.position.y

                //        };

                //    }

                //}

            }

        }
        /// <summary>
        /// �뿪��ײ��
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionExit2D(Collision2D collision)
        {

        }

        private void CheckInput()
        {

            CheckWallBesides();

            CheckOnGround();

            DirectionProcess();

            CurrentJumpState = Input.GetKey(Metric.Settings.KeyBoard.Game_Jump);

            CurrentCatchKeyState = Input.GetKey(Metric.Settings.KeyBoard.Game_Catch);

            CurrentDashState = Input.GetKey(Metric.Settings.KeyBoard.Game_Dash);

            StunButtonState.Current = RealDirection[3] == true;

        }

        private void UpdateInput()
        {

            LastWallInfo = CurrentWallInfo;

            LastJumpState = CurrentJumpState;

            LastCatchKeyState = CurrentCatchKeyState;

            LastDashState = CurrentDashState;

            LastCatchState = CurrentCatchState;

            LastGround = CurrentGround;

            StunButtonState.Past = StunButtonState.Current;

        }

        private void UpdateVelocity()
        {

            if (dashState == DashState.Ignore)
            {

                veiocity = rb.velocity;

                if (moveState == MoveState.Ignore)
                {

                    if (veiocity.x > 0)
                    {
                        if (veiocity.x - PlayerPhysical.Ground.NormalStopAcceleration < 0)
                        {
                            acceleration.x = -veiocity.x;
                        }
                        else
                        {
                            acceleration.x = -PlayerPhysical.Ground.NormalStopAcceleration;
                        }
                    }
                    else if (veiocity.x < 0)
                    {
                        if (veiocity.x + PlayerPhysical.Ground.NormalStopAcceleration > 0)
                        {
                            acceleration.x = -veiocity.x;
                        }
                        else
                        {
                            acceleration.x = PlayerPhysical.Ground.NormalStopAcceleration;
                        }
                    }
                    else
                    {
                        acceleration.x = 0;
                    }

                }

                if (CurrentCatchState != CatchState.Ignore)
                {

                    if (climbState == ClimbState.Ignore)
                    {

                        if (veiocity.y > PlayerPhysical.Catch.ClimbStopAcceleration)
                        {

                            acceleration.y = -PlayerPhysical.Catch.ClimbStopAcceleration;

                        }
                        else if (veiocity.y < -PlayerPhysical.Catch.ClimbStopAcceleration)
                        {

                            acceleration.y = PlayerPhysical.Catch.ClimbStopAcceleration;

                        }
                        else
                        {

                            veiocity.y = 0;

                            acceleration.y = 0;

                        }

                    }

                }

                veiocity += acceleration;

                if (SpecialStagnantCondition && veiocity.x >= PlayerPhysical.Stagnant.SpecialDropRegionLeft && veiocity.x <= PlayerPhysical.Stagnant.SpecialDropRegionRight)
                {
                    SpecialStagnantCondition = false;
                }

                rb.velocity = veiocity;

            }

        }

        private void UpdateAcceleration()
        {

            DashTime += 1;

            if (
                   LastDashState == false
                && CurrentDashState == true
                && LastDashNum > 0
                && DashTime > PlayerPhysical.Dash.NextDashFrameNum
            )
            {

                DashTime = 0;

                LastDashNum -= 1;

                CurrentCatchState = CatchState.Ignore;

                CurrentRoom.OnRoomDashStart();

                if (RealDirection[0] == true)
                {

                    if (RealDirection[1] == true)
                    {

                        dashState = DashState.LeftUp;

                    }
                    else if (RealDirection[3] == true)
                    {

                        dashState = DashState.LeftBottom;

                    }
                    else
                    {

                        dashState = DashState.Left;

                    }

                    effect.OnDash(true);

                }
                else if (RealDirection[2] == true)
                {

                    if (RealDirection[1] == true)
                    {

                        dashState = DashState.RightUp;

                    }
                    else if (RealDirection[3] == true)
                    {

                        dashState = DashState.RightBottom;

                    }
                    else
                    {

                        dashState = DashState.Right;

                    }

                    effect.OnDash(false);

                }
                else
                {

                    if (RealDirection[1] == true)
                    {

                        dashState = DashState.Up;

                    }
                    else if (RealDirection[3] == true)
                    {

                        dashState = DashState.Bottom;

                    }
                    else
                    {

                        dashState = isTowardLeft ? DashState.Left : DashState.Right;

                    }

                    effect.OnDash(isTowardLeft);

                }

            }

            if (dashState != DashState.Ignore)
            {

                if (DashTime % 4 == 0)
                {

                    Ghost newGhost = GhostPool.instance.GetInstance();

                    newGhost.transform.position = transform.position;
                    newGhost.transform.localScale = transform.localScale;

                    newGhost.SetGhost(sprite.sprite);

                }

                if (DashTime < PlayerPhysical.Dash.DashStartStopFrameNum)
                {

                    if (RealDirection[0] == true)
                    {

                        if (RealDirection[1] == true)
                        {

                            dashState = DashState.LeftUp;

                        }
                        else if (RealDirection[3] == true)
                        {

                            dashState = DashState.LeftBottom;

                        }
                        else
                        {

                            dashState = DashState.Left;

                        }

                    }
                    else if (RealDirection[2] == true)
                    {

                        if (RealDirection[1] == true)
                        {

                            dashState = DashState.RightUp;

                        }
                        else if (RealDirection[3] == true)
                        {

                            dashState = DashState.RightBottom;

                        }
                        else
                        {

                            dashState = DashState.Right;

                        }

                    }
                    else
                    {

                        if (RealDirection[1] == true)
                        {

                            dashState = DashState.Up;

                        }
                        else if (RealDirection[3] == true)
                        {

                            dashState = DashState.Bottom;

                        }
                        else
                        {

                            dashState = isTowardLeft ? DashState.Left : DashState.Right;

                        }

                    }

                    rb.velocity = Vector2.zero;

                    acceleration = Vector2.zero;

                }
                else
                {

                    //��̿�ʼ�󴥵ؿ�����Ծ
                    //��Ծ����˳����״̬����Ծ�ӹ�
                    if (IsOnGround && LastJumpState == false && CurrentJumpState == true)
                    {

                        if (dashState == DashState.LeftBottom)
                        {

                            rb.velocity = new Vector2(
                                rb.velocity.x * PlayerPhysical.Dash.DashBumpGroundHorizentalSpeedScale,
                                PlayerPhysical.Jump.NormalJumpSpeed
                            );

                        }
                        else if (dashState == DashState.RightBottom)
                        {

                            rb.velocity = new Vector2(
                                rb.velocity.x * PlayerPhysical.Dash.DashBumpGroundHorizentalSpeedScale,
                                PlayerPhysical.Jump.NormalJumpSpeed
                            );

                        }
                        else
                        {

                            rb.velocity = new Vector2(
                                rb.velocity.x,
                                PlayerPhysical.Jump.NormalJumpSpeed
                            );

                        }

                        dashState = DashState.Ignore;

                        jumpState = JumpState.Jump;

                    }
                    else if (DashTime < PlayerPhysical.Dash.DashContinueFramNum)
                    {

                        rb.velocity = PlayerPhysical.Dash.CalculateDashSpeed(rb.velocity, (int)dashState * 45 - 90);

                        acceleration = Vector2.zero;

                    }
                    else
                    {//�˳����״̬

                        rb.velocity = PlayerPhysical.Dash.CalculateDashEndSpeed(rb.velocity, (int)dashState * 45 - 90);

                        dashState = DashState.Ignore;

                    }
                }

            }

            if (DashTime > PlayerPhysical.Dash.DashContinueFramNum)
            {

                if (IsOnGround)
                {

                    LastDashNum = PlayerPhysical.Dash.MaxDashNum;

                    if (veiocity.x > PlayerPhysical.Ground.MaxHorizentalSpeed)
                    {
                        //�ٶȴ�����������ٶ�
                        if (RealDirection[3] == true)
                        {
                            //��
                            acceleration.x = -PlayerPhysical.Ground.SquatAcceleration;

                        }
                        else if (RealDirection[0] == true)
                        {
                            //����
                            acceleration.x = -PlayerPhysical.Ground.DiffDirectionLimitAcceleration;

                        }
                        else if (RealDirection[2] == true)
                        {
                            //����
                            acceleration.x = -PlayerPhysical.Ground.SameDirectionLimitAcceleration;

                        }
                        else
                        {

                            acceleration.x = -PlayerPhysical.Ground.NormalStopAcceleration;
                        }

                        moveState = MoveState.Right;

                    }
                    else if (veiocity.x < -PlayerPhysical.Ground.MaxHorizentalSpeed)
                    {

                        if (RealDirection[3] == true)
                        {

                            acceleration.x = PlayerPhysical.Ground.SquatAcceleration;

                        }
                        else if (RealDirection[0] == true)
                        {

                            acceleration.x = PlayerPhysical.Ground.SameDirectionLimitAcceleration;

                        }
                        else if (RealDirection[2] == true)
                        {

                            acceleration.x = PlayerPhysical.Ground.DiffDirectionLimitAcceleration;

                        }
                        else
                        {

                            acceleration.x = PlayerPhysical.Ground.NormalStopAcceleration;
                        }

                        moveState = MoveState.Left;

                    }
                    else
                    {

                        if (RealDirection[0] == true)
                        {
                            //����
                            acceleration.x = -PlayerPhysical.Ground.NormalRunAcceleration;

                            moveState = MoveState.Left;

                        }
                        else if (RealDirection[2] == true)
                        {
                            //����
                            acceleration.x = PlayerPhysical.Ground.NormalRunAcceleration;

                            moveState = MoveState.Right;

                        }
                        else
                        {
                            //ֹͣ
                            acceleration.x = (veiocity.x > 0 ? -1 : 1) * PlayerPhysical.Ground.NormalStopAcceleration;

                            moveState = MoveState.Ignore;

                        }

                    }

                    acceleration.y = 0;

                }
                else
                {

                    WolfTime += 1;

                    //ˮƽ�ٶȴ���
                    if (veiocity.x > PlayerPhysical.Stagnant.MaxHorizentalSpeed)
                    {
                        //�ٶȴ�����������ٶ�

                        if (SpecialStagnantCondition == true)
                        {

                            acceleration.x = -PlayerPhysical.Stagnant.NormalHorizentalAcceleration;

                        }
                        if (RealDirection[0] == true)
                        {
                            //����
                            acceleration.x = -PlayerPhysical.Stagnant.DiffDirectionLimitAcceleration;

                            moveState = MoveState.Left;

                        }
                        else if (RealDirection[2] == true)
                        {
                            //����
                            acceleration.x = -PlayerPhysical.Stagnant.SameDirectionLimitAcceleration;

                            moveState = MoveState.Right;

                        }
                        else
                        {

                            moveState = MoveState.Ignore;

                        }

                    }
                    else if (veiocity.x < -PlayerPhysical.Stagnant.MaxHorizentalSpeed)
                    {

                        if (SpecialStagnantCondition == true)
                        {

                            acceleration.x = PlayerPhysical.Stagnant.NormalHorizentalAcceleration;

                        }
                        if (RealDirection[0] == true)
                        {

                            acceleration.x = PlayerPhysical.Stagnant.SameDirectionLimitAcceleration;

                            moveState = MoveState.Left;

                        }
                        else if (RealDirection[2] == true)
                        {

                            acceleration.x = PlayerPhysical.Stagnant.DiffDirectionLimitAcceleration;

                            moveState = MoveState.Right;

                        }
                        else
                        {

                            moveState = MoveState.Ignore;

                        }

                    }
                    else
                    {

                        if (RealDirection[0] == true)
                        {
                            //����
                            acceleration.x = -PlayerPhysical.Stagnant.NormalHorizentalAcceleration;

                            moveState = MoveState.Left;

                        }
                        else if (RealDirection[2] == true)
                        {
                            //����
                            acceleration.x = PlayerPhysical.Stagnant.NormalHorizentalAcceleration;

                            moveState = MoveState.Right;

                        }
                        else
                        {
                            //ֹͣ
                            moveState = MoveState.Ignore;

                        }

                    }

                    //��ֱ�ٶȴ���
                    if (PlayerPhysical.Stagnant.SpecialDropRegionLeft <= veiocity.y && veiocity.y <= PlayerPhysical.Stagnant.SpecialDropRegionRight && CurrentJumpState == true)
                    {

                        acceleration.y = -PlayerPhysical.Stagnant.SpecialDropAcceleration;

                    }
                    else if (veiocity.y < -PlayerPhysical.Stagnant.MaxDropSpeedWithDown)
                    {

                        acceleration.y = PlayerPhysical.Stagnant.VerticalSpeedLimitAcceleration;

                    }
                    else if (veiocity.y < -PlayerPhysical.Stagnant.MaxDropSpeedWithOutDown)
                    {

                        if (RealDirection[3] == true)
                        {

                            acceleration.y = -PlayerPhysical.Stagnant.VerticalSpeedOutOfNormalMaxSpeedAccelerationWithDown;

                        }
                        else
                        {

                            acceleration.y = PlayerPhysical.Stagnant.VerticalSpeedLimitAcceleration;

                        }

                    }
                    else
                    {

                        acceleration.y = -PlayerPhysical.Stagnant.NormalDropAcceleration;

                    }

                }

                if (CurrentCatchKeyState == true)
                {

                    if (CurrentCatchState == CatchState.Ignore)
                    {

                        if (isTowardLeft == true && CurrentWallInfo.IsBesideLeft == true)
                        {

                            CurrentCatchState = CatchState.Left;

                        }
                        else if (isTowardLeft == false && CurrentWallInfo.IsBesideRight == true)
                        {

                            CurrentCatchState = CatchState.Right;

                        }

                    }
                    else if (CurrentCatchState == CatchState.Left)
                    {

                        if (CurrentWallInfo.IsBesideLeft == false)
                        {

                            CurrentCatchState = CatchState.Ignore;

                        }

                    }
                    else if (CurrentCatchState == CatchState.Right)
                    {

                        if (CurrentWallInfo.IsBesideRight == false)
                        {

                            CurrentCatchState = CatchState.Ignore;

                        }

                    }

                }
                else
                {

                    CurrentCatchState = CatchState.Ignore;

                }

                if (CurrentCatchState != CatchState.Ignore)
                {

                    veiocity.x = 0;

                    acceleration.x = 0;

                    if (RealDirection[1] == true)
                    {

                        if (veiocity.y > PlayerPhysical.Catch.MaxClimbUpSpeed)
                        {

                            acceleration.y = -PlayerPhysical.Catch.ClimbUpLimitAcceleration;

                        }
                        else
                        {

                            acceleration.y = PlayerPhysical.Catch.ClimbUpAcceleration;

                        }

                        climbState = ClimbState.Up;

                    }
                    else if (RealDirection[3] == true)
                    {

                        if (veiocity.y < -PlayerPhysical.Catch.MaxClimbDownSpeed)
                        {

                            acceleration.y = PlayerPhysical.Catch.ClimbDownLimitAcceleration;

                        }
                        else
                        {

                            acceleration.y = -PlayerPhysical.Catch.ClimbDownAcceleration;

                        }

                        climbState = ClimbState.Down;

                    }
                    else
                    {

                        climbState = ClimbState.Ignore;

                    }

                }

                if (CurrentJumpState == true && LastJumpState == false)
                {

                    if (CurrentCatchState != CatchState.Ignore)
                    {

                        CurrentCatchState = CatchState.Ignore;

                        if (RealDirection[0] == true)
                        {

                            rb.velocity = new Vector2(-PlayerPhysical.Jump.JumpWithWallVeiocity.x, PlayerPhysical.Jump.JumpWithWallVeiocity.y);

                        }
                        else if (RealDirection[2] == true)
                        {

                            rb.velocity = new Vector2(PlayerPhysical.Jump.JumpWithWallVeiocity.x, PlayerPhysical.Jump.JumpWithWallVeiocity.y);

                        }
                        else
                        {

                            rb.velocity = new Vector2(0, PlayerPhysical.Jump.JumpWithWallVeiocity.y);

                        }

                    }
                    else if (IsOnGround)
                    {

                        rb.velocity = new Vector2(rb.velocity.x, PlayerPhysical.Jump.NormalJumpSpeed);

                    }
                    else if (CurrentWallInfo.IsBesideLeft == true)
                    {

                        SpecialStagnantCondition = true;

                        rb.velocity = new Vector2(PlayerPhysical.Jump.JumpWithWallVeiocity.x, PlayerPhysical.Jump.JumpWithWallVeiocity.y);

                    }
                    else if (CurrentWallInfo.IsBesideRight == true)
                    {

                        SpecialStagnantCondition = true;

                        rb.velocity = new Vector2(-PlayerPhysical.Jump.JumpWithWallVeiocity.x, PlayerPhysical.Jump.JumpWithWallVeiocity.y);

                    }
                    else if (WolfTime <= 20)
                    {

                        rb.velocity = new Vector2(rb.velocity.x, PlayerPhysical.Jump.NormalJumpSpeed);

                    }

                    acceleration.y = -PlayerPhysical.Jump.NormalJumpAcceleration;

                    jumpState = JumpState.Jump;

                }

            }

        }

        #region �µĿ��Ʒ�ʽ

        private float OnGroundHorizentalMove(float oldVelocity)
        {

            float newVelocity = oldVelocity;

            //����
            if (RealDirection[0] == true)
            {

                //�ж��Ƿ񳬹����ˮƽ�ٶ�
                if (oldVelocity < -Metric.PlayerPhysical.Ground.MaxHorizentalSpeed)
                {

                    newVelocity = oldVelocity + Metric.PlayerPhysical.Ground.SameDirectionLimitAcceleration;

                    //�����ٶ�
                    if (newVelocity >= -Metric.PlayerPhysical.Ground.MaxHorizentalSpeed)
                    {

                        newVelocity = -Metric.PlayerPhysical.Ground.MaxHorizentalSpeed;

                    }

                }
                else if (oldVelocity > Metric.PlayerPhysical.Ground.MaxHorizentalSpeed)
                {

                    newVelocity = oldVelocity - Metric.PlayerPhysical.Ground.DiffDirectionLimitAcceleration;

                }
                else
                {

                    newVelocity = oldVelocity - Metric.PlayerPhysical.Ground.NormalRunAcceleration;

                    if (newVelocity < -Metric.PlayerPhysical.Ground.MaxHorizentalSpeed)
                    {

                        newVelocity = -Metric.PlayerPhysical.Ground.MaxHorizentalSpeed;

                    }

                }

            }
            else if (RealDirection[2] == true)
            {

                int i = 0;

                string s = $"123{i}123";

                //�ж��Ƿ񳬹����ˮƽ�ٶ�
                if (oldVelocity > Metric.PlayerPhysical.Ground.MaxHorizentalSpeed)
                {

                    newVelocity = oldVelocity - Metric.PlayerPhysical.Ground.SameDirectionLimitAcceleration;

                    //�����ٶ�
                    if (newVelocity <= Metric.PlayerPhysical.Ground.MaxHorizentalSpeed)
                    {

                        newVelocity = Metric.PlayerPhysical.Ground.MaxHorizentalSpeed;

                    }

                }
                else if (oldVelocity < -Metric.PlayerPhysical.Ground.MaxHorizentalSpeed)
                {

                    newVelocity = oldVelocity + Metric.PlayerPhysical.Ground.DiffDirectionLimitAcceleration;

                }
                else
                {

                    newVelocity = oldVelocity + Metric.PlayerPhysical.Ground.NormalRunAcceleration;

                    if (newVelocity > Metric.PlayerPhysical.Ground.MaxHorizentalSpeed)
                    {

                        newVelocity = Metric.PlayerPhysical.Ground.MaxHorizentalSpeed;

                    }

                }

            }
            else
            {

                if (oldVelocity > 0)
                {

                    newVelocity = oldVelocity - Metric.PlayerPhysical.Ground.NormalStopAcceleration;

                    if (newVelocity < 0)
                    {

                        newVelocity = 0;

                    }

                }
                else if (oldVelocity < 0)
                {

                    newVelocity = oldVelocity + Metric.PlayerPhysical.Ground.NormalStopAcceleration;

                    if (newVelocity > 0)
                    {

                        newVelocity = 0;

                    }

                }
                else
                {

                    newVelocity = 0;

                }

            }

            return newVelocity;

        }

        #endregion

        private void CheckCatchEvent()
        {

            ICatchEvent catchEvent;

            if (LastCatchState == CatchState.Ignore)
            {

                if (CurrentCatchState == CatchState.Ignore)
                {

                    //���·���

                }
                else if (CurrentCatchState == CatchState.Left)
                {

                    catchEvent = CurrentWallInfo.LeftObject.GetComponent<ICatchEvent>();

                    catchEvent?.OnCatchStart(this);

                }
                else
                {

                    catchEvent = CurrentWallInfo.RightObject.GetComponent<ICatchEvent>();

                    catchEvent?.OnCatchStart(this);

                }

            }
            else if (LastCatchState == CatchState.Left)
            {

                if (CurrentCatchState == CatchState.Ignore)
                {

                    catchEvent = LastWallInfo.LeftObject.GetComponent<ICatchEvent>();

                    catchEvent?.OnCatchEnd(this);

                }
                else if (CurrentCatchState == CatchState.Left)
                {

                    if (LastWallInfo.LeftObject != CurrentWallInfo.LeftObject)
                    {

                        catchEvent = LastWallInfo.LeftObject.GetComponent<ICatchEvent>();

                        catchEvent?.OnCatchEnd(this);

                        catchEvent = CurrentWallInfo.LeftObject.GetComponent<ICatchEvent>();

                        catchEvent?.OnCatchStart(this);

                    }
                    else
                    {

                        catchEvent = CurrentWallInfo.LeftObject.GetComponent<ICatchEvent>();

                        catchEvent?.OnCatchContinue(this);

                    }

                }
                else
                {

                    catchEvent = LastWallInfo.LeftObject.GetComponent<ICatchEvent>();

                    catchEvent?.OnCatchEnd(this);

                    catchEvent = CurrentWallInfo.RightObject.GetComponent<ICatchEvent>();

                    catchEvent?.OnCatchStart(this);

                }

            }
            else
            {

                if (CurrentCatchState == CatchState.Ignore)
                {

                    catchEvent = LastWallInfo.RightObject.GetComponent<ICatchEvent>();

                    catchEvent?.OnCatchEnd(this);

                }
                else if (CurrentCatchState == CatchState.Left)
                {

                    catchEvent = LastWallInfo.RightObject.GetComponent<ICatchEvent>();

                    catchEvent?.OnCatchEnd(this);

                    catchEvent = CurrentWallInfo.LeftObject.GetComponent<ICatchEvent>();

                    catchEvent?.OnCatchStart(this);

                }
                else
                {

                    if (LastWallInfo.RightObject != CurrentWallInfo.RightObject)
                    {

                        catchEvent = LastWallInfo.RightObject.GetComponent<ICatchEvent>();

                        catchEvent?.OnCatchEnd(this);

                        catchEvent = CurrentWallInfo.RightObject.GetComponent<ICatchEvent>();

                        catchEvent?.OnCatchStart(this);

                    }
                    else
                    {

                        catchEvent = CurrentWallInfo.RightObject.GetComponent<ICatchEvent>();

                        catchEvent?.OnCatchContinue(this);

                    }

                }

            }

        }
        /// <summary>
        /// �������
        /// </summary>
        private void FixedUpdate()
        {

            //if(LastPoint!= null)
            //{

            //    for (int i = 0; i < LastPoint.Length; ++i)
            //    {

            //        Debug.DrawLine(LastPoint[i].point, transform.position, i == 0 ? Color.red : Color.green);

            //    }

            //}

            if (isControlable == false)
            {

                rb.velocity = Vector2.zero;

                return;

            }

            UpdateVelocity();

            AnimationProcess();

            CheckInput();

            UpdateAcceleration();

            CheckCatchEvent();

            UpdateInput();

            if(IsOnGround == true)
            {

                LastEatStrawberryTime += Time.fixedDeltaTime;   

                if(LastEatStrawberryTime > Metric.Strawberry.EatStrawberryInterval)
                {

                    if (follow.TryEatStrawberry())
                    {

                        LastEatStrawberryTime = 0;

                    }

                }

            }
            else
            {

                LastEatStrawberryTime = 0;

            }

            if (dashState != DashState.Ignore)
            {

                dashTrail.TriggleOn();

            }
            else
            {

                dashTrail.TriggleOff();

            }

        }

        public bool TryDead()
        {

            if (isControlable)
            {

                StartCoroutine(Dead());

                return true;

            }

            return false;

        }

        public void JumpBoardFunc()
        {

            rb.velocity = JumpBoardPhysical.Speed;

        }

        public void BeFollow(Transform followItem)
        {

            follow.AddItem(followItem);

        }

        public bool TryRefreshDashNum()
        {

            if (LastDashNum < PlayerPhysical.Dash.MaxDashNum)
            {

                LastDashNum = PlayerPhysical.Dash.MaxDashNum;

                return true;

            }

            return false;

        }

        //#if UNITY_EDITOR

        //        public void OnValidate()
        //        {

        //            if (CurrentRoom != null)
        //            {

        //                transform.position = CurrentRoom.GetBornPoint().transform.position;

        //                transform.localScale = CurrentRoom.GetBornPoint().transform.localScale;

        //                cameraControl.SetRoom(CurrentRoom);

        //            }

        //        }

        //#endif

    }

}