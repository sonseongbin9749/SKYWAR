using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gameclear : MonoBehaviour
{
    [SerializeField]
    private Text highscoreText = null;
    protected void Start()
    {
        highscoreText.text = string.Format("HIGHSCORE\n{0}", PlayerPrefs.GetInt("HIGHSCORE", 500));
    }
    public void OnClickStartButton()
    {
        SceneManager.LoadScene("Start");
    }
    public void GameExit()
    {
        Application.Quit();
    }
}