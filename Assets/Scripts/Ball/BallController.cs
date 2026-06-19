using UnityEngine;

public class BallController : MonoBehaviour
{
[Header("Ball Settings")]
public float bounceMultiplier = 1f;
public float baseUpwardForce = 8f;
public float speedIncrement = 0.15f;

[Header("State")]
public bool isAlive = true;

private Rigidbody2D rb;
private CircleCollider2D circleCollider;
private float originalRadius;

private GameManager gameManager;

void Start()
{
    rb = GetComponent<Rigidbody2D>();

    circleCollider = GetComponent<CircleCollider2D>();

    if (circleCollider != null)
    {
        originalRadius = circleCollider.radius;
    }

    gameManager = GameManager.Instance;

    rb.gravityScale = 1f;
}

public void ApplyTouchForce(Vector2 hitDirection)
{
    if (!isAlive)
        return;

    rb.linearVelocity = Vector2.zero;

    float currentMultiplier =
        1f + gameManager.GetSpeedBonus();

    float force =
        baseUpwardForce *
        bounceMultiplier *
        currentMultiplier;

    Vector2 finalForce = new Vector2(
        hitDirection.x * 2f,
        force
    );

    rb.AddForce(finalForce, ForceMode2D.Impulse);

    gameManager.RegisterTouch();
}

public void IncreaseHitbox()
{
    if (circleCollider != null)
    {
        circleCollider.radius = originalRadius * 1.5f;
    }
}

public void RestoreHitbox()
{
    if (circleCollider != null)
    {
        circleCollider.radius = originalRadius;
    }
}

void OnCollisionEnter2D(Collision2D collision)
{
    if (!isAlive)
        return;

    if (collision.gameObject.CompareTag("Ground"))
    {
        if (gameManager.IsGravityOffActive())
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                baseUpwardForce * bounceMultiplier
            );

            return;
        }

        isAlive = false;
        gameManager.GameOver();
    }
}

public void ResetBall()
{
    isAlive = true;

    rb.linearVelocity = Vector2.zero;
    rb.angularVelocity = 0f;

    transform.position = Vector3.zero;

    RestoreHitbox();
}


}
