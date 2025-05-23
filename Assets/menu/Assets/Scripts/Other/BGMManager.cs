using UnityEngine;

public class BGMManager : MonoBehaviour
{
    private static BGMManager instance;

    void Awake()
    {
        // �Y�w���@�� BGMManager �s�b�A�o�ӴN�۰ʾP��
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // �������|�b�������ɳQ�P��
    }
}
