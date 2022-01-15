using System;
using System.Collections;
using System.Collections.Generic;
using Pools;
using Stats;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;


public class WaveController : MonoBehaviour
{

	private UIHandler _uiHandler;
	[SerializeField] private bool enableSpawn = true;
	// needed for spawning
	[SerializeField]
	GameObject spawnObject;

	[SerializeField]
	GameObject plane;

	// spawn control
	const float MinSpawnDelay = 1; // seconds
	const float MaxSpawnDelay = 4; // seconds

	Timer spawnTimer;
	Timer waveTimer;
	private float timeToKillAnEnemy = 20; // seconds
	public int wave;
	public int maxNoOfWaves;
	public int level;

	// spawn location support
	float randomX;
	float randomY;
	float randomZ;

	// maximum number of enemies
	public int noOfEnemies = 0;
	public int maxNoOfEnemies;
	public int deadEnemies = 0;
	private static readonly int IsDead = Animator.StringToHash("isDead");
	
	// enemy types
	private enum EnemyType
	{
		Dragon,
		Turtle,
		Wizzard,
		Golem
	};
	
	// sum of spawn chances for current level == 1
	private Dictionary<EnemyType, float> spawnChanceOnCurrentLevel;
	
	// spawnChances[level][EnemyType]
	// If key for current level doesn't exists, then use last "spawnChanceOnCurrentLevel"
	private Dictionary<int, Dictionary<EnemyType, float>> spawnChances; 
	
	void Start()
	{
		_uiHandler = GameObject.Find("UIModifier").GetComponent<UIHandler>();
		if (!enableSpawn) return;

		plane = GameObject.FindWithTag("Plane");

		// save spawn boundaries for efficiency
		float randomX = Random.Range(plane.transform.position.x - plane.transform.localScale.x / 2, plane.transform.position.x + plane.transform.localScale.x / 2);
		float randomY = Random.Range(plane.transform.position.y - plane.transform.localScale.y / 2, plane.transform.position.y + plane.transform.localScale.y / 2);
		float randomZ = Random.Range(plane.transform.position.y - plane.transform.localScale.z / 2, plane.transform.position.y + plane.transform.localScale.z / 2);

		// spawn chance initialize
		// default spawn chance
		spawnChanceOnCurrentLevel = new Dictionary<EnemyType, float>()
		{
			{EnemyType.Dragon, 0},
			{EnemyType.Turtle, 0},
			{EnemyType.Wizzard, 0},
			{EnemyType.Golem, 1}
		};

		// list of spawn chances according to level
		spawnChances = new Dictionary<int, Dictionary<EnemyType, float>>()
		{
			{1, new Dictionary<EnemyType, float>()
			{
				{EnemyType.Dragon, 0},
				{EnemyType.Turtle, 0},
				{EnemyType.Wizzard, 0},
				{EnemyType.Golem, 1}
			}},
			{2, new Dictionary<EnemyType, float>()
			{
				{EnemyType.Dragon, 0},
				{EnemyType.Turtle, 0},
				{EnemyType.Wizzard, 0.1f},
				{EnemyType.Golem, 0.9f}
			}},
			{3, new Dictionary<EnemyType, float>()
			{
				{EnemyType.Dragon, 0},
				{EnemyType.Turtle, 0.1f},
				{EnemyType.Wizzard, 0.2f},
				{EnemyType.Golem, 0.7f}
			}},
			{4, new Dictionary<EnemyType, float>()
			{
				{EnemyType.Dragon, 0.1f},
				{EnemyType.Turtle, 0.2f},
				{EnemyType.Wizzard, 0.3f},
				{EnemyType.Golem, 0.4f}
			}},
			{5, new Dictionary<EnemyType, float>()
			{
				{EnemyType.Dragon, 0.2f},
				{EnemyType.Turtle, 0.2f},
				{EnemyType.Wizzard, 0.3f},
				{EnemyType.Golem, 0.3f}
			}},
		};
		
		
		
		// create and start spawn timer
		spawnTimer = gameObject.AddComponent<Timer>();
		spawnTimer.Duration = Random.Range(MinSpawnDelay, MaxSpawnDelay);
		spawnTimer.Run();

		waveTimer = gameObject.AddComponent<Timer>();
		wave = 0;
		startWave();
	}


	void Update()
	{
		if (!enableSpawn) return;
		if (wave > maxNoOfWaves) return;


		// check for time to spawn a new enemy
		if (spawnTimer.Finished && noOfEnemies < maxNoOfEnemies)
		{
			objectSpawn();

			// change spawn timer duration and restart
			spawnTimer.Duration = Random.Range(MinSpawnDelay, MaxSpawnDelay);
			spawnTimer.Run();
		}

		if (waveTimer.Finished)
		{
			Debug.Log("Wave timer finished.");
			startWave();
		}

		if (deadEnemies == maxNoOfEnemies)
		{
			Debug.Log("All enemies from this wave are dead.");
			startWave();
		}
	}

