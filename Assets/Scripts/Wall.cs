using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    public Sprite damageSprite;
    public int health = 4;

    public AudioClip chopSound1;
    public AudioClip chopSound2;

    private SpriteRenderer spriteRenderer;

	
	void Awake ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
    public void DamageWall (int loss)
    {
        spriteRenderer.sprite = damageSprite;
        health -= loss;

        SoundManager.Instance.RandomizeSfx(chopSound1, chopSound2);

        if (health <= 0)
            gameObject.SetActive(false);      
        
    }

}
