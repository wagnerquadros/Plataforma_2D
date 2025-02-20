using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Import para utilizar a interface com o usuário

public class GameManeger : MonoBehaviour
{

    public static GameManeger Instance;

    public int score;
    public Text scoreText;

    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        
    }


    void Update()
    {
        
    }
    public void GetCoin()
    {
        score++;
        scoreText.text = score.ToString();
    }
}
