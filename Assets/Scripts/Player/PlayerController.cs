using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public float hitZoneHeight = 0.5f;    // altura desde donde el ratón golpea
    public LayerMask ballLayer;

    private Camera mainCamera;
    private BallController ball;

    void Start()
    {
        mainCamera = Camera.main;
        BallController bc = FindFirstObjectByType<BallController>();
        if (ball == null)
        {
            Debug.LogError("No se encontró ningún BallController en la escena.");
        }

        void FixedUpdate()
        {
            if (!GameManager.Instance.gameRunning) return;

            Vector2 mousePos = GetInputPosition();
            TryHitBall(mousePos);
        }

        Vector2 GetInputPosition()
        {
            // Funciona tanto en PC (ratón) como móvil (dedo)
            if (Input.touchCount > 0)
            {
                return mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
            return mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        void TryHitBall(Vector2 inputPos)
        {
            if (ball == null || !ball.isAlive) return;

            Vector2 ballPos = ball.transform.position;

            // El ratón debe estar justo debajo del balón
            float horizontalDistance = Mathf.Abs(inputPos.x - ballPos.x);
            float verticalDiff = ballPos.y - inputPos.y; // positivo = ratón debajo del balón

            bool mouseIsBelowBall = verticalDiff > 0f && verticalDiff < hitZoneHeight;
            bool mouseIsAligned = horizontalDistance < 0.6f;

            if (mouseIsBelowBall && mouseIsAligned)
            {
                // Dirección del golpe según posición horizontal del ratón
                float hitDirectionX = (inputPos.x - ballPos.x) * 0.5f;
                Vector2 hitDirection = new Vector2(hitDirectionX, 1f);

                ball.ApplyTouchForce(hitDirection);
            }
        }
    }
}