using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Configuración del Juego")]
    public float gameDuration = 60f;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel; 
    public AudioClip gameOverSound;

    private AudioSource audioSource;
    private float remainingTime;
    private int score = 0; // Puntaje
    private bool isGameOver = false;

    void Start()
    {
        remainingTime = gameDuration;
        audioSource = GetComponent<AudioSource>();
        UpdateScoreText();
        UpdateTimerText();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (isGameOver) return;

        remainingTime -= Time.deltaTime;
        UpdateTimerText();

        if (remainingTime <= 0)
        {
            EndGame();
        }
    }

    public void HandleBalloonPopped(int points)
    {
        if (isGameOver) return;

        score += points;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }

    void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text = $"Time: {Mathf.CeilToInt(remainingTime)}";
        }
    }

    void EndGame()
    {
        isGameOver = true;

        BalloonSpawner spawner = FindFirstObjectByType<BalloonSpawner>();
        if (spawner != null)
        {
            spawner.enabled = false;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (audioSource != null && gameOverSound != null)
        {
            audioSource.PlayOneShot(gameOverSound);
        }

        DestroyAllBalloons();
    }
    void DestroyAllBalloons()
    {
        // Encuentra todas las instancias activas de Balloon y destrúyelas
        Balloon[] balloons = FindObjectsOfType<Balloon>(); // Si lo cambio por FindObjectsByType me da error?? no se el porque
        foreach (Balloon balloon in balloons)
        {
            Destroy(balloon.gameObject);
        }
    }
}
