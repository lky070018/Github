using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleManager : MonoBehaviour {
    [SerializeField]
    GameObject menu;
    [SerializeField]
    GameObject options;

    public void OptionPanel()
    {
        menu.SetActive(false);
        options.SetActive(true);
    }
    // start game to the lobby
    public void InToLobby()
    {
        SceneManager.LoadScene(1);
    }
    // quit game button
    public void ExitGame()
    {
        Application.Quit();
    }   
}
