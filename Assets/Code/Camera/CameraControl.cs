using UnityEngine;

public class CameraControl : MonoBehaviour
{

    //1280*720

    [SerializeField] private Room CurrentRoom;

    [SerializeField] private Transform FollowItem;

    [SerializeField] private Camera thisCamera;

    private float HalfHeight
    {

        get
        {

            return thisCamera.orthographicSize;

        }

    }

    private float HalfWidth
    {

        get
        {

            return thisCamera.orthographicSize * 1920 / 1080;

        }

    }

    public float smoothTime = 0.3F;

    private Vector3 velocity = Vector3.zero;

    public void SetRoom(Room newRoom)
    {

        CurrentRoom = newRoom;

        transform.position = CalcuCameraPosition();

    }

    public void EnterRoom(Room newRoom)
    {

        CurrentRoom = newRoom;

        CurrentRoom.EnterRoomInit();

    }

    private Vector3 CalcuCameraPosition()
    {

        Vector3 Result = FollowItem.position;

        if (Result.x - HalfWidth < CurrentRoom.Left)
        {

            Result.x = CurrentRoom.Left + HalfWidth;

        }
        else if (Result.x + HalfWidth > CurrentRoom.Right)
        {

            Result.x = CurrentRoom.Right - HalfWidth;

        }

        if (Result.y - HalfHeight < CurrentRoom.Bottom)
        {

            Result.y = CurrentRoom.Bottom + HalfHeight;

        }
        else if (Result.y + HalfHeight > CurrentRoom.Top)
        {

            Result.y = CurrentRoom.Top - HalfHeight;

        }

        Result.z = transform.position.z;

        return Result;

    }

    public void Update()
    {

        //平滑地移动摄像机朝向目标位置
        transform.position = Vector3.SmoothDamp(transform.position, CalcuCameraPosition(), ref velocity, smoothTime);

    }

}
