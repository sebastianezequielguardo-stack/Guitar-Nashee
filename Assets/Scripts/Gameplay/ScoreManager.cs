using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public int totalNotes = 0;
    public int hitNotes = 0;

    public void RegisterHit()
    {
        score += 100;
        hitNotes++;
    }

    public void RegisterMiss()
    {
        score -= 50;
    }

    public float GetAccuracy()
    {
        return totalNotes > 0 ? (hitNotes / (float)totalNotes) * 100f : 0f;
    }
}
