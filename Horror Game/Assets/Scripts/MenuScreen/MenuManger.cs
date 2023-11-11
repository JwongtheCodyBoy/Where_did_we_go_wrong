using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManger : MonoBehaviour
{
    [Header("References")]
    public GameObject mainMenu;
    public GameObject helpScreen;

    private void Start()
    {
        BackToMain();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void HelpScreen()
    {
        mainMenu.SetActive(false);
        helpScreen.SetActive(true);
    }

    public void BackToMain()
    {
        helpScreen.SetActive(false);
        mainMenu.SetActive(true);
    }
}
