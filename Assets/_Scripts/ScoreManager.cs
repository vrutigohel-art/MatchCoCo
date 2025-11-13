using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    
    public TextMeshProUGUI scoretext;
    public TextMeshProUGUI wonscoretext;
   
    public void UpdateScore(int score)
    {
        PlayerPrefs.SetInt("GameScore", score);
        scoretext.text = wonscoretext.text = score.ToString();
    }
}
