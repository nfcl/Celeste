using System;
using System.Collections;
using UnityEngine;
using n_Player.PlayerEventInterface;

public class MoveBlock : MonoBehaviour, ICatchEvent, IStepOnEvent
{
    /// <summary>
    /// ���
    /// </summary>
    [SerializeField] private MoveBlockWithTrack track;
    /// <summary>
    /// �߿�
    /// </summary>
    [SerializeField] private BlockBoard Board;
    /// <summary>
    /// ��ײ��
    /// </summary>
    [SerializeField] private BoxCollider2D Bodycollider;
    /// <summary>
    /// ���ָ���������
    /// </summary>
    [SerializeField] private Transform ChiLunContainer;
    /// <summary>
    /// ����Ŀ�͸ߵ�λ��Ϊ4����
    /// </summary>
    [SerializeField] private int Width;
    [SerializeField] private int Height;
    /// <summary>
    /// �����б�
    /// </summary>
    private ChiLun[] Chiluns;
    /// <summary>
    /// ����Ԥ����
    /// </summary>
    private static ChiLun ChiLunPrefab;
    /// <summary>
    /// ���ֶ����ٶ�
    /// </summary>
    public float speed = 20;
    /// <summary>
    /// ����֡��ֵ
    /// </summary>
    private float Add = 0f;

    public Vector2 Position
    {

        get
        {

            return transform.position;

        }
        set
        {

            transform.position = value;

        }

    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        


    }
    /// <summary>
    /// ��ʼ��
    /// </summary>
    public void InitBlock(bool OnlyBoard = false)
    {

        //����߿�

        //  ��ι�ϵ : 
        //
        //   --- ----------- ------------- ----------- ---
        //  | �� |     ��     |             |     ��     | �� |
        //   --- -----------               ----------- ---
        //  |   |           |             |           |   |
        //  |   |           |             |           |   |
        //  |   |           |             |           |   |
        //  |   |           |             |           |   |
        //  |   |           |             |           |   |
        //  | �� |            \           /            | �� |
        //  |   |             \         /             |   |
        //  |   |              \       /              |   |
        //  |   |               -------               |   |
        //  |   |                                     |   |
        //  |   |                                     |   |
        //   --- ------------------------------------- ---
        //  | �� |                  ��                  | �� |
        //   --- ------------------------------------- ---

        Board.center.localPosition = new Vector2(0, Height * 0.02f - 0.24f);

        Board.LeftTopConnect.size = new Vector2(0.02f * (Width - 16), 0.12f);
        Board.LeftTopConnect.transform.localPosition = new Vector2(-0.16f - Board.LeftTopConnect.size.x / 2f, 0.18f);

        Board.LeftTop.transform.localPosition = new Vector2(-0.08f - Board.LeftTopConnect.size.x / 2, -0.04f);

        Board.LeftConnect.size = new Vector2(0.16f, 0.04f * (Height - 9));
        Board.LeftConnect.transform.localPosition = new Vector2(0, -0.1f - Board.LeftConnect.size.y / 2);

        Board.LeftBottom.transform.localPosition = new Vector2(0, -0.08f - Board.LeftConnect.size.y / 2);

        Board.BottomConnect.size = new Vector2(0.04f * (Width - 8), 0.08f);
        Board.BottomConnect.transform.localPosition = new Vector2(0.08f + Board.BottomConnect.size.x / 2, -0.04f);

        Board.RightTopConnect.size = Board.LeftTopConnect.size;
        Board.RightTopConnect.transform.localPosition = new Vector2(-Board.LeftTopConnect.transform.localPosition.x, Board.LeftTopConnect.transform.localPosition.y);

        Board.RightTop.transform.localPosition = new Vector2(-Board.LeftTop.transform.localPosition.x, Board.LeftTop.transform.localPosition.y);

        Board.RightConnect.size = Board.LeftConnect.size;
        Board.RightConnect.transform.localPosition = new Vector2(-Board.LeftConnect.transform.localPosition.x, Board.LeftConnect.transform.localPosition.y);

        Board.RightBottom.transform.localPosition = new Vector2(-Board.LeftBottom.transform.localPosition.x, Board.LeftBottom.transform.localPosition.y);

        Bodycollider.size = new Vector2(Width * 0.04f, Height * 0.04f);

        if (OnlyBoard)
        {

            return;

        }

        //�����������

        int FixedWidth = Width - 10;
        int FixedHeight = Height - 10;

        //������Ҫ�ĸ���

        int HorNum = FixedWidth / 8 + 2;
        int VerNum = FixedHeight / 8 + 2;

        //���ɳ���

        int[] XValues = new int[HorNum];
        int[] YValues = new int[VerNum];

        //   ? - 5          X
        //----------- = --------
        // W - 5 - 5     NX - 1

        for (int x = 0; x < HorNum; ++x)
        {

            XValues[x] = (int)(x * (Width - 10f) / (HorNum - 1)) + 5;

        }

        for (int y = 0; y < VerNum; ++y)
        {

            YValues[y] = (int)(y * (Height - 10f) / (VerNum - 1)) + 5;

        }

        ChiLunContainer.transform.localPosition = new Vector2(-Width * 0.02f, -Height * 0.02f);

        Chiluns = new ChiLun[HorNum * VerNum];

        int Index;

        for (int x = 0; x < HorNum; ++x)
        {

            for (int y = 0; y < VerNum; ++y)
            {

                Index = y * HorNum + x;

                Chiluns[Index] = GameObject.Instantiate(ChiLunPrefab, ChiLunContainer);

                Chiluns[Index].transform.localPosition = new Vector2(
                    XValues[x] * 0.04f,
                    YValues[y] * 0.04f
                );

                Chiluns[Index].IsBackGround = (x % 2 == 0) ^ ((VerNum - y) % 2 == 1);

                Chiluns[Index].Init();

            }

        }

        StartCoroutine(AnimationUpdate());

    }
    /// <summary>
    /// ��������Э��
    /// </summary>
    private IEnumerator AnimationUpdate()
    {

        while (true)
        {

            yield return new WaitUntil(delegate { return speed != 0; });

            Add += speed * Time.deltaTime;

            if (speed > 0)
            {

                if (Add > 1)
                {

                    Add -= 1;

                    for (int i = 0; i < Chiluns.Length; ++i)
                    {

                        Chiluns[i].Award();

                    }

                }

            }
            else
            {

                if (Add < 0)
                {

                    Add += 1;

                    for (int i = 0; i < Chiluns.Length; ++i)
                    {

                        Chiluns[i].Back();

                    }

                }

            }

        }


    }

