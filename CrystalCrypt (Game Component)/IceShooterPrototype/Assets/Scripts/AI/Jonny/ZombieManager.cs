using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour {

    [SerializeField]
    Transform[] spawnPoints;
    [SerializeField]
    GameObject enemy;
	UIManager uiManager;
	GameManager gameManager;
    private float InitWaveTime;
	private float InitWaveDelay;
	bool waveComplete = false;
	int numOfPlayers;
    
	public float spawnTime;
	public int wave = 1;
	public float waveTime = 10.0f;
	public int EnemyCount;
    public bool ImpossibleMode = false;
    public bool HardMode = false;
	public float waveDelay = 5f;
    
    public float waveTimeMultiplier = 1.1f;
    public float spawnTimeMultiplier = 1.1f;
    public float enemyHealthMultiplier = 1.15f;
    public float enemySpeedMultiplier = 1.1f;

    public float enemyHealth;
	public float enemySpeed;
    

    // Use this for initialization
    void Start ()
    {

		uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		numOfPlayers = gameManager.numberOfPlayers;
		uiManager.UpdateWave(wave);
		spawnTime /= numOfPlayers;
		//spawnTimeMultiplier += numOfPlayers / 4;

        if (HardMode)
        {
            waveTime = 15f;
            spawnTime = 0.5f;
            waveDelay = 2f;
            spawnTimeMultiplier = 1.25f;
        }
        if (ImpossibleMode)
        {
            waveTime = 20f;
            spawnTime = 0.1f;
            waveDelay = 2f;
            spawnTimeMultiplier = 1.75f;
        }
        
        enemyHealth = 100f;
        EnemyCount = 0;
        StartSpawning();
        InitWaveTime = waveTime;
        InitWaveDelay = waveDelay;
    }

    void Spawn()
    {
        //print("WTF!");
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);

        // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
        Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        EnemyCount++;
    }

    void StartSpawning()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    // Update is called once per frame
    void Update ()
    {
        if (!waveComplete)
        {
            waveTime -= Time.deltaTime;
        }
        
        if (waveTime <= 0.0f)
        {           
            CancelInvoke();   // stop enemies from spawning
            waveComplete = true;
            waveTime = InitWaveTime; // resets wave time
            
            // update wave ui text
        }

        // Wave Delay
        if (waveComplete && EnemyCount < 1)
        {
            print("Wave " + wave + " Complete get ready for next wave!");
            waveDelay -= Time.deltaTime;
            //print(waveDelay);
        }
        if (waveDelay <= 0.0f)
        {
            waveComplete = false;
            waveDelay = InitWaveDelay; // Reset Wave Delay
            wave++;
            //enemy.GetComponent<ZombieAI>().speed *= enemySpeedMultiplier;
			enemySpeed *= enemySpeedMultiplier; // increase enemy speed
            uiManager.UpdateWave(wave);
            print("WAVE " + wave + "!");
            InitWaveTime *= waveTimeMultiplier; // increase wave time (longer wave)
            waveTime = InitWaveTime; // reset the Wave Delay
            spawnTime /= spawnTimeMultiplier; // decrease spawn time (faster spawning)
            enemyHealth *= enemyHealthMultiplier; // increase enemy health 
            StartSpawning(); // Start spawning enemies!
        }
        if (EnemyCount < 0)
        {
            EnemyCount = 0;
        }
    }
}
