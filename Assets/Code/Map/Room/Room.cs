using System.Collections.Generic;
using UnityEngine;

public interface IRoomEvent
{

    public void EnterRoomInit();

}

public class Room : MonoBehaviour
{
    /// <summary>
    /// 房间的碰撞箱
    /// </summary>
    [SerializeField] private BoxCollider2D RoomCollider;
    /// <summary>
    /// 房间内的重生点
    /// </summary>
    [SerializeField] private List<BornPoint> BornPoints;
    /// <summary>
    /// 房间的左边界
    /// </summary>
    public float Left
    {
        get
        {

            return RoomCollider.bounds.min.x;

        }

    }
    /// <summary>
    /// 房间的右边界
    /// </summary>
    public float Right
    {

        get
        {

            return RoomCollider.bounds.max.x;

        }

    }
    /// <summary>
    /// 房间的上边界
    /// </summary>
    public float Top
    {

        get
        {

            return RoomCollider.bounds.max.y;

        }

    }
    /// <summary>
    /// 房间的下边界
    /// </summary>
    public float Bottom
    {

        get
        {

            return RoomCollider.bounds.min.y;

        }

    }
    /// <summary>
    /// 
    /// </summary>
    private IRoomEvent[] RoomObjects;

    private FlyStrawberry[] flyStrawberries;

    public void Start()
    {

        BornPoint item;

        BornPoints.Clear();

        for (int i = 0; i < transform.childCount; ++i)
        {

            if (transform.GetChild(i).TryGetComponent(out item))
            {

                BornPoints.Add(item);

            }

        }

    }

    public BornPoint GetBornPoint()
    {

        return BornPoints[0];

    }

    public void EnterRoomInit()
    {

        RoomObjects ??= GetComponentsInChildren<IRoomEvent>();

        for (int i = 0; i < RoomObjects.Length; ++i)
        {

            RoomObjects[i].EnterRoomInit();

        }

    }

    public void OnRoomDashStart()
    {

        if(flyStrawberries == null)
        {

            flyStrawberries = GetComponentsInChildren<FlyStrawberry>();

        }

        for(int i = 0; i < flyStrawberries.Length; ++i)
        {

            flyStrawberries[i].OnDashStart();

        }

    }

#if UNITY_EDITOR

    public bool ShowRoomRegion;

    public void OnDrawGizmos()
    {

        if (ShowRoomRegion)
        {

            Gizmos.color = new Color(1, 1, 1, 0.5f);

            Gizmos.DrawCube(RoomCollider.bounds.center, RoomCollider.bounds.size);

        }

    }

#endif

}
