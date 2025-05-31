using UnityEngine;
using UnityEngine.UI;
//using static System.Net.Mime.MediaTypeNames;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Player_health playerHealth;
    [SerializeField] private Image fillImage;

    private void Update()
    {
        float ratio = playerHealth != null ? playerHealth.CurrentHealthRatio() : 0f;
        fillImage.fillAmount = ratio;
    }
}
