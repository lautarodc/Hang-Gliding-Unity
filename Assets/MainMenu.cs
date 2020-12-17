using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    // El orden en el que se cargan las escenas se configura en Build Settings.
    public void Beginner() {

        SceneManager.LoadScene(1);
    }

    public void Advanced() {
        SceneManager.LoadScene(2);
    }

    public void QuitGame() {
        UnityEngine.Debug.Log("Quit");
        Application.Quit();
    }
}
