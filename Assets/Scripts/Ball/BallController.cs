using UnityEngine;

public class BallController : MonoBehaviour
{
    [Header("Ball Settings")]
    public float bounceMultiplier = 1f;     // cada balón tendrá el suyo
    public float baseUpwardForce = 8f;      // fuerza base del toque
    public float speedIncrement = 0.15f;    // +0.15x cada 50 toques

    [Header("State")]
    public bool isAlive = true;

    private Rigidbody2D rb;
    private GameManager gameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameManager.Instance;

        // Gravedad normal hacia abajo
        rb.gravityScale = 1f;
    }

    // Llamado desde PlayerController cuando el ratón toca el balón
    public void ApplyTouchForce(Vector2 hitDirection)
    {
        if (!isAlive) return;

        // Cancelamos velocidad actual y aplicamos nuevo impulso hacia arriba
        rb.linearVelocity = Vector2.zero;

        float currentMultiplier = 1f + (gameManager.GetSpeedBonus());
        float force = baseUpwardForce * bounceMultiplier * currentMultiplier;

        // La dirección tiene componente horizontal según donde golpeaste
        Vector2 finalForce = new Vector2(hitDirection.x * 2f, force);
        rb.AddForce(finalForce, ForceMode2D.Impulse);

        gameManager.RegisterTouch();
    }

    // El balón tocó el suelo
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && isAlive)
        {
            // Verificar si Gravity Off está activo
            if (gameManager.IsGravityOffActive())
            {
                // Rebotar sin perder
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, baseUpwardForce * bounceMultiplier);
                return;
            }

            isAlive = false;
            gameManager.GameOver();
        }
    }
}