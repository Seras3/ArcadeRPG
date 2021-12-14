using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaveHandler : MonoBehaviour
{

	[SerializeField] private bool enableSpawn = true;
	// needed for spawning
	[SerializeField]
	GameObject spawnObject;

	[SerializeField]
	GameObject plane;

	// spawn control
	const float MinSpawnDelay = 1;
	const float MaxSpawnDelay = 1;

	const float yBuffer = 1;
	Timer spawnTimer;
	Timer waveTimer;
	int wave;

	// spawn location support
	float randomX;
	float randomY;
	float randomZ;

	// maximum number of enemies
	public int noOfEnemies = 0;
	private int maxNoOfEnemies;
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
			startWave();
		}

		if (deadEnemies == maxNoOfEnemies)
		{
			Debug.Log("All enemies are dead.");
			startWave();
		}
	}

	void startWave()
	{
		wave++;
		// start wave timer
		waveTimer.Duration = wave * 5;
		waveTimer.Run();
		maxNoOfEnemies = noOfEnemies + 1 * wave;
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
				Debug.Log("IDK what happened.");
				break;
		}

		Vector3 newVec = new Vector3(x, plane.transform.position.y + yBuffer, z);
		return newVec;


	}

}
