using UnityEngine;

public class Note : MonoBehaviour
{
    [Header("Note Settings")]
    public float speed = 5f;
    public float hitTime;  // When this note should hit exactly
    public int lane;       // Lane index (0–3)

    [Header("Rendering")]
    public SpriteRenderer spriteRenderer;

    [HideInInspector] public bool isHittable = false;

    void Start()
    {
        // Auto-find SpriteRenderer if not linked
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        // Move downwards each frame
        transform.position += Vector3.down * speed * Time.deltaTime;

        // Destroy if far below screen
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    public void SetSprite(Sprite sprite)
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer != null && sprite != null)
        {
            spriteRenderer.sprite = sprite;
        }
    }
}
