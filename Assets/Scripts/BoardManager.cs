using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random; // uses Unity's Random generator, not C#

public class BoardManager : MonoBehaviour {

    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    [Header("Gameboard size")]
    public int columns = 8;
    public int rows = 8;

    [Header("Number of random items to spawn:")]
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);

    [Header("Prefabs used in level creation:")]
    public GameObject exit;
    public GameObject junk;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;

    private Slider loadingSlider;
    private float currentLoadPercent = 0;

    private List<Vector3> gridPositions = new List<Vector3>();

    // Sets up "playable" region as list of Vector3 coordinates
    void InitializeList()
    {
        gridPositions.Clear();

        // leaves one space around edges
        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }

        UpdateLoadProgress(.33f);
    }

    void UpdateLoadProgress(float amountCompleted)
    {
        loadingSlider = GameManager.Instance.loadingSlider;

        currentLoadPercent += amountCompleted;
        loadingSlider.value = currentLoadPercent;
    }

    // Sets up board tiles
    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        // drawing outer walls around the current game board size
        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                // choose outerwall tile if current position is around the edge
                if (x == -1 || x == columns || y == -1 || y == rows)
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }
        }

        UpdateLoadProgress(.33f);
    }

    // returns a random position on the playable area; removes that choice from future possibilities
    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex); // don't spawn 2 things in one location

        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitializeList();

        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);

        // place the map's junk powerup
        Vector3 junkPosition = RandomPosition();
        Instantiate(junk, junkPosition, Quaternion.identity);

        // enemies # based on logarithmic difficulty curve
        int enemyCount = (int)Mathf.Log(level, 2f);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);

        // exit always in the same location
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);

        UpdateLoadProgress(.33f);
    }
}
