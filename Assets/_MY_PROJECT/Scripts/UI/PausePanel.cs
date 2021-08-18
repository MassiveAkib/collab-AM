using System;
using System.Collections;
using System.Collections.Generic;
using MassiveStar;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{

    public GameObject crossIcon;
    public GameObject Panel;


    private void Awake()
    {
        if (crossIcon == null || Panel == null) return;

        crossIcon.SetActive(true);
        Panel.SetActive(false);
    }

    public void OnPause()
    {
        if (crossIcon == null || Panel == null) return;
        crossIcon.SetActive(false);
        Panel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnUnpause()
    {
        if (crossIcon == null || Panel == null) return;
        Time.timeScale = 1f;
        crossIcon.SetActive(true);
        Panel.SetActive(false);
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMenu(string scene)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(scene);
    }

    public void skipLevel(string scene)
    {
        SceneManager.LoadScene(scene);
    }

}
