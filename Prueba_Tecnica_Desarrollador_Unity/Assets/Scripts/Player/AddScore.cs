using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScore : MonoBehaviour
{
    public int addScore;
    public Score ScorePlus;
    private void OnCollisionEnter2D(Collision2D ScoreAdd)
    {
        //al colisionar agrega el score y lo asigna al valor del score general
        string tag = ScoreAdd.gameObject.tag;
        if (tag == "Pig")        {
            
            addScore = addScore + 1000;
            ScorePlus.totalScore += addScore;
        }
        else if (tag == "Brick")
        {
            addScore = addScore + 500;
            ScorePlus.totalScore += addScore;
        }
    }
}
