using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject settingButton;
    public GameObject diaryButton;

    public GameObject exitButton;

    public GameObject datePanel;
    private Text dateText;

    // Start is called before the first frame update
    void Start()
    {
        dateText = datePanel.GetComponent<Text>();
        exitButton.GetComponent<Button>().onClick.AddListener(ExitGame);
    }

    // Update is called once per frame
    void Update()
    {
        dateText.text = GameManager.currentDate.ToString();
    }

    private void OpenSettings()
    {
        
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
