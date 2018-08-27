using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{

    public GameObject play;
    public GameObject pause;
    public Texture darken;

	
    public void OnPlayClicked()
    {
        play.SetActive(false);
        pause.SetActive(true);
        Time.timeScale = 1;

    }

    public void OnPauseClicked()
    {
        pause.SetActive(false);
        play.SetActive(true);
        Time.timeScale = 0;
        AudioListener.pause = true;
    }

	public void OnGUI()
	{
        if (play.activeInHierarchy == true)
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), darken, ScaleMode.StretchToFill);
    
	}
}
