using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Setting : MonoBehaviour
{
    public GameObject setting;   

    private bool isSettingVisible = false;          


    private void Update()
    {
        // «ö¤U Esc Áä¤Á´«setting¶}Ãö
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isSettingVisible = !isSettingVisible;
            setting.SetActive(isSettingVisible);
        }
    }

}
