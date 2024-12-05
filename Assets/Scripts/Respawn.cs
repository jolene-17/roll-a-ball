using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Respawn : MonoBehaviour
{
    public void LoadMainMenu()
    {
        Debug.Log("Respawn button pressed");
        SceneManager.LoadScene(0);
        Debug.Log("Respawn button triggered");

    }

    public void OnQuitButton ()
    {
        Application.Quit();
    }
}
