using UnityEngine;

namespace n_Player
{
    public class Ghost : MonoBehaviour, ObjectPool.ISetActivable
    {

        [SerializeField] private SpriteMask spriteMask;
        [SerializeField] private Animation anime;

        public void SetActive(bool active)
        {

            gameObject.SetActive(active);

        }

        public void SetGhost(Sprite sprite)
        {

            spriteMask.sprite = sprite;
            anime.Play("GhostFade");

        }

        public void AnimeEnd()
        {

            GhostPool.instance.ReturnInstance(this);

        }

    }
}