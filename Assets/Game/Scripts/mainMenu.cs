using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public AudioSource audio_JiaoSheng;
    
   
    
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayJiaoSheng()
    {
        audio_JiaoSheng.Play();
    }
}
