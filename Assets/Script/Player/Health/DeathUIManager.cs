using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathUIManager : MonoBehaviour
{
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private Player_health playerHealth;

    private void Update()
    {
        if (playerHealth.CurrentHealthRatio() <= 0 && !deathPanel.activeSelf)
        {
            deathPanel.SetActive(true);
        }
    }

    // 掛在按鈕的 OnClick 事件
    public void RevivePlayer()
    {
        playerHealth.ResetHealth(); // 讓玩家復活
        deathPanel.SetActive(false);
    }

    // public void BackToHome()
    // {
    //     SceneManager.LoadScene("MainMenu"); // 請確保這個場景名稱正確
    // }
}
