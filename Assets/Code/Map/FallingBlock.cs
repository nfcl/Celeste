using System.Collections;
using UnityEngine;
using n_Player.PlayerEventInterface;
using DG.Tweening;

namespace Map
{

    public class FallingBlock : MonoBehaviour, ICatchEvent, IStepOnEvent, IRoomEvent
    {

        [SerializeField] private SpriteRenderer spriteRender;

        [SerializeField] private Rigidbody2D rb;

        [SerializeField] private BoxCollider2D bodyCollider;

        private DropBlockState state;

        private Vector2 OriginPosition;

        private enum DropBlockState
        {

            Static,
            Shake,
            Drop

        }

        private void Start()
        {

            OriginPosition = transform.position;

            state = DropBlockState.Static;

            rb.constraints = RigidbodyConstraints2D.FreezeAll;

        }

        private void Drop()
        {

            rb.gravityScale = Metric.DropBlock.DropAccelerate;

            rb.constraints = ~RigidbodyConstraints2D.FreezePositionY;

        }

        private void TryShake()
        {

            if(state == DropBlockState.Static)
            {
                state = DropBlockState.Shake;

                GameSceneMusicManager.instance.mapItem.fallingBlock.OnShake();

                spriteRender.transform.DOShakePosition(Metric.FallingBlock.ShakeTime, Metric.FallingBlock.ShakeStrength, 40).OnComplete(

                    () =>
                    {

                        spriteRender.transform.localPosition = Vector2.zero;

                        state = DropBlockState.Drop;

                        Drop();

                    }

                );

            }

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {

            if (collision.contacts[0].normal.y == 1 && collision.gameObject.layer != LayerMask.NameToLayer("Ci") && collision.gameObject.layer != LayerMask.NameToLayer("Player") && collision.gameObject.layer != LayerMask.NameToLayer("AirWall"))
            {

                spriteRender.transform.DOShakePosition(Metric.FallingBlock.ShakeTime, Metric.FallingBlock.ShakeStrength, 40);

                GameSceneMusicManager.instance.mapItem.fallingBlock.OnImpact();

                Debug.Log($"{this.name}\n{collision.collider}");

            }

        }

        #region 玩家交互事件

        public void OnCatchContinue(n_Player.Player player)
        {

            TryShake();

        }

        public void OnCatchEnd(n_Player.Player player)
        {

            TryShake();

            player.transform.SetParent(null);

        }

        public void OnCatchStart(n_Player.Player player)
        {

            TryShake();

            player.transform.SetParent(transform);

        }

        public void OnStepOn(n_Player.Player player)
        {

            TryShake();

            player.transform.SetParent(transform);

        }

        public void OnStepStay(n_Player.Player player)
        {

            TryShake();

        }

        public void OnStepLeave(n_Player.Player player)
        {

            TryShake();

            player.transform.SetParent(null);

        }

        #endregion

        #region 进入房间初始化

        public void EnterRoomInit()
        {

            StopAllCoroutines();

            state = DropBlockState.Static;

            rb.constraints = RigidbodyConstraints2D.FreezeAll;

            bodyCollider.size = new Vector2(bodyCollider.size.x - 0.1f, bodyCollider.size.y);

            spriteRender.transform.localPosition = Vector2.zero;

            transform.position = OriginPosition;

            rb.gravityScale = 0;

        }

        #endregion

    }

}
