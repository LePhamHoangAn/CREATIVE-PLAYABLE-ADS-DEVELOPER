using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool enemyPhase = true;
    public RhythmButton[] playerButtons;

    public void StartPlayerPhase()
    {
        Debug.Log("Enemy phase done — player phase begins!");
        enemyPhase = false;

        foreach (var button in playerButtons)
        {
            button.EnableInput(true);
        }
    }
}
