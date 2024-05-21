using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreSetter : MonoBehaviour
{

    public TextMeshProUGUI scoreText;

    public void Start() {
        scoreText.SetText(GameManager.instance.Score.ToString());
    }
}
