using UnityEngine;

public class HitZone : MonoBehaviour
{
    public int laneIndex;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Note note = other.GetComponent<Note>();
        if (note != null)
        {
            note.isHittable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Note note = other.GetComponent<Note>();
        if (note != null)
        {
            note.isHittable = false;
        }
    }
}
