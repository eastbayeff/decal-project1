using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Tooltip("UI Image to display during pause")]
    public Image pauseImage;

    [HideInInspector]
    public bool isPaused;
    public AudioClip pauseSound;
    public AudioClip unpauseSound;

    private void Start()
    {
        isPaused = false;
    }

    void Update ()
    {
        // Pause menu button determined in project settings > input
        // toggle the pause menu if not currently setting up level
        if (Input.GetButtonDown("PauseMenu") && !GameManager.Instance.settingUp)
        {
            isPaused = !pauseImage.gameObject.activeSelf;
            pauseImage.gameObject.SetActive(isPaused);
            SoundManager.Instance.PlaySingle(isPaused ? pauseSound : unpauseSound);
        }
    }
}
