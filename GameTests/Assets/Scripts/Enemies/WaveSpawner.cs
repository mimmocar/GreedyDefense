using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

public class WaveSpawner : MonoBehaviour {

	private const string NEW_LINE = "\n";
	private const string SEMICOLON = ";";

	private string filePath = "File/Level1_waves";
	private string enemyPath = "EnemiesPrefab/";

	private enum SpawnState { SPAWNING, WAITING, COUNTING };

	private SpawnState state = SpawnState.COUNTING;

	[SerializeField] protected Transform[] spawnPoints;
	private List<Wave> waves = new List<Wave>();
	private List<float> timeBetweenWaves = new List<float>();
	private int waveIndex = -1;
	private float countDown;

	private List<Transform> enemies = new List<Transform>();

	private void Start()
	{
		GameObject enemy;

		TextAsset data = Resources.Load<TextAsset>(filePath);	//Presuppone che il file sia in Asset/Resources
		string[] lines = data.text.Split(NEW_LINE.ToCharArray());
		
		for (int i = 0; i < lines.Length; i++)
		{
			string enemyPrefab = "";

			string[] row = lines[i].Split(SEMICOLON.ToCharArray());

			timeBetweenWaves.Add(float.Parse(row[0]));

			int enemiesNum = int.Parse(row[1]);
			float rate = float.Parse(row[2]);
			string enemyType = row[3];

			if (enemyType.Equals("B"))
				enemyPrefab = enemyPath + "BarbarianAI";
			else if (enemyType.Equals("D"))
				enemyPrefab = enemyPath + "DragonSoulEaterBlueHPAI";
			else if (enemyType.Equals("M"))
				enemyPrefab = enemyPath + "MonsterAI";
			else
				Debug.LogError("Error reading waves file: illegal enemy prefab id");
			enemy = Resources.Load<GameObject>(enemyPrefab);

			Wave wave = new Wave(enemiesNum, rate, enemy.transform);
			waves.Add(wave);
		}
		countDown = timeBetweenWaves[0];
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
			yield return new WaitWhile(EnemyisAlive);

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

	private bool EnemyisAlive()
	{
		// Uses Linq to filter out null (previously detroyed) entries
		enemies = enemies.Where(e => e != null).ToList();

		return enemies.Count > 0;
	}

	private IEnumerator SpawnWave()
	{
		waveIndex++;
		countDown = timeBetweenWaves[waveIndex];
	
		Wave wave = waves[waveIndex];
		for (int i = 0; i < wave.WaveCount; i++)
		{
			for (int j = 0; j < spawnPoints.Length; j++)
			{
				SpawnEnemy(spawnPoints[j], wave.Enemy);
			}
			yield return new WaitForSeconds(1.0f/wave.WaveRate);
		}
		
	}

	private void SpawnEnemy(Transform point, Transform enemy)
	{
		enemies.Add(Instantiate(enemy, point.position, point.rotation));
	}
}
