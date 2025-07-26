using UnityEngine;
using UnityEngine.UI;

public class RhythmButton : MonoBehaviour
{
    public int buttonIndex;
    public CharacterController character;
    public string direction; 

    [Header("Visuals")]
    [SerializeField] private ParticleSystem pressEffect;

    [Header("Hit Zone")]
    public Transform hitZone; 

    [Header("Rating UI")]
    public Image ratingImage; 
    public Sprite badSprite, goodSprite, sickSprite;

    [Header("Timing Windows (seconds)")]
    public float perfectWindow = 0.05f; 
    public float goodWindow = 0.15f;    

    [Header("Battle")]
    public BattleBar battleBar;

    [Header("Score Settings")]
    public float badScoreDelta = -0.02f;
    public float goodScoreDelta = 0.03f;
    public float perfectScoreDelta = 0.05f;

    private bool inputEnabled = false;

    public void EnableInput(bool value)
    {
        inputEnabled = value;
    }

    private void Update()
    {
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
        string rating = "Bad";
        float scoreDelta = badScoreDelta; 

        if (noteToHit != null)
        {
            float unitDiff = Mathf.Abs(noteToHit.transform.position.y - hitZone.position.y);
            float timeDiff = unitDiff / noteToHit.speed;

            Debug.Log($"unitDiff: {unitDiff:F3} | speed: {noteToHit.speed:F2} | timeDiff: {timeDiff:F3}");

            if (timeDiff <= perfectWindow)
            {
                rating = "Sick!!";
                scoreDelta = perfectScoreDelta; 
                ShowRating(sickSprite);
            }
            else if (timeDiff <= goodWindow)
            {
                rating = "Good";
                scoreDelta = goodScoreDelta; 
                ShowRating(goodSprite);
            }
            else
            {
                ShowRating(badSprite);
            }

            Destroy(noteToHit.gameObject);
        }
        else
        {
            Debug.Log("Miss! No note in hit zone.");
            ShowRating(badSprite);
        }

        Debug.Log($"Rating: {rating} | Score Delta: {scoreDelta}");

        if (battleBar != null)
        {
            battleBar.Adjust(scoreDelta);
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

        foreach (Collider2D col in colliders)
        {
            Note note = col.GetComponent<Note>();
            if (note != null)
            {
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

        CancelInvoke(nameof(HideRating));
        Invoke(nameof(HideRating), 0.5f);
    }

    private void HideRating()
    {
        if (ratingImage != null)
            ratingImage.gameObject.SetActive(false);
    }

    public void RegisterMiss()
    {
        Debug.Log($"Miss registered by Note in button {buttonIndex}!");

        if (battleBar != null)
            battleBar.Adjust(badScoreDelta);

        ShowRating(badSprite);
    }
    private void OnDrawGizmosSelected()
    {
        if (hitZone != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(hitZone.position, hitZone.localScale);
        }
    }
}
