using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject settingsMenu;
    public EventSystem eventSystem;


    public void StartGame()
    {
        //SceneManager.LoadScene(1);
        StartCoroutine(LoadAsynchronously(1));
    }

    public void QuitGame()
    {
        Debug.LogWarning("Quit application called");
        Application.Quit();
    }

    // Co-routine to restart scene, reloading the board; required for displaying loading progress
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log(progress);

            yield return null;
        }
    }

    public void DisplaySettings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(GameObject.Find("Back Button"));
        
    }

    public void HideSettings()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(GameObject.Find("New Game Button"));
    }

}
