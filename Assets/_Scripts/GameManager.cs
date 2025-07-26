using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Phase")]
    public bool enemyPhase = true;
    public RhythmButton[] playerButtons;

    [Header("Refs")]
    public BattleBar battleBar;       
    public AudioSource music;         

    [Header("Game Length")]
    public float songDuration = 30f;  


    private void Start()
    {
        if (music != null && music.clip != null)
        {
            songDuration = music.clip.length;
        }
    }

    public void StartPlayerPhase()
    {
        Debug.Log("Enemy phase done — player phase begins!");
        enemyPhase = false;

        foreach (var button in playerButtons)
        {
            button.EnableInput(true);
        }
    }

    private void Update()
    {
        if (music != null && music.time >= songDuration)
        {
            EndGame();
        }
    }

    public void EndGame()
    {

        if (battleBar != null)
            battleBar.EvaluateBattle();

        Debug.Log("Game Over — Evaluation Done!");
    }
}

