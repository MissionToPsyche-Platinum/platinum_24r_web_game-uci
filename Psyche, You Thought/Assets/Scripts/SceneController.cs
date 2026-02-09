using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void GoToMain()
    {
        SceneManager.LoadScene(1);
    }

    public void GoToStart()
    {
        SceneManager.LoadScene(0);
    }

    public void GoToRandomMinigame()
    {
        SceneManager.LoadScene(Random.Range(2, 8));
    }

    public void GoToEnd()
    {
        SceneManager.LoadScene(8);
    }

    public void QuitGame ()
    {
        Application.Quit();
    }
}
