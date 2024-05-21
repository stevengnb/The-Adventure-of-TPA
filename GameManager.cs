using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    private float svcoin = 20000;
    private int storage = 0;
    private int maxStorage = 8;
    private float score;
    public int enemyKilled;
    public bool isEndGame = false;
    public List<ShopItem> shopLists = new List<ShopItem>();
    private FadeOut fadeOut;

    public float Svcoin {
        get { return svcoin; }
        set { svcoin = value; }
    }

    public int Storage {
        get { return storage; }
        set { storage = value; }
    }

    public int MaxStorage { 
        get { return maxStorage; }
    }

    public int Score {
        get { return Mathf.FloorToInt(score); }
    }

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    public void EndGame(int type) {
        if(!isEndGame){
            isEndGame = true;
            fadeOut = FindObjectOfType<FadeOut>();
            score = (TimeController.instance.ElapsedTime * 100) + (enemyKilled * 500);

            fadeOut.Fade();
            fadeOut.GameOverType(type);
            svcoin += Mathf.FloorToInt(score);
        }
    }

}
