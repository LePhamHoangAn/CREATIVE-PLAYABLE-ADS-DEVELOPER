using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Button startButton;
    public Image countdownImage; //Image element for 3, 2, 1, Go!
    public Sprite threeSprite;
    public Sprite twoSprite;
    public Sprite oneSprite;
    public Sprite goSprite;

    [Header("Audio")]
    public AudioSource sfxSource;
    public AudioClip threeClip;
    public AudioClip twoClip;
    public AudioClip oneClip;
    public AudioClip goClip;

    [Header("Game References")]
    public NoteSpawner noteSpawner;
    public RhythmButton[] buttons;

    [Header("Timing")]
    public float stepTime = 1f; //Seconds between steps

    public GameObject BattleBar;
    public GameObject RhythmButtons;
    public GameObject GameLogo;
    void Start()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartCountdown);
        }

        countdownImage.gameObject.SetActive(false);
        BattleBar.SetActive(false);
        RhythmButtons.SetActive(false);
        GameLogo.SetActive(true);

    }

    public void StartCountdown()
    {
        startButton.gameObject.SetActive(false);
        StartCoroutine(CountdownSequence());
    }

    private IEnumerator CountdownSequence()
    {
        countdownImage.gameObject.SetActive(true);
        GameLogo.SetActive(false);

        //3
        countdownImage.sprite = threeSprite;
        PlaySFX(threeClip);
        yield return new WaitForSeconds(stepTime);

        //2
        countdownImage.sprite = twoSprite;
        PlaySFX(twoClip);
        yield return new WaitForSeconds(stepTime);

        //1
        countdownImage.sprite = oneSprite;
        PlaySFX(oneClip);
        yield return new WaitForSeconds(stepTime);

        //Go
        countdownImage.sprite = goSprite;
        PlaySFX(goClip);
        yield return new WaitForSeconds(1f);

        countdownImage.gameObject.SetActive(false);
        BattleBar.SetActive(true);
        RhythmButtons.SetActive(true);
        noteSpawner.music.Play();
        noteSpawner.StartSpawning();
        EnableInputs();
    }

    private void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    private void EnableInputs()
    {
        foreach (var b in buttons)
        {
            b.EnableInput(true);
        }
    }


}
