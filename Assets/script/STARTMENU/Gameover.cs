using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gameover : MonoBehaviour
{
    [SerializeField]
    private Text highscoreText = null;
    void Start()
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
