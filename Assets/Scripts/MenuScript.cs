using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MenuScreen{
    Menu,
    Win,
    Loss,
    Level,
}
public class MenuScript : MonoBehaviour
{
    public MenuScreen screen;
    public MusicManager manager;

    private void Start()
    {
        switch(screen){
            case MenuScreen.Menu:
                manager.PlayMusic("MainMenu", (1 / 2));
                break;
            case MenuScreen.Win:
                manager.PlayMusic("WinTrack", (1 / 2));
                break;
            case MenuScreen.Loss:
                manager.PlayMusic("LoseTrack", (1 / 2));
                break;
            case MenuScreen.Level:
                manager.PlayMusic("Henhouse", (1 / 2));
                break;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(1);
            manager.PlayMusic("Henhouse", (1 / 2));
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
