using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrash : MonoBehaviour {
	[SerializeField] private GameObject m_Mine;
	[SerializeField] private GameObject m_Fish;
	[SerializeField] private GameObject m_Anchor;

	public static int mineCount=0;
	public static int fishCount=0;
	public static int anchorCount=0;

	// bounds for thefish objects
	[Header("Fish Bounds")]
	[SerializeField] private float m_MinX;
	[SerializeField] private float m_MaxX;
	[SerializeField] private float m_MinY;
	[SerializeField] private float m_MaxY;

	// bounds for the anchor
	[Header("Anchor Bounds")]
	[SerializeField] private float m_AnchorMinX;
	[SerializeField] private float m_AnchorMaxX;
	[SerializeField] private float m_AnchorMinY;
	[SerializeField] private float m_AnchorMaxY;

// bounds for the mine
	[Header("Anchor Bounds")]
	[SerializeField] private float m_MineMinX;
	[SerializeField] private float m_MineMaxX;
	[SerializeField] private float m_MineMinY;
	[SerializeField] private float m_MineMaxY;

	[Header("Timers")]
	[SerializeField] private float[] fishTimer = { 0.5f, 3f };
	[SerializeField] private float[] mineTimer = { 1f, 4f };
	[SerializeField] private float[] anchorTimer = { 1f, 3f };

	private float[] allTimes;

	float fishRand;
	float anchorRand;
	float mineRand;

	// Use this for initialization
	void Start () 
	{
		allTimes = new float[3];		

		fishRand = Random.Range (fishTimer [0], fishTimer [1]);
		anchorRand = Random.Range (fishTimer [0], fishTimer [1]);
		mineRand = Random.Range (fishTimer [0], fishTimer [1]);

		// spawn fish
		for(int i=0;i<Random.Range(1,3);i++)
			SpawnFish ();
		// spawn mine
		for(int i=0;i<Random.Range(1,3);i++)
			SpawnMine();

		SpawnAnchor ();
	}

	// Update is called once per frame
	void Update () 
	{
		Debug.Log("Anchor Count: "+anchorCount);
		// if timer goes over, spawn new fish
		if (Time.time>=allTimes[0]+fishRand)
		{
			
			SpawnFish ();
			fishRand = Random.Range (fishTimer [0], fishTimer [1]);
			allTimes [0] = Time.time;
		}

		// spawn new mine if timer runs over
		if (Time.time>=allTimes[1]+mineRand)
		{
			SpawnMine ();
			anchorRand = Random.Range (fishTimer [0], fishTimer [1]);
			allTimes [1] = Time.time;
		}

		// spawn the anchors if timer goes over
		if (Time.time>=allTimes[2]+anchorRand)
		{
			SpawnAnchor ();
			mineRand = Random.Range (fishTimer [0], fishTimer [1]);
			allTimes [2] = Time.time;
		}
			
	}


	GameObject SpawnFish()
	{
		Vector3 newPos = RandomPosition ();
		GameObject temp = Instantiate (m_Fish, newPos,this.transform.rotation);
		fishCount++;
		return temp;
	}

	GameObject SpawnMine()
	{
		Vector3 newPos = RandomPosition(m_MineMinX,m_MineMaxX,m_MineMinY,m_MineMaxY);
		GameObject temp = Instantiate (m_Mine, newPos,new Quaternion(0,0,0,0));
		mineCount++;
		return temp;
	}

	GameObject SpawnAnchor()
	{
		Vector3 newPos = RandomPositionAnchor ();
		GameObject temp = Instantiate (m_Anchor, newPos,this.transform.rotation);
		anchorCount++;
		return temp;
	}

	Vector3 RandomPosition()
	{
		float x = Random.Range (m_MinX, m_MaxX);
		float y = Random.Range (m_MinY, m_MaxY);
		Vector3 newPos = new Vector3 (x, y, 0);
		return newPos;
	}

	Vector3 RandomPosition(float minX, float maxX, float minY, float maxY)
	{
		float x = Random.Range (minX, maxX);
		float y = Random.Range (minY, maxY);
		Vector3 newPos = new Vector3 (x, y, 0);
		return newPos;
	}

	Vector3 RandomPositionAnchor()
	{
		float x = Random.Range (m_AnchorMinX, m_AnchorMaxX);
		float y = Random.Range (m_AnchorMinY, m_AnchorMaxY);
		Vector3 newPos = new Vector3 (x, y, 0);
		return newPos;
	}




	// Creates a random time amount for each object
	float RandomTime(float min,float max)
	{
		float rand = Random.Range (min,max);
		return rand;
	}

	public static void RemoveFish()
	{
		fishCount--;
	}
	public static void RemoveFish(GameObject fish)
	{
		fishCount--;
		GameObject.Destroy(fish);
	}

	public static void RemoveAnchor()
	{
		anchorCount--;
	}
	public static void RemoveAnchor(GameObject anchor)
	{
		anchorCount--;
		GameObject.Destroy(anchor);
	}

	public static void RemoveMine()
	{
		mineCount--;
	}
	public static void RemoveMine(GameObject mine)
	{
		mineCount--;
		GameObject.Destroy(mine);
	}
}
