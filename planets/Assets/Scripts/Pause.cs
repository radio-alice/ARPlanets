using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

/// <summary>
/// Pauses and unpauses game
/// </summary>

public class Pause : MonoBehaviour
{
    public static bool paused = true;

    public GameObject pauseButton;
    public GameObject pauseMenu;
    public GameObject clear;
    public ARCoreBackgroundRenderer background;
   
    void Start()
    {
        PauseGame(); //pause game on start
    }

    public void ResumeGame()
    {
        pauseButton.SetActive(true); //enable pause button, clear button, camera feed
        clear.SetActive(true);
        background.enabled = true;

        pauseMenu.SetActive(false); //disable instructions, play button

        Time.timeScale = 1; //run game
        paused = false; //tell other scripts we are in play mode
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true); //enable instructions, play button

        background.enabled = false; //disable pause button, clear button, camera feed
        clear.SetActive(false);
        pauseButton.SetActive(false);

        Time.timeScale = 0; //pause game
        paused = true; //tell other scripts we're paused
    }
}
