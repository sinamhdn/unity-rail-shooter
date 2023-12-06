using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    int score;
    TextMeshProUGUI scoreText;

    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        scoreText.text = score.ToString();
    }

    public void Scored(int scorePerHit)
    {
        score += scorePerHit;
        scoreText.text = score.ToString();
    }
}
