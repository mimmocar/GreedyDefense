using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System;

public class WaveSpawner : MonoBehaviour
{

	private const string NEW_LINE = "\n";
	private const string SEMICOLON = ";";

	private string filePath = "File/Level1_waves";
	private string enemyPath = "EnemiesPrefab/";

	private enum SpawnState { SPAWNING, WAITING, COUNTING };

	private SpawnState state = SpawnState.COUNTING;

	[SerializeField] protected Transform[] spawnPoints;
	//private List<Wave> waves = new List<Wave>();
	private Dictionary<string, Wave> waves = new Dictionary<string, Wave>();
	//private List<float> timeBetweenWaves = new List<float>();
	private float timeBetweenWaves;
	private int waveIndex = -1;
	private float countDown;

	private List<Transform> enemies = new List<Transform>();

	private int numWaves;
	private int numSpawnPoints;

	private void Start()
	{
		//GameObject enemy;
		TextAsset data = Resources.Load<TextAsset>(filePath);   //Presuppone che il file sia in Asset/Resources
		string[] lines = data.text.Split(NEW_LINE.ToCharArray());

		numWaves = int.Parse(lines[0].Split(SEMICOLON.ToCharArray())[1], CultureInfo.InvariantCulture);
		numSpawnPoints = int.Parse(lines[1].Split(SEMICOLON.ToCharArray())[1], CultureInfo.InvariantCulture);
		countDown = float.Parse(lines[2].Split(SEMICOLON.ToCharArray())[1], CultureInfo.InvariantCulture);
		timeBetweenWaves = float.Parse(lines[3].Split(SEMICOLON.ToCharArray())[1], CultureInfo.InvariantCulture);

		for (int i = 5; i < lines.Length; i++)
		{
			List<GameObject> enemiesPrefabs = new List<GameObject>();
			string enemyPrefab = "";

			string[] row = lines[i].Split(SEMICOLON.ToCharArray());

			//timeBetweenWaves.Add(float.Parse(row[0], CultureInfo.InvariantCulture));

			string waveID = row[0];
			int enemiesNum = int.Parse(row[1], CultureInfo.InvariantCulture);
			float rate = float.Parse(row[2], CultureInfo.InvariantCulture);

			for (int j = 3; j < row.Length; j++)
			{
				//Debug.LogError(j + " " + row[j]);
				if (row[j].Equals("B"))
					enemyPrefab = enemyPath + "BarbarianAI";
				else if (row[j].Equals("D"))
					enemyPrefab = enemyPath + "DragonSoulEaterBlueHPAI";
				else if (row[j].Equals("M"))
					enemyPrefab = enemyPath + "MonsterAI";
				else if (!row[j].Equals("B") && !row[j].Equals("M") && !row[j].Equals("D"))
				{
					Debug.LogError("Error reading waves file: illegal enemy prefab id " + row[j]);
					continue;
				}
				enemiesPrefabs.Add(Resources.Load<GameObject>(enemyPrefab));
				enemyPrefab = "";
			}
			//string enemyType = row[3];

			//if (enemyType.Equals("B"))
			//	enemyPrefab = enemyPath + "BarbarianAI";
			//else if (enemyType.Equals("D"))
			//	enemyPrefab = enemyPath + "DragonSoulEaterBlueHPAI";
			//else if (enemyType.Equals("M"))
			//	enemyPrefab = enemyPath + "MonsterAI";
			//else
			//	Debug.LogError("Error reading waves file: illegal enemy prefab id");

			//enemy = Resources.Load<GameObject>(enemyPrefab);

			//Wave wave = new Wave(enemiesNum, rate, enemy.transform);
			Wave wave = new Wave(enemiesNum, rate, enemiesPrefabs);
			waves.Add(waveID, wave);
			//waves.Add(wave);
		}
		//countDown = timeBetweenWaves[0];
		StartCoroutine(RunSpawner());
	}

	// This replaces Update method
	private IEnumerator RunSpawner()
	{
		// First time waiting
		yield return new WaitForSeconds(countDown);

		while (true)
		{
			state = SpawnState.SPAWNING;

			// Do the spawning and at the same time wait until it's finished
			yield return SpawnWave();

			state = SpawnState.WAITING;

			// Wait until all enemies died (are destroyed)
			yield return new WaitWhile(EnemyIsAlive);

			// Ended level control
			if (waveIndex == waves.Count - 1)
			{
				Debug.Log("LEVEL FINISHED");
				StopCoroutine(RunSpawner());
				break;
			}


			state = SpawnState.COUNTING;

			// Wait next wave
			yield return new WaitForSeconds(countDown);
		}
	}

	private bool EnemyIsAlive()
	{
		// Uses Linq to filter out null (previously detroyed) entries
		enemies = enemies.Where(e => e != null).ToList();

		return enemies.Count > 0;
	}

	private IEnumerator SpawnWave()
	{
		//waveIndex++;
		//countDown = timeBetweenWaves[waveIndex];
		countDown = timeBetweenWaves;
		Wave[] waveSpawnPoints = new Wave[numSpawnPoints];
		//List<Transform> es = new List<Transform>();
		//int maxSpCount = 0;
		float waveRate = 0;
		int num = 0;

		for (int i = 0; i < numSpawnPoints; i++)
		{
			waveIndex++;
			waveSpawnPoints[i] = waves.ElementAt(waveIndex).Value;
			num = waveSpawnPoints[i].WaveCount;
			//if (sps[i].GetEnemies().Count > maxSpCount)
			//	maxSpCount = sps[i].GetEnemies().Count;
		}

		//Debug.LogError("MaxSpCount "+maxSpCount);

		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < numSpawnPoints; j++)
			{
				waveRate = waveSpawnPoints[j].WaveRate;
				Transform enemy = waveSpawnPoints[j].GetEnemies()[i];
				SpawnEnemy(spawnPoints[j], enemy);
			}
			yield return new WaitForSeconds(1.0f / waveRate);
		}

		//for (int i = 0; i <= maxSpCount; i++)
		//{
		//	for (int j = 0; j < numSpawnPoints; j++)
		//	{
		//		Debug.LogError("Dimension " + waveSpawnPoints[j].GetEnemies().Count);
		//		if(i > waveSpawnPoints[j].GetEnemies().Count)
		//		{
		//			continue;
		//		}
		//		else
		//		{
		//			waveRate = waveSpawnPoints[j].WaveRate;
		//			Transform enemy = waveSpawnPoints[j].GetEnemies()[i];
		//			es.Add(enemy);
		//			SpawnEnemy(spawnPoints[j], enemy);
		//		}
		//	}
		//	yield return new WaitForSeconds(1.0f / waveRate);
		//}

		//for (int i = 0; i < wave.WaveCount; i++)
		//{
		//	for (int j = 0; j < spawnPoints.Length; j++)
		//	{
		//		SpawnEnemy(spawnPoints[j], wave.Enemy);
		//	}
		//	yield return new WaitForSeconds(1.0f/wave.WaveRate);
		//}

	}

	private void SpawnEnemy(Transform point, Transform enemy)
	{
		enemies.Add(Instantiate(enemy, point.position, point.rotation));
	}
}
