using DG.Tweening;
using Player;
using UnityEngine;
using n_Player.PlayerEventInterface;
using Map;

#if UNITY_EDITOR

using UnityEditor;

[CustomEditor(typeof(Strawberry))]
public class StrawberryExtension : Editor
{

    private Strawberry m_target;

    private void OnEnable()
    {

        m_target = target as Strawberry;

    }
    /// <summary>
    /// 重写OnInspectorGUI，之后所有的GUI绘制都在此方法中。
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); //调用父类方法绘制一次GUI，TutorialMono中原本的可序列化数据等会在这里绘制一次。
        //如果不调用父类方法，则这个Mono的Inspector全权由下面代码绘制。

        if (GUILayout.Button("吃草莓"))   //自定义按钮
        {

            if (Application.isPlaying == true || UnityEditor.EditorApplication.isPlaying == true)
            {

                m_target.Eat();

            }

        }
    }

}

#endif

namespace Map
{

    public interface IFollowedable
    {

        public void BeFollow(Transform followItem);

    }

    public interface IFollowable
    {

        public void Follow(Follow follow);

    }

    public class Strawberry : MonoBehaviour, IFollowable, IDead
    {

        [SerializeField] private SpriteRenderer sr;

        [SerializeField] protected Animator animator;

        [SerializeField] protected bool isFollowing;

        [SerializeField] protected Follow follow;

        [SerializeField] protected Vector3 OriginPos;

        [SerializeField] private BoxCollider2D bodyCollider;

        public StrawberryManager.State strawberryState;

        private void Start()
        {

            animator.Play("rotation");

            isFollowing = false;

            OriginPos = transform.position;

        }

        protected void OnTriggerStay2D(Collider2D collision)
        {
            
            if(isFollowing == false)
            {

                if(collision.TryGetComponent<IFollowedable>(out var Interface))
                {

                    Touch(Interface);

                }

            }

        }

        protected void Touch(IFollowedable Interface)
        {

            Interface.BeFollow(transform);

        }

        public virtual void Follow(Follow follow)
        {

            this.follow = follow;

            animator.SetTrigger("Touch");

            isFollowing = true;

            GameSceneMusicManager.instance.mapItem.strawberry.TouchStrawberry_Red();

        }

        public void Eat()
        {

            if (follow != null)
            {

                follow.RemoveItem(this.transform);

                follow = null;

                isFollowing = false;

            }

            animator.Play("Eat");

            bodyCollider.enabled = false;

            strawberryState = StrawberryManager.State.Have;

            GameSceneMusicManager.instance.mapItem.strawberry.GetStrawberry_Red(1);

        }

        public virtual void OnPlayerDead(n_Player.Player player)
        {

            follow.RemoveItem(this.transform);

            follow = null;

            isFollowing = false;

            transform.DOMove(OriginPos, 1, true);

        }

    }

}