using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
        // 在編輯器中顯示提示
        Debug.Log("Quit Game");

        // 實際關閉遊戲
        Application.Quit();
    }
}
