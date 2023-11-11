using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEvents : MonoBehaviour
{
    [Header("References")]
    public TurnBasedMove playerScript;
    public Transform playerFoV;
    public Transform lightOfFlashLight;
    public GameObject GameOverScreen;
    public GameObject WinScreen;
    public GameObject playerMovePoint;

    private void Start()
    {
        GameOverScreen.SetActive(false);
        playerFoV.gameObject.SetActive(true);
        lightOfFlashLight.gameObject.SetActive(true);
        playerScript.enabled = true;
    }

    public void GameOver()
    {
        GameOverScreen.SetActive(true);
        playerFoV.gameObject.SetActive(false);
        lightOfFlashLight.gameObject.SetActive(false);
        Invoke(nameof(DelayDisable), 0.2f);
    }

    public void Winner()
    {
        WinScreen.SetActive(true);
        playerFoV.gameObject.SetActive(false);
        lightOfFlashLight.gameObject.SetActive(false);
        Invoke(nameof(DelayDisable), 0.2f);
    }

    private void DelayDisable()
    {
        playerScript.enabled = false;
        playerMovePoint.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
