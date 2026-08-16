using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header(" UI Setting")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Win Game Setting")]
    [SerializeField] private int winScore = 10;
    [SerializeField] private Button winButton;
    [SerializeField] private GameObject confettiPrefab;
    [SerializeField] private float panelAnimDuration = 0.5f;

    private GameObject confetti;

    private int currentScore = 0;
    private const string SCORE_KEY = "PlayerScore";

    private bool hasWon = false;

    private void Awake()
    {
        Instance = this;

        if (winButton != null)
        {
            winButton.gameObject.SetActive(false);
        }

        winButton.onClick.AddListener(OnWinClick);

        LoadScore();
    }

    private void LoadScore()
    {
        currentScore = PlayerPrefs.GetInt(SCORE_KEY, 0);
        UpdateScoreUI();
    }

    public void AddScore(int amount, Vector3 gemWorldPos)
    {
        if (hasWon) return;

        currentScore += amount;
        PlayerPrefs.SetInt(SCORE_KEY, currentScore);
        PlayerPrefs.Save();
        UpdateScoreUI();

        if (currentScore >= winScore && !hasWon)
        {
            hasWon = true;
            HandleWin(gemWorldPos);
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "GEM: " + currentScore.ToString();
        }
    }
    private void HandleWin(Vector3 spawnPosition)
    {
        if (confettiPrefab != null)
        {
            //confetti = Instantiate(confettiPrefab, spawnPosition + Vector3.up * 3, Quaternion.identity);
            confetti = ObjectPool.instance.GetObject(confettiPrefab, spawnPosition + Vector3.up * 3, Quaternion.identity);
        }
        currentScore = 0;
        PlayerPrefs.SetInt(SCORE_KEY, currentScore);
        PlayerPrefs.Save();
        StartCoroutine(ShowWinPanelCoroutine());
    }
    private IEnumerator ShowWinPanelCoroutine()
    {
        yield return new WaitForSeconds(2f);

        if (winButton != null)
        {
            winButton.gameObject.SetActive(true);
            RectTransform panelRect = winButton.GetComponent<RectTransform>();
            panelRect.localScale = Vector3.zero;

            float time = 0;

            while (time < panelAnimDuration)
            {
                time += Time.deltaTime;
                float t = time / panelAnimDuration;

                panelRect.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
                yield return null;
            }

            panelRect.localScale = Vector3.one;
        }
    }

    private void OnWinClick()
    {
        if (winButton != null)
        {
            UpdateScoreUI();
            //Destroy(confetti);
            ObjectPool.instance.ReturnObject(confetti);

            winButton.gameObject.SetActive(false);
            UIManager.Instance.OnStart();
        }
    }
}