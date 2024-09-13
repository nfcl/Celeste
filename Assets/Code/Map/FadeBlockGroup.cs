using UnityEngine;
using n_Player.PlayerEventInterface;
using System.Collections;
using System;

public class FadeBlockGroup : MonoBehaviour,ICatchEvent,IStepOnEvent
{

    [SerializeField] private FadeBlock[] blocks;

    [SerializeField] private int blockNum;

    [SerializeField] private Transform container;

    [SerializeField] private BoxCollider2D bodyCollider;

    [SerializeField] private Border border;

    private static FadeBlock prefab;

    private FadeBlockState state;

    private enum FadeBlockState
    {

        None,
        Shake,
        Fade,
        Refresh,
        Appear

    }

    [Serializable]
    private class Border
    {

        public SpriteRenderer center;
        public SpriteRenderer left;
        public SpriteRenderer right;

    }

    public void Init()
    {

        blocks = new FadeBlock[blockNum];

        int X = -32 * (blockNum - 1) / 2;

        bodyCollider.size = new Vector2(32 * blockNum, 32);

        if (blockNum <= 2)
        {
            border.center.gameObject.SetActive(false);
        }
        else
        {

            border.center.size = new Vector2(0.25f * (blockNum - 2), 0.1875f);

        }

        border.left.transform.localPosition = new Vector2(-16 * (blockNum - 1), border.left.transform.localPosition.y);
        border.right.transform.localPosition = new Vector2(16 * (blockNum - 1), border.left.transform.localPosition.y);


        for (int i = 0; i < blockNum; ++i)
        {

            blocks[i] = GameObject.Instantiate(prefab, container);

            blocks[i].transform.localPosition = new Vector3(X, 0, 0);

            blocks[i].OriginX = X;

            X += 32;

        }

    }

    public void Awake()
    {

        prefab = Resources.Load<FadeBlock>("Map/MapElements/FadeBlocks/FadeBlock");

    }

    public void Start()
    {

        Init();

        state = FadeBlockState.None;

    }

    public IEnumerator Shake()
    {

        float Add = 0;

        WaitForSeconds wait = new WaitForSeconds(0.05f);

        while (true)
        {

            yield return wait;

            Add += 0.05f;

            for (int i = 0; i < blocks.Length; ++i)
            {

                blocks[i].RandomShake();

            }

            if (Add >= Metric.FadeBlock.ShakeTime)
            {

                StartCoroutine(Fade());

                yield break;

            }

        }

    }

    private IEnumerator Fade()
    {

        state = FadeBlockState.Fade;

        float Add = 0;

        float lerpValue;

        Vector2 bufScale;

        float bufAlpha;

        Vector2 StartScale = new Vector2(1, 1);
        Vector2 EndScale = new Vector2(0.6f, 0.6f);

        float StartAlpha = 1;
        float EndAlpha = 0;

        WaitForSeconds wait = new WaitForSeconds(0.05f);

        bodyCollider.enabled = false;

        while (true)
        {

            yield return wait;

            Add += 0.05f;

            if (Add > Metric.FadeBlock.FadeTime)
            {
                Add = Metric.FadeBlock.FadeTime;
            }

            lerpValue = Add / Metric.FadeBlock.FadeTime;

            bufScale = Vector2.Lerp(StartScale, EndScale, lerpValue);
            bufAlpha = Mathf.Lerp(StartAlpha, EndAlpha, lerpValue);

            for (int i = 0; i < blocks.Length; ++i)
            {

                blocks[i].Fade(bufScale, bufAlpha);

            }

            if (Add >= Metric.FadeBlock.FadeTime)
            {

                StartCoroutine(Appear());

                yield break;

            }

        }

    }

    private IEnumerator Appear()
    {

        state = FadeBlockState.Refresh;

        yield return new WaitForSeconds(Metric.FadeBlock.RefreshTime);

        state = FadeBlockState.Appear;

        float Add = 0;

        float lerpValue;

        Vector2 bufScale;

        float bufAlpha;

        Vector2 StartScale = new Vector2(0.6f, 0.6f);
        Vector2 EndScale = new Vector2(1, 1);

        float StartAlpha = 0;
        float EndAlpha = 1;

        WaitForSeconds wait = new WaitForSeconds(0.05f);

        for (int i = 0; i < blocks.Length; ++i)
        {

            blocks[i].Format();

        }

        while (true)
        {

            yield return wait;

            Add += 0.05f;

            if (Add > Metric.FadeBlock.AppearTime)
            {
                Add = Metric.FadeBlock.AppearTime;
            }

            lerpValue = Add / Metric.FadeBlock.AppearTime;

            bufScale = Vector2.Lerp(StartScale, EndScale, lerpValue);
            bufAlpha = Mathf.Lerp(StartAlpha, EndAlpha, lerpValue);

            for (int i = 0; i < blocks.Length; ++i)
            {

                blocks[i].Appear(bufScale, bufAlpha);

            }

            if (Add >= Metric.FadeBlock.AppearTime)
            {

                bodyCollider.enabled = true;

                state = FadeBlockState.None;

                yield break;

            }

        }

    }

    public void TryFade()
    {

        if(state== FadeBlockState.None)
        {

            state = FadeBlockState.Shake;

            StartCoroutine(Shake());

        }

    }

    public void OnCatchStart(n_Player.Player player)
    {

        TryFade();

    }

    public void OnCatchContinue(n_Player.Player player)
    {

        TryFade();

    }

    public void OnCatchEnd(n_Player.Player player)
    {

        TryFade();

    }

    public void OnStepOn(n_Player.Player player)
    {

        TryFade();

    }

    public void OnStepStay(n_Player.Player player)
    {



    }

    public void OnStepLeave(n_Player.Player player)
    {



    }
}