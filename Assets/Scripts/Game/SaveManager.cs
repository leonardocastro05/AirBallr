using UnityEngine;

public static class SaveManager
{
    private const string KEY_POINTS = "totalPoints";
    private const string KEY_HIGHSCORE = "highScore";
    private const string KEY_UNLOCKED = "unlockedBalls";
    private const string KEY_VOLUME_MUSIC = "volumeMusic";
    private const string KEY_VOLUME_SFX = "volumeSFX";

    public static void SaveScore(int points)
    {
        // Guardar puntos acumulados
        int current = PlayerPrefs.GetInt(KEY_POINTS, 0);
        PlayerPrefs.SetInt(KEY_POINTS, current + points);

        // Actualizar highscore si corresponde
        int highScore = PlayerPrefs.GetInt(KEY_HIGHSCORE, 0);
        if (points > highScore)
            PlayerPrefs.SetInt(KEY_HIGHSCORE, points);

        PlayerPrefs.Save();
    }

    public static int GetTotalPoints() => PlayerPrefs.GetInt(KEY_POINTS, 0);
    public static int GetHighScore() => PlayerPrefs.GetInt(KEY_HIGHSCORE, 0);

    public static void UnlockBall(string ballName)
    {
        string current = PlayerPrefs.GetString(KEY_UNLOCKED, "");
        if (!current.Contains(ballName))
        {
            PlayerPrefs.SetString(KEY_UNLOCKED, current + ballName + ",");
            PlayerPrefs.Save();
        }
    }

    public static bool IsBallUnlocked(string ballName)
    {
        // El balón de fútbol siempre disponible
        if (ballName == "Futbol") return true;
        string saved = PlayerPrefs.GetString(KEY_UNLOCKED, "");
        return saved.Contains(ballName);
    }

    public static void SaveVolume(float music, float sfx)
    {
        PlayerPrefs.SetFloat(KEY_VOLUME_MUSIC, music);
        PlayerPrefs.SetFloat(KEY_VOLUME_SFX, sfx);
        PlayerPrefs.Save();
    }

    public static float GetMusicVolume() => PlayerPrefs.GetFloat(KEY_VOLUME_MUSIC, 1f);
    public static float GetSFXVolume() => PlayerPrefs.GetFloat(KEY_VOLUME_SFX, 1f);
}