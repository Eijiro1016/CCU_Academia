using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingSceneManager : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("Overworld");  // 替換成遊戲主場景名稱
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu");   // 替換成主選單場景名稱
    }
}
