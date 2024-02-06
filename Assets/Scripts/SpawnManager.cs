using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class spawnManager : MonoBehaviour
{
    // Allow prefabs to be loaded onto SpawnManager
    public GameObject boxPrefab;
    public GameObject coinPrefab;

    // Spawn time variables
    public float startDelay = 2;
    public float spawnInterval = 1.5f;

    // Box scale variables
    public int boxMinScale = 1;
    public int boxMaxScale = 5;

    // Coin spawn elevation variables
    public int minCoinY = -3;
    public int maxCoinY = 3;

    // Start is called before the first frame update
    void Start()
    {
        // Coin and box spawns are separate void functions
        InvokeRepeating("SpawnBox", startDelay, spawnInterval);
        InvokeRepeating("SpawnCoin", startDelay, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void SpawnBox()
    {
        // Generate a box with random size: Vector2 (RandomRange(1, 5), RandomRange(1, 5)
        var box = Instantiate(boxPrefab, new Vector3(8, -4, -1), Quaternion.identity);

        Vector3 RandomScale = new Vector3(Random.Range(boxMinScale, boxMaxScale), Random.Range(boxMinScale, boxMaxScale), 1);
        box.transform.localScale = RandomScale;
    }
    void SpawnCoin()
    {
        Vector3 RandomPos = new Vector3(-11, Random.Range(minCoinY, maxCoinY), -2);

        // Generate a coin at random elevation: 
        var coin = Instantiate(coinPrefab, RandomPos, Quaternion.identity);
    }
}
