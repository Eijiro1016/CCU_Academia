using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    private SceneTransition sceneTransition;

    void Awake()
    {
        // 使用新版 Unity 建議的方法
        sceneTransition = Object.FindFirstObjectByType<SceneTransition>();
    }

    public void SelectScene(int index)
    {
        if (sceneTransition != null)
        {
            sceneTransition.FadeToScene(index);
        }
        else
        {
            Debug.LogWarning("SceneTransition 未找到，無法切場景！");
        }
    }
}
