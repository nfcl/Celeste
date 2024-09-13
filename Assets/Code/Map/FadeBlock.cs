using UnityEngine;

public class FadeBlock : MonoBehaviour
{

    [SerializeField] private Animation anime;

    [SerializeField] private SpriteRenderer sr;

    public float OriginX;

    public void RandomShake()
    {

        float angle = Random.Range(0, 2 * Mathf.PI);

        float raid = Random.Range(0, 32) * Metric.FadeBlock.ShakeStrength;

        transform.localPosition = new Vector3(
            OriginX + raid * Mathf.Cos(angle),
            0 + raid * Mathf.Sin(angle),
            0
        );

    }

    public void Format()
    {

        transform.localPosition = new Vector3(OriginX, 0, 0);

    }

    public void Fade(Vector2 scale, float alpha)
    {

        transform.localScale = scale;

        sr.color = new Color(alpha, alpha, alpha, alpha);

    }

    public void Appear(Vector2 scale, float alpha)
    {

        transform.localScale = scale;

        sr.color = new Color(alpha, alpha, alpha, alpha);

    }

}
