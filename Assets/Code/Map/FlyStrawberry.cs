using DG.Tweening;
using Map;
using n_Player.PlayerEventInterface;
using Player;
using System.Collections;
using UnityEngine;
using static GameSceneMusicManager.MapItem.Strawberry;

public class FlyStrawberry : Strawberry, IDash, IRoomEvent
{

    public bool isFly;

    private IEnumerator Fly()
    {

        animator.SetTrigger("Shake");

        GameSceneMusicManager.instance.mapItem.strawberry.Laugh();

        WaitForFixedUpdate wait = new WaitForFixedUpdate();

        float velocity = 0;

        float Adding = 0;

        while (true)
        {

            yield return wait;

            Adding += Time.fixedDeltaTime;

            if (Adding > Metric.FlyStrawberry.FlyDelayTime)
            {

                velocity += Metric.FlyStrawberry.FlyAccelarate * Time.fixedDeltaTime;

                animator.SetTrigger("Fly");

                if (Adding - Time.fixedDeltaTime < Metric.FlyStrawberry.FlyDelayTime)
                {

                    GameSceneMusicManager.instance.mapItem.strawberry.FlyAway();

                }

            }
            else
            {

                velocity += Metric.FlyStrawberry.FlyAccelarate * Time.fixedDeltaTime * 0.2f;

            }

            if (velocity > Metric.FlyStrawberry.FlyMaxSpeed)
            {

                velocity = Metric.FlyStrawberry.FlyMaxSpeed;

            }

            transform.position = new Vector3(
                transform.position.x,
                transform.position.y + velocity * Time.fixedDeltaTime,
                transform.position.z
            );

        }

    }

    private void Start()
    {

        animator.Play("Flying");

        isFollowing = false;

        OriginPos = transform.position;

        isFly = false;

    }

    public override void Follow(Follow follow)
    {

        StopAllCoroutines();

        isFly = false;

        base.Follow(follow);

    }

    public void OnDashStart()
    {

        if (isFollowing == true || isFly == true || strawberryState == StrawberryManager.State.Have)
        {

            return;

        }

        isFly = true;

        StartCoroutine(Fly());

    }

    public override void OnPlayerDead(n_Player.Player player)
    {

        animator.Play("Flying");

        base.OnPlayerDead(player);

    }

    public void EnterRoomInit()
    {

        if (strawberryState == StrawberryManager.State.None && isFollowing == false)
        {

            StopAllCoroutines();

            isFly = false;

            animator.Play("Flying");

            transform.position = OriginPos;

        }

    }
}
