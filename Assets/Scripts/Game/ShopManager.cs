using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("Ball Data")]
    public BallData[] allBalls;         // arrastra aquí los 6 ScriptableObjects
    public BallData currentBall;

    [Header("UI Referencias")]
    public TextMeshProUGUI shopPointsText;

    void Start()
    {
        // Cargar qué balones están desbloqueados
        foreach (var ball in allBalls)
        {
            ball.isUnlocked = SaveManager.IsBallUnlocked(ball.ballName);
        }

        // Verificar si se desbloquea el balón dorado
        CheckGoldenBallUnlock();

        // Equipar el balón de fútbol por defecto
        currentBall = allBalls[0];
        ApplyBallToGame(currentBall);
    }

    public void TryBuyBall(BallData ball)
    {
        int currentPoints = SaveManager.GetTotalPoints();

        if (ball.isUnlocked)
        {
            EquipBall(ball);
            return;
        }

        if (currentPoints < ball.cost)
        {
            Debug.Log("Puntos insuficientes");
            return;
        }

        // Descontar puntos y desbloquear
        PlayerPrefs.SetInt("totalPoints", currentPoints - ball.cost);
        PlayerPrefs.Save();
        SaveManager.UnlockBall(ball.ballName);
        ball.isUnlocked = true;

        CheckGoldenBallUnlock();
        EquipBall(ball);
    }

    void EquipBall(BallData ball)
    {
        currentBall = ball;
        ApplyBallToGame(ball);
    }

    void ApplyBallToGame(BallData ball)
    {
        // Aplicar stats al GameManager y BallController
        GameManager.Instance.pointsPerTouch = ball.pointsPerTouch;

        BallController bc = FindObjectOfType<BallController>();
        if (bc != null)
        {
            bc.bounceMultiplier = ball.bounceMultiplier;

            // Ajustar hitbox del balón
            CircleCollider2D col = bc.GetComponent<CircleCollider2D>();
            if (col != null) col.radius = ball.hitboxSize;
        }
    }

    void CheckGoldenBallUnlock()
    {
        // El balón dorado se desbloquea solo cuando tienes todos los demás
        bool allUnlocked = true;
        for (int i = 0; i < allBalls.Length - 1; i++)
        {
            if (!allBalls[i].isUnlocked) allUnlocked = false;
        }

        if (allUnlocked && allBalls.Length > 0)
        {
            BallData golden = allBalls[allBalls.Length - 1];
            golden.isUnlocked = true;
            SaveManager.UnlockBall(golden.ballName);
        }
    }

    public void UpdateShopUI()
    {
        shopPointsText.text = $"Puntos: {SaveManager.GetTotalPoints()}";
    }
}