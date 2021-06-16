using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject settingButton;
    public GameObject diaryButton;
    public GameObject drivingPanel;
    public GameObject exitButton;
    
    private Animator drivingPanAnim;
    private bool isOpened = false;
    // Start is called before the first frame update
    void Start()
    {
        drivingPanAnim = drivingPanel.GetComponent<Animator>();
        exitButton.GetComponent<Button>().onClick.AddListener(ExitGame);
        settingButton.GetComponent<Button>().onClick.AddListener(OpenSettings);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OpenSettings()
    {
        if(!isOpened)
        {
            settingButton.GetComponent<Button>().enabled = false;
            drivingPanAnim.SetTrigger("show");
            isOpened = true;
        }
        else 
        {
            settingButton.GetComponent<Button>().enabled = false;
            drivingPanAnim.SetTrigger("hide");
            isOpened = false;
        }
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
