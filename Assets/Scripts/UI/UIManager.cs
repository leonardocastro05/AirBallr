using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("HUD")]
    public TextMeshProUGUI touchCountText;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI speedBonusText;

    [Header("Panels")]
    public GameObject startPanel;
    public GameObject gamePanel;
    public GameObject gameOverPanel;
    public GameObject shopPanel;
    public GameObject exitPopup;

    [Header("Game Over")]
    public TextMeshProUGUI finalTouchesText;
    public TextMeshProUGUI finalPointsText;
    public TextMeshProUGUI highScoreText;

    [Header("Power-up Feedback")]
    public GameObject easyItUpVisual;   // efecto visual cuando está activo
    public TextMeshProUGUI rewardMessage;
    public TextMeshProUGUI speedUpMessage;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // HUD durante la partida
    public void UpdateHUD(int touches, int points)
    {
        touchCountText.text = touches.ToString();
        pointsText.text = points.ToString();
    }

    public void ShowSpeedUpMessage()
    {
        speedUpMessage.text = "¡VELOCIDAD +0.15x!";
        Invoke(nameof(HideSpeedUpMessage), 2f);
    }

    void HideSpeedUpMessage() => speedUpMessage.text = "";

    public void ShowRewardMessage(string msg)
    {
        rewardMessage.text = msg;
        Invoke(nameof(HideRewardMessage), 3f);
    }

    void HideRewardMessage() => rewardMessage.text = "";

    // EasyIt Up — ampliar hitbox visualmente
    public void ActivateEasyItUp()
    {
        easyItUpVisual.SetActive(true);
        Invoke(nameof(DeactivateEasyItUp), 5f);
    }

    void DeactivateEasyItUp() => easyItUpVisual.SetActive(false);

    // Pantallas
    public void ShowStartPanel()
    {
        startPanel.SetActive(true);
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void StartGame()
    {
        startPanel.SetActive(false);
        gamePanel.SetActive(true);
        GameManager.Instance.gameRunning = true;
    }

    public void ShowGameOver(int touches, int points)
    {
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        finalTouchesText.text = $"Toques: {touches}";
        finalPointsText.text = $"Puntos: {points}";
        highScoreText.text = $"Récord: {SaveManager.GetHighScore()}";
    }

    public void OpenShop() => shopPanel.SetActive(true);
    public void CloseShop() => shopPanel.SetActive(false);

    // Popup de salida
    public void ShowExitPopup() => exitPopup.SetActive(true);

    public void ConfirmExit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void CancelExit() => exitPopup.SetActive(false);
}