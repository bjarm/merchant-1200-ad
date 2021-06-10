using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject settingButton;
    public GameObject diaryButton;

    public GameObject exitButton;
    
    // Start is called before the first frame update
    void Start()
    {
        exitButton.GetComponent<Button>().onClick.AddListener(ExitGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OpenSettings()
    {
        
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
