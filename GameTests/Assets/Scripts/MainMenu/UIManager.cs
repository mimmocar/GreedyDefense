using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIManager : MonoBehaviour
{
   

    public void Select(string selectedButton)
    {
        GameControl.Load(selectedButton);
        //SceneManager.LoadScene(selectedButton)
    }

    public void Quit()
    {
        //Application.Quit();
        GameControl.Quit();
    }
}
