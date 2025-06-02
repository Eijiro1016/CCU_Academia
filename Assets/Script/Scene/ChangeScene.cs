using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    private SceneTransition sceneTransition;

    void Awake()
    {
        if (sceneTransition == null)
        {
            sceneTransition = FindObjectOfType<SceneTransition>();
        }
    }

    public void SelectScene(int sceneIndex)
    {
        if (sceneTransition != null)
        {
            sceneTransition.FadeToScene(sceneIndex);
        }
        else
        {
            Debug.LogWarning(" SceneTransition reference not assigned!");
        }
    }
}
