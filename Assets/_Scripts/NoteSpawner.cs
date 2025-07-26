using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [Header("Music")]
    public AudioSource music;

    [Header("Note Prefab")]
    public GameObject notePrefab;

    [Header("Lanes")]
    public Transform[] lanes; //4 spawn points, ordered left to right

    [Header("Buttons")]
    public RhythmButton[] buttons; 

    [Header("Sprites")]
    public Sprite[] enemySprites;  
    public Sprite[] playerSprites; 

    [HideInInspector]
    public Sprite[] noteSprites;   

    [Header("Timing")]
    public float noteTravelTime = 2f;

    [Header("Beat Map")]
    public List<Beat> beatMap = new List<Beat>();

    [Header("Phase")]
    public float enemyPhaseEndTime = 10f;
    public GameManager gameManager;

    [Header("Enemy Ref")]
    public CharacterController enemyCharacter;

    [Header("Hit Zone")]
    public float hitZoneY = -3.6f;

    private int nextBeatIndex = 0;
    private bool isActive = false;

    void Start()
    {
        noteSprites = enemySprites; 
    }

    void Update()
    {
        if (!isActive) return;
        if (nextBeatIndex >= beatMap.Count) return;

        float songTime = music.time;
        Beat nextBeat = beatMap[nextBeatIndex];

        if (songTime >= nextBeat.time - noteTravelTime)
        {
            SpawnNote(nextBeat);
            nextBeatIndex++;
        }

        if (gameManager != null && gameManager.enemyPhase && songTime >= enemyPhaseEndTime)
        {
            gameManager.StartPlayerPhase();
            noteSprites = playerSprites;
        }
    }

    public void StartSpawning()
    {
        isActive = true;
        if (music != null) music.Play();
    }

    void SpawnNote(Beat beat)
    {
        int lane = beat.lane;
        if (lane >= lanes.Length)
        {
            return;
        }

        Transform spawnPoint = lanes[lane];

        GameObject noteObj = Instantiate(notePrefab, spawnPoint.position, Quaternion.identity);
        Note noteScript = noteObj.GetComponent<Note>();

        noteScript.isEnemyNote = beat.isEnemyNote;

        float distance = spawnPoint.position.y - hitZoneY;
        noteScript.speed = distance / noteTravelTime;

        noteScript.lane = lane;
        noteScript.hitTime = beat.time;
        noteScript.hitZoneY = hitZoneY;

        if (beat.isEnemyNote)
        {
            noteScript.enemyCharacter = enemyCharacter;
        }
        else
        {
            noteScript.enemyCharacter = null;
        }

        if (buttons != null && lane < buttons.Length)
        {
            noteScript.parentButton = buttons[lane];
        }
        else
        {
            //Debug.LogWarning($"[Spawner] Missing RhythmButton for lane {lane}!");
        }

        Sprite[] sourceSprites = beat.isEnemyNote ? enemySprites : playerSprites;

        if (sourceSprites != null && lane < sourceSprites.Length && sourceSprites[lane] != null)
        {
            noteScript.SetSprite(sourceSprites[lane]);
            //Debug.Log($"[Spawner] Set sprite for lane {lane}: {sourceSprites[lane].name}");
        }
        else
        {
            //Debug.LogWarning($"[Spawner] Sprite missing for lane {lane}!");
        }
        

        //Debug.Log($"Spawned {(beat.isEnemyNote ? "ENEMY" : "PLAYER")} note | Lane {lane} | HitZoneY {hitZoneY}");
    }
}

[System.Serializable]
public class Beat
{
    public float time;
    public int lane;
    public bool isEnemyNote; 
}
