using UnityEngine;

public class Note : MonoBehaviour
{
    [Header("Note Settings")]
    public float speed = 5f;       
    public float hitTime;          
    public int lane;               

    [Header("Rendering")]
    public SpriteRenderer spriteRenderer;

    [Header("Hit Zone")]
    public float hitZoneY = -3.6f;    

    [Header("Reference")]
    public CharacterController enemyCharacter;
    public RhythmButton parentButton;

    [Header("Note Type")]
    public bool isEnemyNote = true;  

    [Header("Hittable")]
    [HideInInspector] public bool isHittable = false;

    private bool alreadyMissed = false;

    void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;

        if (!alreadyMissed && transform.position.y <= hitZoneY)
        {
            alreadyMissed = true;

            if (enemyCharacter != null)
            {
                enemyCharacter.SetDirection(lane);
            }

            if (parentButton != null && !isEnemyNote)
            {
                parentButton.RegisterMiss();
            }

            Destroy(gameObject);
        }

        if (transform.position.y < -20f)
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
