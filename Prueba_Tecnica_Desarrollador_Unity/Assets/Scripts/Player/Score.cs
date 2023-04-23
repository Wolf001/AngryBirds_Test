using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public int totalScore;
    public TextMeshProUGUI scoregame;
    public TextMeshProUGUI scoreFinal;
    public TextMeshProUGUI scoreFinalLost;

    public void Update()
    {
        //muestra el calculo del score
        scoregame.text = totalScore.ToString();
        scoreFinal.text = totalScore.ToString();
        scoreFinalLost.text = totalScore.ToString();
    }
}
