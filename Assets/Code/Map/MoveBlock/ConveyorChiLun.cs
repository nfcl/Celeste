using UnityEngine;

public class ConveyorChiLun : MonoBehaviour
{

    [SerializeField] private SpriteRenderer spriteRenderer;

    private static Sprite[] ClipSprite;

    public void SetClipIndex(float value, float scale)
    {

        spriteRenderer.sprite = ClipSprite[(int)(value * scale * ClipSprite.Length / 2f) % ClipSprite.Length];

    }

    public void Awake()
    {

        if (ClipSprite == null)
        {

            ClipSprite = new Sprite[7];

            for (int i = 0; i < ClipSprite.Length; ++i)
            {

                ClipSprite[i] = Resources.Load<Sprite>($"Map/MoveBlock/ConveyChiLun/sprite_{i}");

            }

        }

    }

}
