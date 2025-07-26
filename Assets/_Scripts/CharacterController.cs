using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite idleSprite;
    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    [Header("Settings")]
    public float idleTimeout = 0.2f;

    private SpriteRenderer spriteRenderer;
    private float lastSetTime;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        SetIdle();
    }

    private void Update()
    {
        if (Time.time - lastSetTime > idleTimeout)
        {
            SetIdle();
        }
    }

    public void SetDirection(string direction)
    {
        lastSetTime = Time.time;

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        switch (direction)
        {
            case "Up":
                spriteRenderer.sprite = upSprite;
                break;
            case "Down":
                spriteRenderer.sprite = downSprite;
                break;
            case "Left":
                spriteRenderer.sprite = leftSprite;
                break;
            case "Right":
                spriteRenderer.sprite = rightSprite;
                break;
            default:
                SetIdle();
                break;
        }
    }

    public void SetDirection(int laneIndex)
    {
        lastSetTime = Time.time;

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        switch (laneIndex)
        {
            case 0:
                spriteRenderer.sprite = leftSprite;
                break;
            case 1:
                spriteRenderer.sprite = downSprite;
                break;
            case 2:
                spriteRenderer.sprite = upSprite;
                break;
            case 3:
                spriteRenderer.sprite = rightSprite;
                break;
            default:
                SetIdle();
                break;
        }
    }

    private void SetIdle()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = idleSprite;
    }
}
