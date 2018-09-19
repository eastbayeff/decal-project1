using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject {

    private int defense = 0;
    private int foodPerMove = 1;
    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;
    public Text foodText;

    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip gameOverSound;
    public AudioClip exitSound;

    private MenuManager menuManager;
    private Animator animator;
    private int food;
    private int sceneID;

    private void Awake()
    {
        menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
    }

    protected override void Start ()
    {
        sceneID = SceneManager.GetActiveScene().buildIndex;
        animator = GetComponent<Animator>();

        food = GameManager.Instance.playerFoodPoints;
        foodText.text = "Food: " + food;

        ModifyStats(GameManager.Instance.junkEffects);

        base.Start();
	}

    void ModifyStats(JunkEffects effects)
    {
        pointsPerFood += effects.foodPerPickup;
        pointsPerSoda += effects.foodPerPickup;
        wallDamage += effects.attack;
        defense += effects.defense;
        foodPerMove += effects.foodPerMove;
    }

    // retain data in GameManager when player object disabled
    private void OnDisable()
    {
        GameManager.Instance.playerFoodPoints = food;
    }

    void Update ()
    {
        // disallow movement input if it's not the player's turn, or if the pause menu is active
        if (!GameManager.Instance.playersTurn || menuManager.isPaused)
            return;

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        // prevent diagonal input
        if (horizontal != 0)
            vertical = 0;

        #region touchControls
        /* touch input
         * 
        if (Input.touchCount > 0)
        {
            Touch myTouch = Input.touches[0];

            if (myTouch.phase == TouchPhase.Began)
            {
                touchOrigin = myTouch.position;
            }
            else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
            {
                Vector2 touchEnd = myTouch.position;
                float x = touchEnd.x - touchOrigin.x;
                float y = touchEnd.y - touchOrigin.y;
                touchOrigin.x = -1;
                if (Mathf.Abs(x) > Mathf.Abs(y))
                    horizontal = x > 0 ? 1 : -1;
                else
                    vertical = y > 0 ? 1 : -1;
            }

        }
        */
        #endregion

        // do work if input is non-zero
        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove<Wall>(horizontal, vertical);
        }
	}

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food -= foodPerMove;
        foodText.text = "Food: " + food;

        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;
        if (Move (xDir, yDir, out hit))
        {
            SoundManager.Instance.RandomizeSfx(moveSound1, moveSound2);
        }

        CheckIfGameOver();

        GameManager.Instance.playersTurn = false;
    }

    // TODO: this is a clunky method to interact with the world
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            SoundManager.Instance.PlaySingle(exitSound);
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            food += pointsPerFood;
            foodText.text = "+" + pointsPerFood + " Food: " + food;
            SoundManager.Instance.RandomizeSfx(eatSound1, eatSound2);
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
            foodText.text = "+" + pointsPerSoda + " Food: " + food;
            SoundManager.Instance.RandomizeSfx(drinkSound1, drinkSound2);
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Junk")
        {
            foodText.text = "What's this strange junk do?!";

            if (other.GetComponent<RandomJunk>().junk.sound != null)
                SoundManager.Instance.PlaySingle(other.GetComponent<Junk>().sound);


            GameManager.Instance.junkCollected.Add(other.GetComponent<RandomJunk>().junk);
            other.gameObject.SetActive(false);
        }
    }

    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("playerChop");

        Debug.Log("player damage during chop: " + wallDamage);
    }

    private void Restart()
    {
        SceneManager.LoadScene(sceneID);
    }

    public void LoseFood(int loss)
    {
        animator.SetTrigger("playerHit");
        food -= (loss - defense);
        foodText.text = "-" + loss + " Food: " + food;
        CheckIfGameOver();
    }

    private void CheckIfGameOver()
    {
        if (food <= 0)
        {
            SoundManager.Instance.PlaySingle(gameOverSound);
            SoundManager.Instance.musicSource.Stop();
            GameManager.Instance.GameOver();      
        }      
    }
}