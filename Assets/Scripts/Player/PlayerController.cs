using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public float hitZoneHeight = 0.5f;
    public LayerMask ballLayer;

    private Camera mainCamera;
    private BallController ball;

    void Start()
    {
        mainCamera = Camera.main;

        ball = FindFirstObjectByType<BallController>();

        if (ball == null)
        {
            Debug.LogError("No se encontró ningún BallController en la escena.");
        }
    }

    void Update()
    {
        if (!GameManager.Instance.gameRunning)
            return;

        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            Vector2 inputPos = GetInputPosition();
            TryHitBall(inputPos);
        }
    }

    Vector2 GetInputPosition()
    {
        if (Input.touchCount > 0)
        {
            return mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
        }

        return mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    void TryHitBall(Vector2 inputPos)
    {
        if (ball == null || !ball.isAlive)
            return;

        Vector2 ballPos = ball.transform.position;

        float horizontalDistance = Mathf.Abs(inputPos.x - ballPos.x);
        float verticalDiff = ballPos.y - inputPos.y;

        bool mouseIsBelowBall =
            verticalDiff > 0 &&
            verticalDiff < hitZoneHeight;

        bool mouseIsAligned =
            horizontalDistance < 0.6f;

        if (mouseIsBelowBall && mouseIsAligned)
        {
            float hitDirectionX =
                (inputPos.x - ballPos.x) * 0.5f;

            Vector2 hitDirection =
                new Vector2(hitDirectionX, 1f);

            ball.ApplyTouchForce(hitDirection);
        }
    }
}