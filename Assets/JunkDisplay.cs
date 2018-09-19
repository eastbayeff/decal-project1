using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JunkDisplay : MonoBehaviour {

    [Tooltip("Where to display the collected junk; panel should have a grid layout")]
    public Transform junkPanel;
    public GameObject junkUIPrefab;

    private void Start()
    {
        foreach (Junk item in GameManager.Instance.junkCollected)
        {
            // draw it on the screen
            var junk = Instantiate(junkUIPrefab, junkPanel);
            junk.GetComponent<Image>().sprite = item.sprite;
        }
    }

}