    private void Awake()
    {

        ChiLunPrefab = Resources.Load<ChiLun>("Map/MoveBlock/BlockChiLun/SingleChiLun");

    }

    /// <summary>
    /// ץס�¼�
    /// </summary>
    public void OnCatchStart(n_Player.Player player)
    {

        //�����ץס�ƶ���ʱ�Ὣ��ҵĸ���������Ϊ�ƶ���ʵ�ָ����˶�

        player.transform.SetParent(transform);

        track.TryMoveStart();

    }

    public void OnCatchContinue(n_Player.Player player)
    {

        player.transform.SetParent(transform);

        track.TryMoveStart();

    }

    public void OnCatchEnd(n_Player.Player player)
    {

        player.transform.SetParent(null);

        player.GetComponent<Rigidbody2D>().velocity += track.GetBlockVelocity() * Metric.MoveBlock.PlayerSpeedEffectScale;

    }

    public void OnStepOn(n_Player.Player player)
    {

        player.transform.SetParent(transform);

        track.TryMoveStart();

    }

    public void OnStepStay(n_Player.Player player)
    {

        track.TryMoveStart();

    }

    public void OnStepLeave(n_Player.Player player)
    {

        player.transform.SetParent(null);

        player.GetComponent<Rigidbody2D>().velocity += track.GetBlockVelocity() * Metric.MoveBlock.PlayerSpeedEffectScale;

    }

    /// <summary>
    /// �߿�
    /// </summary>
    [Serializable] private struct BlockBoard
    {

        public Transform center;
        public Transform LeftTop;
        public Transform RightTop;
        public Transform LeftBottom;
        public Transform RightBottom;

        public SpriteRenderer LeftTopConnect;
        public SpriteRenderer RightTopConnect;
        public SpriteRenderer LeftConnect;
        public SpriteRenderer RightConnect;
        public SpriteRenderer BottomConnect;

    }

//#if UNITY_EDITOR

//    private void OnValidate()
//    {

//        InitBlock(true);

//    }

//#endif

}
