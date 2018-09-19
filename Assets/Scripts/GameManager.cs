using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// A package of information about collected junk
public struct JunkEffects
{
    // Player
    public int attack;
    public int defense;
    public int foodPerPickup;
    public int foodPerMove;

    // World
    public int column;
    public int row;
    public int enemy;
    public int foodPerLevel;
}

public class GameManager : MonoBehaviour {

    #region reference variables
    public float levelStartDelay = 2f;
    public float turnDelay = .1f;

    public static GameManager Instance;
    [HideInInspector]
    public JunkEffects junkEffects;

    public int playerFoodPoints = 100;
    [HideInInspector]
    public bool playersTurn = true;
    [HideInInspector]
    public List<Junk> junkCollected = new List<Junk>();

    private Text levelText;
    private GameObject levelImage;
    private BoardManager boardScript;
    private Settings settings;
    private int level = 0;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetup = true;
    #endregion


    public Slider loadingSlider
    {
        get { return levelImage.GetComponentInChildren<Slider>(); }
        protected set { }
    }

    public bool settingUp
    {
        get { return doingSetup; }
        protected set { }
    }

    private void Awake()
    {
        // Singleton
        #region 
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        #endregion

        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
    }

    void Update()
    {
        if (playersTurn || enemiesMoving || doingSetup)
            return;

        StartCoroutine(MoveEnemies());
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        ModifyLevelDetails();

        level++;
        InitGame();
    }

    // allows the GameManager to make changes to the game state before the level is built
    void ModifyLevelDetails()
    {
        // Take changes from main settings menu at start
        settings = FindObjectOfType<Settings>();
        if (settings != null)
        {
            boardScript.columns = settings.GetColumns;
            boardScript.rows = settings.GetRows;
        }
        
        junkEffects = ParseJunk(Instance.junkCollected);

        // TODO(chris): Make changes to the game board based on collected junk

    }

    // Returns a JunkEffects struct with all aggregated effects from collected junk, ensures nothing out-of-bounds
    JunkEffects ParseJunk(List<Junk> allJunk)
    {
        JunkEffects junkEffects = new JunkEffects();

        foreach (Junk item in allJunk)
        {
            junkEffects.attack += item.attack;
            junkEffects.defense += item.defense;
            junkEffects.foodPerPickup += item.foodPerPickup;
            junkEffects.foodPerMove += item.foodPerMove;
            junkEffects.column += item.column;
            junkEffects.row += item.row;
            junkEffects.enemy += item.enemy;
            junkEffects.foodPerLevel += item.foodPerLevel;
        }

        return junkEffects;
    }

    // initializes a new game board for a new level and populates the items on it
    void InitGame()
    {
        doingSetup = true;

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);

        enemies.Clear();
        boardScript.SetupScene(level);
    }

    // hides the black screen shown between level transitions
    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {
        levelText.text = "After " + level + " days, you perished.";
        levelImage.SetActive(true);
        enabled = false;
    }
	


    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    // Scene management events
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    // Co-routine to move the enemies and force time between allowable inputs
    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);

        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        playersTurn = true;
        enemiesMoving = false;
    }
}
