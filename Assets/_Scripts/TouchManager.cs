using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public Camera cam;
    public LayerMask inputLayer;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPos = cam.ScreenToWorldPoint(Input.mousePosition);

            // Only hits Layer "ButtonInput"
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, Mathf.Infinity, inputLayer);

            if (hit.collider != null && hit.collider.CompareTag("Button"))
            {
                RhythmButton button = hit.collider.GetComponentInParent<RhythmButton>();
                if (button != null)
                {
                    button.OnPressed();
                }
            }
        }
    }
}