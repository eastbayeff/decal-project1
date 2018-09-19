using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private List<Junk> inventory = new List<Junk>();


    private void OnEnable()
    {
        inventory = GameManager.Instance.junkCollected;
        Debug.Log("playerinventory initialized. junk count = " + inventory.Count);
    
    }

}
