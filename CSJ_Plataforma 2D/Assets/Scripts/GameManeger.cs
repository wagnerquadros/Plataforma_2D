using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Import para utilizar a interface com o usuário
using UnityEngine.SceneManagement; //Import para trabalhar com cenas

public class GameManeger : MonoBehaviour
{

    public static GameManeger instance;

    public int score;
    public Text scoreText;

    private void Awake()
    {
        DontDestroyOnLoad(this); // mantem um objeto entre cenas

        if (instance == null) // checar se já existe um player na cena
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }


        if (PlayerPrefs.GetInt("score") > 0) // verifica se o valor armazenado para score é maior que 0
        {
            score = PlayerPrefs.GetInt("score");
            scoreText.text = score.ToString();
        }
    }

    public void GetCoin()
    {
        score++;
        scoreText.text = score.ToString();

        PlayerPrefs.SetInt("score", score); // salvando as moedas
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(1);
    }

}
