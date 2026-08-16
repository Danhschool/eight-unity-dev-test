using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private Button startButton;
    [SerializeField] private Button resetButton;

    private void Awake()
    {
        Instance = this;

        Time.timeScale = 0f;
        if (startButton != null) startButton.gameObject.SetActive(true);

        startButton.onClick.AddListener(OnStartButtonClicked);
        resetButton.onClick.AddListener(OnResetButtonClicked);
    }

    public void OnStart()
    {
        Time.timeScale = 0f;
        if (startButton != null) startButton.gameObject.SetActive(true);
    }

    public void OnStartButtonClicked()
    {
        if (startButton != null) startButton.gameObject.SetActive(false);

        Time.timeScale = 1f;
        
        CameraTour.Instance.StartTour();
    }

    public void OnResetButtonClicked()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
