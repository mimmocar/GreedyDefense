using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;

public class WaveSpawner : MonoBehaviour
{
	private ObjectManager om;

	private const string NEW_LINE = "\n";
	private const string SEMICOLON = ";";

	private string filePath = "File/Level1_waves";
	private string enemyPath = "EnemiesPrefab/";

	private enum SpawnState { SPAWNING, WAITING, COUNTING };

	private SpawnState state = SpawnState.COUNTING;

	[SerializeField] protected Transform[] spawnPoints;
	private Dictionary<string, Wave> waves = new Dictionary<string, Wave>();
	private float timeBetweenWaves;
	private int waveIndex = -1;
	private float countDown;

	private List<Transform> enemies = new List<Transform>();

	private int numWaves;
	private int numSpawnPoints;

	private int currentLevel;

	private void Start()
	{
		om = FindObjectOfType<ObjectManager>().GetComponent<ObjectManager>(); //implementare singleton

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

			string waveID = row[0];
			int enemiesNum = int.Parse(row[1], CultureInfo.InvariantCulture);
			float rate = float.Parse(row[2], CultureInfo.InvariantCulture);

			for (int j = 3; j < row.Length; j++)
			{
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

			Wave wave = new Wave(enemiesNum, rate, enemiesPrefabs);
			waves.Add(waveID, wave);
		}
		currentLevel = GameControl.LevelReached();
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

				// IMPLEMENTARE PUSH TO DATA
				float finalCurrency = om.Currency;
				int score = 0;
				if (finalCurrency < 100)
				{
					score = 1;
					PlayerPrefs.SetInt("starForLevel" + currentLevel, score);
				}
				else if (finalCurrency >= 100 && finalCurrency < 200)
				{
					score = 2;
					PlayerPrefs.SetInt("starForLevel" + currentLevel, score);
				}
				else if (finalCurrency >= 200)
				{
					score = 3;
					PlayerPrefs.SetInt("starForLevel" + currentLevel, score);
				}
				else
					PlayerPrefs.SetInt("starForLevel" + currentLevel, score);

				PlayerPrefs.SetInt("levelReached", currentLevel++);

				GameControl.SetGameStateWon();
				Messenger<int>.Broadcast(GameEvent.LEVEL_WON, score);

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
		countDown = timeBetweenWaves;
		Wave[] waveSpawnPoints = new Wave[numSpawnPoints];
		int maxSpCount = 0;
		float waveRate = 0;

		for (int i = 0; i < numSpawnPoints; i++)
		{
			waveIndex++;
			waveSpawnPoints[i] = waves.ElementAt(waveIndex).Value;
			if (waveSpawnPoints[i].GetEnemies().Count > maxSpCount)
				maxSpCount = waveSpawnPoints[i].GetEnemies().Count;
		}

		for (int i = 0; i <= maxSpCount; i++)
		{
			for (int j = 0; j < numSpawnPoints; j++)
			{
				if (i >= waveSpawnPoints[j].GetEnemies().Count)
				{
					continue;
				}
				else
				{
					waveRate = waveSpawnPoints[j].WaveRate;
					Transform enemy = waveSpawnPoints[j].GetEnemies()[i];
					SpawnEnemy(spawnPoints[j], enemy);
				}
			}
			yield return new WaitForSeconds(1.0f / waveRate);
		}

	}

	private void SpawnEnemy(Transform point, Transform enemy)
	{
		enemies.Add(Instantiate(enemy, point.position, point.rotation));
	}
}
