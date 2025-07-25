using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [Header("Music")]
    public AudioSource music;

    [Header("Notes")]
    public GameObject notePrefab;
    public Transform[] lanes; // 4 spawn points

    [Header("Sprites")]
    public Sprite[] enemySprites; // Enemy note sprites per lane
    public Sprite[] playerSprites; // Player note sprites per lane

    [HideInInspector]
    public Sprite[] noteSprites; // The currently active sprites (enemy or player)

    [Header("Timing")]
    public float noteTravelTime = 2f;

    [Header("Beat Map")]
    public List<Beat> beatMap = new List<Beat>();

    [Header("Phase")]
    public float enemyPhaseEndTime = 10f; // When enemy phase ends
    public GameManager gameManager; // Link to your GameManager

    private int nextBeatIndex = 0;

    void Start()
    {
        // Start with enemy note sprites by default
        noteSprites = enemySprites;

        if (music != null)
        {
            music.Play();
        }
    }

    void Update()
    {
        if (nextBeatIndex >= beatMap.Count) return;

        float songTime = music.time;

        Beat nextBeat = beatMap[nextBeatIndex];

        // Spawn note early so it arrives at hit zone on time
        if (songTime >= nextBeat.time - noteTravelTime)
        {
            SpawnNote(nextBeat);
            nextBeatIndex++;
        }

        // If the enemy phase is over, switch to player phase
        if (gameManager != null && gameManager.enemyPhase && songTime >= enemyPhaseEndTime)
        {
            gameManager.StartPlayerPhase();
            noteSprites = playerSprites;
        }
    }

    void SpawnNote(Beat beat)
    {
        int lane = beat.lane;

        Transform spawnPoint = lanes[lane];
        GameObject noteObj = Instantiate(notePrefab, spawnPoint.position, Quaternion.identity);

        Note noteScript = noteObj.GetComponent<Note>();

        // Calculate speed so the note hits the zone exactly on beat
        float distance = spawnPoint.position.y; // If your hit zone is at Y = 0
        noteScript.speed = distance / noteTravelTime;

        noteScript.lane = lane;
        noteScript.hitTime = beat.time;

        // Set sprite for this lane based on active phase
        if (noteSprites != null && lane < noteSprites.Length)
        {
            noteScript.SetSprite(noteSprites[lane]);
        }

        // Tag the note for clarity if needed
        noteObj.tag = beat.isEnemyNote ? "EnemyNote" : "PlayerNote";

        Debug.Log($"Spawned {(beat.isEnemyNote ? "ENEMY" : "PLAYER")} note | Lane {lane}");
    }
}

[System.Serializable]
public class Beat
{
    public float time;
    public int lane;
    public bool isEnemyNote;
}
