using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    public GameObject settingButton;
    public GameObject drivingPanel;

    public void showEnd()
    {
        settingButton.GetComponent<Button>().enabled = true;
    }
    public void hideEnd()
    {
        drivingPanel.GetComponent<Animator>().SetTrigger("finished");
        settingButton.GetComponent<Button>().enabled = true;
    }
}
