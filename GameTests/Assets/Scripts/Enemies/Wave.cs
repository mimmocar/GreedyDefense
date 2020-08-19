using System.Collections.Generic;
using UnityEngine;

public class Wave
{

	private List<GameObject> enemies = new List<GameObject>();
	private int count;
	private float rate;

	public int WaveCount
	{
		get
		{
			return count;
		}
		set
		{
			count = value;
		}
	}

	public float WaveRate
	{
		get
		{
			return rate;
		}
		set
		{
			rate = value;
		}
	}

	public List<Transform> GetEnemies()
	{
		List<Transform> e = new List<Transform>();
		foreach (GameObject item in enemies)
		{
			e.Add(item.transform);
		}

		return e;
	}

	public Wave(int c, float r, List<GameObject> es)
	{
		WaveCount = c;
		WaveRate = r;
		enemies = es;
	}
}
