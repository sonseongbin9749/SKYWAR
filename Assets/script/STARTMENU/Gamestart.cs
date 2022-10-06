using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gamestart : MonoBehaviour
{
    public void OnClickStartButton()
    {
        SceneManager.LoadScene("Start");
    }
    public void GameExit()
    {
        Application.Quit();
    }
}
