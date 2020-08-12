using UnityEngine;

public class Wave {

	private Transform enemy;
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

	public Transform Enemy
	{
		get
		{
			return enemy;
		} 
		set
		{
			enemy = value;
		}
	}

	public Wave(int c, float r, Transform e)
	{
		WaveCount = c;
		WaveRate = r;
		Enemy = e;
	}

	public override string ToString()
	{
		return "Number of enemies: " + WaveCount + "; Enemies rate: "+ WaveRate + "; Enemy : " + Enemy.name;
	}
}
