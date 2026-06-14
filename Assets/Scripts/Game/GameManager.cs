using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game State")]
    public int touchCount = 0;
    public int totalPoints = 0;
    public bool gameRunning = false;

    [Header("Power-ups State")]
    private bool gravityOffActive = false;
    private float gravityOffTimer = 0f;
    private bool doublePointsActive = false;
    private float doublePointsTimer = 0f;
    private int easyItUpStored = 0;

    [Header("Current Ball")]
    public int pointsPerTouch = 1;          // depende del balón equipado

    private float speedBonus = 0f;          // acumulado de velocidad

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        // Countdown de power-ups activos
        if (gravityOffActive)
        {
            gravityOffTimer -= Time.deltaTime;
            if (gravityOffTimer <= 0f) gravityOffActive = false;
        }

        if (doublePointsActive)
        {
            doublePointsTimer -= Time.deltaTime;
            if (doublePointsTimer <= 0f) doublePointsActive = false;
        }
    }

    public void RegisterTouch()
    {
        touchCount++;

        // Calcular puntos con posible 2x
        int earned = doublePointsActive ? pointsPerTouch * 2 : pointsPerTouch;
        totalPoints += earned;

        // Cada 50 toques → velocidad +0.15x
        if (touchCount % 50 == 0)
        {
            speedBonus += 0.15f;
            UIManager.Instance.ShowSpeedUpMessage();
        }

        // Cada 150 toques → EasyIt Up gratis
        if (touchCount % 150 == 0)
        {
            easyItUpStored++;
            UIManager.Instance.ShowRewardMessage("¡EasyIt Up desbloqueado!");
        }

        // Cada 300 toques → Golden Ball x3
        if (touchCount % 300 == 0)
        {
            totalPoints += totalPoints * 2; // x3 total
            UIManager.Instance.ShowRewardMessage("¡GOLDEN BALL x3!");
        }

        UIManager.Instance.UpdateHUD(touchCount, totalPoints);
    }

    public float GetSpeedBonus() => speedBonus;
    public bool IsGravityOffActive() => gravityOffActive;

    // Power-ups
    public bool ActivateGravityOff()
    {
        if (totalPoints < 300) return false;
        totalPoints -= 300;
        gravityOffActive = true;
        gravityOffTimer = 5f;
        return true;
    }

    public bool ActivateDoublePoints()
    {
        if (totalPoints < 200) return false;
        totalPoints -= 200;
        doublePointsActive = true;
        doublePointsTimer = 10f;
        return true;
    }

    public bool ActivateEasyItUp()
    {
        // Puede venir de la tienda o del reward de 150 toques
        if (easyItUpStored > 0)
        {
            easyItUpStored--;
        }
        else if (totalPoints >= 250)
        {
            totalPoints -= 250;
        }
        else return false;

        UIManager.Instance.ActivateEasyItUp();
        return true;
    }

    public void GameOver()
    {
        gameRunning = false;
        UIManager.Instance.ShowGameOver(touchCount, totalPoints);
        SaveManager.SaveScore(totalPoints);
    }
}