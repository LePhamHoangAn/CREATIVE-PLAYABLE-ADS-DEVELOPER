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
    public float idleTimeout = 0.2f; // time in seconds to return to idle

    private SpriteRenderer spriteRenderer;
    private float lastPressTime;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetIdle();
    }

    private void Update()
    {
        // Return to idle if no press for some time
        if (Time.time - lastPressTime > idleTimeout)
        {
            SetIdle();
        }
    }

    public void SetDirection(string direction)
    {
        lastPressTime = Time.time;

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

    private void SetIdle()
    {
        spriteRenderer.sprite = idleSprite;
    }
}
