using UnityEngine;
using UnityEngine.UI;

public class RhythmButton : MonoBehaviour
{
    public int buttonIndex;
    public CharacterController character;
    public string direction; // "Up", "Down", "Left", "Right"

    [Header("Visuals")]
    [SerializeField] private ParticleSystem pressEffect;

    [Header("Hit Zone")]
    public Transform hitZone; // Assign the HitZone transform in Inspector

    [Header("Rating UI")]
    public Image ratingImage; // UI Image to display rating
    public Sprite badSprite, goodSprite, sickSprite;

    [Header("Timing Windows (seconds)")]
    public float perfectWindow = 0.05f; // 50 ms for Sick!!
    public float goodWindow = 0.15f;    // 150 ms for Good
    private bool inputEnabled = false;

    public void EnableInput(bool value)
    {
        inputEnabled = value;
    }
    private void Update()
    {
        // Optional: test with keyboard input
        if (Input.GetKeyDown(KeyCode.Alpha1) && buttonIndex == 0) OnPressed();
        if (Input.GetKeyDown(KeyCode.Alpha2) && buttonIndex == 1) OnPressed();
        if (Input.GetKeyDown(KeyCode.Alpha3) && buttonIndex == 2) OnPressed();
        if (Input.GetKeyDown(KeyCode.Alpha4) && buttonIndex == 3) OnPressed();
    }

    public void OnPressed()
    {
        if (!inputEnabled)
        {
            Debug.Log($"Input disabled for {gameObject.name}");
            return;
        }
        Debug.Log($"Button {buttonIndex} pressed!");
        PlayParticle();
        if (character != null)
        {
            character.SetDirection(direction);
        }
        Note noteToHit = FindHittableNote();
        if (noteToHit != null)
        {
            float unitDiff = Mathf.Abs(noteToHit.transform.position.y - hitZone.position.y);

            float timeDiff = unitDiff / noteToHit.speed;

            Debug.Log($"unitDiff: {unitDiff:F3} | speed: {noteToHit.speed:F2} | timeDiff: {timeDiff:F3}");

            string rating = "Bad";
            Sprite ratingSprite = badSprite;

            if (timeDiff <= perfectWindow)
            {
                rating = "Sick!!";
                ratingSprite = sickSprite;
            }
            else if (timeDiff <= goodWindow)
            {
                rating = "Good";
                ratingSprite = goodSprite;
            }

            Debug.Log($"Hit rating: {rating}");

            ShowRating(ratingSprite);

            Destroy(noteToHit.gameObject);
        }
        else
        {
            Debug.Log("Miss! No note in hit zone.");
            ShowRating(badSprite);
        }
    }

    private void PlayParticle()
    {
        if (pressEffect != null)
        {
            pressEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            pressEffect.Play();
        }
    }

    private Note FindHittableNote()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(hitZone.position, hitZone.localScale, 0f);

        Debug.Log($"[HITZONE DEBUG] Found {colliders.Length} colliders inside hit zone.");
        Debug.Log($"[HITZONE DEBUG] Found {colliders} ");

        foreach (Collider2D col in colliders)
        {
            Debug.Log($"[HITZONE DEBUG] Collider: {col.name}");

            Note note = col.GetComponent<Note>();
            if (note != null)
            {
                Debug.Log($"[HITZONE DEBUG] Found Note: {note.name}");
                return note;
            }
        }

        return null;
    }
    private void ShowRating(Sprite sprite)
    {
        if (ratingImage == null) return;

        ratingImage.sprite = sprite;
        ratingImage.gameObject.SetActive(true);

        // Hide after short time
        CancelInvoke(nameof(HideRating));
        Invoke(nameof(HideRating), 0.5f);
    }

    private void HideRating()
    {
        ratingImage.gameObject.SetActive(false);
    }

    // Debug: Draw hit zone in Scene view
    private void OnDrawGizmosSelected()
    {
        if (hitZone != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(hitZone.position, hitZone.localScale);
        }
    }
}
