using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    public void OnPlayButton ()
    {
        Debug.Log("Enter button pressed");        
        SceneManager.LoadScene(1);
    }

    public void OnQuitButton ()
    {
        Application.Quit();
    }
}
