using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class ButtonFunctions : MonoBehaviour
{
    public Text staminatxt;
    public  int stamina;
    public int timeNow;
    public int realTime;

    public AudioMixer mixer;
    public AudioSource mainMusic;
    public AudioSource btnSound;

    System.DateTime currentTime;
    


    public void GoPlayScene()
    {
        if (stamina < 2) return;
        if (stamina == 20)
        {
            timeNow = System.DateTime.Now.Day * 3600 * 24 + System.DateTime.Now.Hour * 3600 + System.DateTime.Now.Minute * 60 + System.DateTime.Now.Second;
           // Debug.Log(timeNow);
            PlayerPrefs.SetInt("timerStarted", timeNow );
        }
        stamina -= 2;
        PlayerPrefs.SetInt("stamina", stamina);
        SceneManager.LoadScene(1);
        
    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitApp()
    {
        PlayerPrefs.SetInt("stamina", stamina);
        Application.Quit();

    }

    void Start()
    {
        stamina = PlayerPrefs.GetInt("stamina");
        if (PlayerPrefs.GetInt("timerStarted") == 0) stamina = 20;
        if (SceneManager.GetActiveScene().buildIndex ==0) mainMusic.Play();
        
    }
   
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0) return;
        staminatxt.text = stamina.ToString()+"/20";
        if (stamina != 20)
        {
            realTime = System.DateTime.Now.Day * 3600 * 24 + System.DateTime.Now.Hour * 3600 + System.DateTime.Now.Minute * 60 + System.DateTime.Now.Second;
            //bug.Log(realTime - PlayerPrefs.GetInt("timerStarted"));
            if (realTime - PlayerPrefs.GetInt("timerStarted") > 120) stamina+=(realTime - PlayerPrefs.GetInt("timerStarted")) / 60;
            if (stamina > 20) stamina = 20;
        }
    }

    public void SetVolume(float volume)
    {
        mixer.SetFloat("volume", volume);
    }

    public void BtnPressed()
    {
        btnSound.Play();
    }
}
