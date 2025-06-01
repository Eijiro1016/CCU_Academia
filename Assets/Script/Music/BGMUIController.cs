using UnityEngine;
using UnityEngine.UI;

public class BGMUIController : MonoBehaviour
{
    public Slider volumeSlider;
    public Button muteButton;

    void Start()
    {
        // 初始化滑桿為目前音量
        if (BGMManager.instance != null && volumeSlider != null)
        {
            volumeSlider.value = BGMManager.instance.GetVolume();
        }

        // 設定事件監聽
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(delegate { OnVolumeChanged(); });
        }

        if (muteButton != null)
        {
            muteButton.onClick.AddListener(OnMuteClicked);
        }
    }

    public void OnVolumeChanged()
    {
        if (BGMManager.instance != null)
        {
            BGMManager.instance.SetVolume(volumeSlider.value);
        }
    }

    public void OnMuteClicked()
    {
        if (BGMManager.instance != null)
        {
            BGMManager.instance.ToggleMute();
            volumeSlider.value = BGMManager.instance.GetVolume(); // 更新滑桿
        }
    }
}
