﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaveController : MonoBehaviour
{

	[SerializeField] private bool enableSpawn = true;
	// needed for spawning
	[SerializeField]
	GameObject spawnObject;

	[SerializeField]
	GameObject plane;

	// spawn control
	const float MinSpawnDelay = 3;
	const float MaxSpawnDelay = 5;

	const float yBuffer = 1;
	Timer spawnTimer;
	Timer waveTimer;
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

	void Start()
	{
		if (!enableSpawn) return;

		plane = GameObject.FindWithTag("Plane");

		// save spawn boundaries for efficiency
		float randomX = Random.Range(plane.transform.position.x - plane.transform.localScale.x / 2, plane.transform.position.x + plane.transform.localScale.x / 2);
		float randomY = Random.Range(plane.transform.position.y - plane.transform.localScale.y / 2, plane.transform.position.y + plane.transform.localScale.y / 2);
		float randomZ = Random.Range(plane.transform.position.y - plane.transform.localScale.z / 2, plane.transform.position.y + plane.transform.localScale.z / 2);

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
		maxNoOfWaves = level + 2;
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
		Debug.Log("Starting wave " + wave.ToString());
		int enemiesForCurrentWave = wave + level + 1; //(int)System.Math.Round(wave + level + 1);
		waveTimer.Duration = 25 * enemiesForCurrentWave / level;
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
		GameObject enemyObject;
		int randomNumber = Random.Range(0, 2);
		if (randomNumber == 0)
        {
			enemyObject = EnemyPool.instance.GetPooledObject();
		} 
		else
        {
			enemyObject = VampirePool.instance.GetPooledObject();
		}
		
		if (enemyObject != null)
        {
			enemyObject.transform.position = randomPosition;
			enemyObject.GetComponent<Stats.EnemyStats>().InitStats();
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

		Vector3 newVec = new Vector3(x, plane.transform.position.y + yBuffer, z);
		return newVec;
	}

}