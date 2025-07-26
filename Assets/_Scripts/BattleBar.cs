using UnityEngine;
using UnityEngine.UI;

public class BattleBar : MonoBehaviour
{
    [Header("Player Bar")]
    public Image playerBar;

    [Header("Portion")]
    [Range(0f, 1f)]
    public float playerPortion = 0.5f;

    [Header("UI")]
    public Button tryAgainButton;

    public GameObject RhythmButtons;

    private bool gameEnded = false;

    void Start()
    {
        if (playerBar == null)
        {
            Debug.LogError("PlayerBar not assigned!");
            return;
        }

        if (tryAgainButton != null)
        {
            tryAgainButton.gameObject.SetActive(false);
        }

        UpdateBar();
    }

    public void Adjust(float amount)
    {
        if (gameEnded) return;

        playerPortion += amount;
        playerPortion = Mathf.Clamp01(playerPortion);
        UpdateBar();
    }

    private void UpdateBar()
    {
        playerBar.fillAmount = playerPortion;
    }

    public void EvaluateBattle()
    {
        if (gameEnded) return;

        if (playerPortion >= 0.5f)
        {
            Win();
        }
        else
        {
            Lose();
        }
    }

    private void Win()
    {
        gameEnded = true;
        Debug.Log("YOU WIN!");
        RhythmButtons.SetActive(false);
    }

    private void Lose()
    {
        gameEnded = true;
        Debug.Log("YOU LOSE!");
        if (tryAgainButton != null)
        {
            tryAgainButton.gameObject.SetActive(true);
        }
        RhythmButtons.SetActive(false);

    }
}
