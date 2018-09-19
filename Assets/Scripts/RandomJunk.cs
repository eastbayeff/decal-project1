using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomJunk : MonoBehaviour
{
    [Tooltip("An array of all possible junk that could be spawned")]
    public Junk[] allPossibleJunk;
    private Junk junkItem;

    public Junk junk
    {
        get { return junkItem; }
        protected set { }
    }

    private void Start()
    {
        junkItem = allPossibleJunk[Random.Range(0, allPossibleJunk.Length)];
        GetComponent<SpriteRenderer>().sprite = junkItem.sprite;
    }
}
