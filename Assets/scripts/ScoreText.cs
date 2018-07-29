using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {

    Text score;

    void Start()
    {
        score = GetComponent<Text>();
        score.text = "Score: " + GameManager.Instance.Score;
    }

}
