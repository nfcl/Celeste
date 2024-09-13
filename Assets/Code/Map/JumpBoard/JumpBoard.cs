using UnityEngine;

public class JumpBoard : MonoBehaviour
{

    public Animator animator;

    public interface IJumpBoardJumpAble
    {

        public void JumpBoardFunc();

    };

    public void OnTriggerStay2D(Collider2D collision)
    {

        IJumpBoardJumpAble item = collision.GetComponent<IJumpBoardJumpAble>();

        if (item != null)
        {

            animator.SetTrigger("Start");

            GameSceneMusicManager.instance.mapItem.jumpBoard.Jump();

            item.JumpBoardFunc();

        }

    }

}
