using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    FreeRoam,
    Dialog,
    Battle
}

public class gameControl : MonoBehaviour
{
    public static gameControl Instance { get; private set; }

    [SerializeField] private PlayerMovement playerMovement;
    private GameState state;

    private void Awake()
    {
        // 保證只有一個 gameControl 存在
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        // 啟動時載入主選單（SampleScene）
        SceneManager.LoadScene("SampleScene");
    }

    private void Update()
    {
        // 若尚未找到 playerMovement，動態尋找
        if (playerMovement == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerMovement = player.GetComponent<PlayerMovement>();
            }
            return;
        }

        if (state == GameState.FreeRoam)
        {
            playerMovement.HandleUpdate();
        }
        else if (state == GameState.Dialog)
        {
            DialogManager.instance.HandleUpdate();
        }
    }

    public void SetGameState(GameState newState)
    {
        state = newState;
    }
}