	public void startLevel(int level)
	{
		this.level = level;
		maxNoOfWaves = (level/2) + 2;
		wave = 0;
		if(level != 1)
        {
			startWave();
		}
	}

	public bool levelFinished()
	{
		return wave > maxNoOfWaves && deadEnemies == maxNoOfEnemies;
	}

	void startWave()
	{
		wave++;
		if (wave > maxNoOfWaves) return;
		Debug.Log("Starting wave " + level + "." +  wave + " out of " + maxNoOfWaves);
		_uiHandler.UpdateWaveInfo(wave.ToString(), maxNoOfWaves.ToString());
		int enemiesForCurrentWave = wave + level + 1; //(int)System.Math.Round(wave + level + 1);
		waveTimer.Duration = timeToKillAnEnemy * enemiesForCurrentWave / level;
		waveTimer.Run();
		maxNoOfEnemies = noOfEnemies + enemiesForCurrentWave;
	}

	public void killEnemy()
	{
		deadEnemies++;
	}

	void objectSpawn()
	{
		// generate random location and create new object
		Vector3 randomPosition = GetARandomPos(plane);
		GameObject enemyObject = null;
		float randomNumber = Random.value;
		
		if (spawnChances.ContainsKey(level))
		{
			spawnChanceOnCurrentLevel = spawnChances[level];
		}
		
		print("Spawn Chances: " + string.Join(",", spawnChanceOnCurrentLevel));
		
		if (randomNumber <= spawnChanceOnCurrentLevel[EnemyType.Dragon])
		{
	        enemyObject = DragonPool.instance.GetPooledObject();
            FindObjectOfType<AudioManager>().Play("dragonSpawn");
        }
		else if (randomNumber <= spawnChanceOnCurrentLevel[EnemyType.Dragon] +
		         spawnChanceOnCurrentLevel[EnemyType.Turtle])
		{
			enemyObject = TurtlePool.instance.GetPooledObject();
            FindObjectOfType<AudioManager>().Play("turtleSpawn");
        }
		else if (randomNumber <= spawnChanceOnCurrentLevel[EnemyType.Dragon] +
								 spawnChanceOnCurrentLevel[EnemyType.Turtle] +
								 spawnChanceOnCurrentLevel[EnemyType.Wizzard])
		{
	        enemyObject = WizardPool.instance.GetPooledObject();
            FindObjectOfType<AudioManager>().Play("wizardSpawn");
        }
		else
		{
			enemyObject = EnemyPool.instance.GetPooledObject();
            FindObjectOfType<AudioManager>().Play("rocks1");
            FindObjectOfType<AudioManager>().Play("golemSpawn");
        }
		
		if (enemyObject != null)
        {
	        randomPosition.y += enemyObject.GetComponent<EnemyStats>().OffsetYPosition;
			enemyObject.transform.position = randomPosition;
			enemyObject.GetComponent<EnemyStats>().InitStats();
			enemyObject.GetComponent<EnemyController>().enabled = true;
			enemyObject.GetComponent<Animator>().SetBool(IsDead, false);
			enemyObject.SetActive(true);
		}

		noOfEnemies += 1;
	}

	public Vector3 GetARandomPos(GameObject plane)
	{
		Mesh planeMesh = plane.GetComponent<MeshFilter>().mesh;
		Bounds bounds = planeMesh.bounds;

		float minX = plane.transform.position.x - plane.transform.localScale.x * bounds.size.x * 0.5f;
		float minZ = plane.transform.position.z - plane.transform.localScale.z * bounds.size.z * 0.5f;

		// Possible spawn positions:
		// 0 -> Top; 1 -> Right; 2 -> Bottom; 3 -> Left
		int spawnPosition = Random.Range(0, 4);
		string[] positionNames = new string[] { "Top", "Right", "Bottom", "Left" };
		float x, z;
		switch (spawnPosition)
		{
			case 0:
				x = minX;
				z = 0;
				break;
			case 1:
				x = 0;
				z = -minZ;
				break;
			case 2:
				x = -minX;
				z = 0;
				break;
			case 3:
				x = 0;
				z = minZ;
				break;
			default:
				x = 0;
				z = 0;
				Debug.Log("Something wrong happened.");
				break;
		}

		Vector3 newVec = new Vector3(x, plane.transform.position.y, z);
		return newVec;
	}

}
