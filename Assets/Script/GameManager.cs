using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public GameObject Player;
    public List<GameObject> cells = new List<GameObject>();
    public GameObject obstaclePrefab;
    public Transform spawnPoint;
    public int score;

    [Header("Properties")]
    public bool isPlaying = false;
    public float minObstacleSpawnTime = 0.9f;
    public float maxObstacleSpawnTime = 3;

    public float currentScore = 0;
    public float highScore;

    [Header("UI")]
    public GameObject mainMenu;
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI highScoreText;
    public GameObject resumeButton;
    public GameObject pauseButton;
    public GameObject pauseText;

    private Coroutine spawnerCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        pauseButton.SetActive(false);
        resumeButton.SetActive(false);
        pauseText.SetActive(false);
        Player.SetActive(false);
        mainMenu.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnObstacle()
    {
        while (isPlaying)
        {
            float waitTime = Random.Range(minObstacleSpawnTime, maxObstacleSpawnTime);

            yield return new WaitForSeconds(waitTime);

            Instantiate(obstaclePrefab, spawnPoint);
        }
    }

    public void ScoreUp()
    {
        currentScore++;
        currentScoreText.text = currentScore.ToString();

        maxObstacleSpawnTime = maxObstacleSpawnTime - ((maxObstacleSpawnTime - minObstacleSpawnTime) * Mathf.Min(currentScore / 100, 1));
    }

    public void GameStart()
    {
        pauseButton.SetActive(true);

        // REMOVE CELL 
        GameObject[] cells = GameObject.FindGameObjectsWithTag("BreakableCell");
        foreach (GameObject cell in cells)
        {
            Destroy(cell);
        }

        // RESET SCORE
        currentScore = 0;
        currentScoreText.text = currentScore.ToString();


        isPlaying = true;
        Player.SetActive(true);
        currentScoreText.gameObject.SetActive(true);
        mainMenu.SetActive(false);
        spawnerCoroutine = StartCoroutine(SpawnObstacle());

        SoundManager.Instance.backgroundMusicChannel.Play();
        SoundManager.Instance.mainAudioChannel.PlayOneShot(SoundManager.Instance.ButtonSound);
    }

    public void GameEnd()
    {
        pauseButton.SetActive(false);
        resumeButton.SetActive(false);

        SoundManager.Instance.mainAudioChannel.PlayOneShot(SoundManager.Instance.loseSound);

        // REMOVE REMAIN OBSTACLES
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject taggedObject in taggedObjects)
        {
            Destroy(taggedObject);
        }
        StopCoroutine(spawnerCoroutine);

        // CHECK SCORE
        if (currentScore > highScore)
        {
            highScore = currentScore;
            highScoreText.text = highScore.ToString();
        }

        // BREAK PLAYER
        Player.GetComponent<PlayerController>().BreakApart();

        isPlaying = false;
        currentScoreText.gameObject.SetActive(false);
        mainMenu.SetActive(true);

        SoundManager.Instance.backgroundMusicChannel.Stop();
    }


    public void PauseGame()
    {
        SoundManager.Instance.mainAudioChannel.PlayOneShot(SoundManager.Instance.ButtonSound);
        pauseButton.SetActive(false);
        resumeButton.SetActive(true);
        pauseText.SetActive(true);

        Time.timeScale = 0;

        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audios)
        {
            audio.Pause();
        }
    }

    public void ResumeGame()
    {
        SoundManager.Instance.mainAudioChannel.PlayOneShot(SoundManager.Instance.ButtonSound);
        pauseButton.SetActive(true);
        resumeButton.SetActive(false);
        pauseText.SetActive(false);

        Time.timeScale = 1;

        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audios)
        {
            audio.Play();
        }
    }
}
