using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuLogic : MonoBehaviour
{
    public void PlayGame()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayClickSound()
    {
        
        FindObjectOfType<AudioManager>().Play("gameStarts");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void UpdateVolume()
    {

        Slider mainSlider = GameObject.Find("VolumeSlider").GetComponent<Slider>();
        
        if (!PlayerPrefs.HasKey("soundVolume"))
        {
            mainSlider.value = (float)  1.0;
        }
        else
        {
            mainSlider.value = PlayerPrefs.GetFloat("soundVolume");
        }
    }
    public void textUpdate(float value)
    {
        PlayerPrefs.SetFloat("soundVolume", value);
        FindObjectOfType<AudioManager>().ChangeVolume();
    }
}
