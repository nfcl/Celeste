using System.Collections;
using UnityEngine;

namespace Map
{

    public class DashCrystal : MonoBehaviour, IRoomEvent
    {

        [SerializeField] private BoxCollider2D bodyCollider;

        [SerializeField] private Animator animtor;

        private static Sprite[] sprites;

        private bool isActive;

        private void Start()
        {

            isActive = true;

        }

        private void Awake()
        {

            sprites = new Sprite[6];

            for (int i = 0; i < sprites.Length; ++i)
            {

                sprites[i] = Resources.Load<Sprite>($"Map/DashCrystal/sprite_{i}");

            }

        }

        private void OnTriggerStay2D(Collider2D collision)
        {

            if (collision.gameObject.TryGetComponent<n_Player.Player>(out n_Player.Player player))
            {

                if (isActive == true)
                {

                    if (player.TryRefreshDashNum() == true)
                    {

                        Broken(player.GetComponent<Rigidbody2D>().velocity);

                    }

                }

            }

        }

        private void Broken(Vector2 velocity)
        {

            isActive = false;

            bodyCollider.enabled = false;

            GameSceneMusicManager.instance.mapItem.dashCrystal.Fade();

            animtor.SetTrigger("Broken");

            transform.localEulerAngles = new Vector3(0, 0, -180 / Mathf.PI * Mathf.Atan2(velocity.x, velocity.y));

            StartCoroutine(Refresh());

        }

        private IEnumerator Refresh()
        {

            yield return new WaitForSeconds(Metric.DashCrystal.RefreshTime);

            Appear();

        }

        private void Appear()
        {

            isActive = true;

            bodyCollider.enabled = true;

            GameSceneMusicManager.instance.mapItem.dashCrystal.Appear();

            animtor.SetTrigger("Refresh");

            transform.localEulerAngles = new Vector3(0, 0, 0);

        }

        public void EnterRoomInit()
        {

            StopAllCoroutines();

            animtor.Play("DashCrystal");

            bodyCollider.enabled = true;

            isActive = true;

            transform.localEulerAngles = new Vector3(0, 0, 0);

        }

    }

} 