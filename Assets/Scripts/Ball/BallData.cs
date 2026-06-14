using UnityEngine;

[CreateAssetMenu(fileName = "BallData", menuName = "AirBallr/Ball Data")]
public class BallData : ScriptableObject
{
    public string ballName;
    public Sprite ballSprite;
    public int pointsPerTouch;
    public float bounceMultiplier;
    public float hitboxSize;
    public int cost;
    public bool isUnlocked;
    public bool isOval;         // para el balón americano
}