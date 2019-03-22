using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnTrash : MonoBehaviour {
	[SerializeField] private GameObject m_Mine;
	[SerializeField] private GameObject m_Fish;
	[SerializeField] private GameObject m_Anchor;
    [SerializeField] private GameObject m_Shark;

	private List<GameObject> m_Mines;
	private List<GameObject> m_Fishes;
	private List<GameObject> m_Anchors;

	// bounds for the mine and fish objects
	[SerializeField] private float m_MinX;
	[SerializeField] private float m_MaxX;
	[SerializeField] private float m_MinY;
	[SerializeField] private float m_MaxY;

	// bounds for the anchor
	[SerializeField] private float m_AnchorMinX;
	[SerializeField] private float m_AnchorMaxX;
	[SerializeField] private float m_AnchorMinY;
	[SerializeField] private float m_AnchorMaxY;

	[SerializeField] private float[] fishTimer = { 0.5f, 3f };
	[SerializeField] private float[] mineTimer = { 1f, 4f };
	[SerializeField] private float[] anchorTimer = { 1f, 3f };
    private float sharkTimer = 0f;
    private bool firstShark = false;

	private float[] allTimes;

	private float m_LastAnchorTime;

    [SerializeField] private GameObject swimmerObject;
    [SerializeField] private GameObject m_sharkIndicator;
    private GameObject shark;
    private bool isSharkSpawned = false;
    private SharkInstance sharkSpawnInstance;
    private SwimmerCharacter2D swimmerCharacter;
    private Swimmer2DUserControl swimmerControl;
    private  Vector3 overrideStartingPosition = new Vector3(-10.21f, 0, 0);
    private GameObject SharkIndicator;

    float fishRand;
	float anchorRand;
	float mineRand;

    private void SpawnIndicator()
    {
        m_sharkIndicator.SetActive(true);
        //Vector3 targetScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 indicatorPosition = new Vector3(-7.5f, swimmerObject.transform.position.y, m_sharkIndicator.transform.position.z);
        m_sharkIndicator.transform.position = indicatorPosition;
    }

    private void DeSpawnIndicator()
    {
        m_sharkIndicator.SetActive(false);
    }

    // Use this for initialization
    void Start ()
	{
	    
		allTimes = new float[4];		

		fishRand = Random.Range (fishTimer [0], fishTimer [1]);
		anchorRand = Random.Range (fishTimer [0], fishTimer [1]);
		mineRand = Random.Range (fishTimer [0], fishTimer [1]);

		// spawn fish
		for(int i=0;i<Random.Range(1,3);i++)
			SpawnFish ();
		// spawn mine
		for(int i=0;i<Random.Range(1,3);i++)
			SpawnMine();

		SpawnAnchor();
        PoolShark();
        ResetSharkSpawnTime();
        swimmerCharacter = swimmerObject.GetComponent<SwimmerCharacter2D>();
    }

	// Update is called once per frame
	void Update () 
	{
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

	    if (sharkTimer < 75f && !isSharkSpawned)
	    {
	        SpawnIndicator();
        }

	    if (sharkTimer > 0f)
	    {
	        sharkTimer--;
        }

        //if (swimmerCharacter.FishCount > 1)
        //{
        //    firstShark = true;
        //}

	    if (sharkTimer <= 0f && !isSharkSpawned)
	    {
	        DeSpawnIndicator();
	        SpawnShark();
	    }

        if (isSharkSpawned)
        {
            StartSharkInstance();
        }
        
	}


	GameObject SpawnFish()
	{
		Vector3 newPos = RandomPosition ();
		GameObject temp = Instantiate (m_Fish, newPos,this.transform.rotation);
		return temp;
	}

	GameObject SpawnMine()
	{
		Vector3 newPos = RandomPosition ();
		GameObject temp = Instantiate (m_Mine, newPos,new Quaternion(0,0,0,0));
		return temp;
	}

	GameObject SpawnAnchor()
	{
		Vector3 newPos = RandomPositionAnchor ();
		GameObject temp = Instantiate (m_Anchor, newPos,this.transform.rotation);
		return temp;
	}

	Vector3 RandomPosition()
	{
		float x = Random.Range (m_MinX, m_MaxX);
		float y = Random.Range (m_MinY, m_MaxY);
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

	void RemoveOffScreenObject()
	{
		
	}
		
	void NewRandomTimers()
	{
		
	}

    void ResetSharkSpawnTime()
    {
        sharkTimer =500f;
    }

    void PoolShark()
    {
        shark = Instantiate(m_Shark, overrideStartingPosition, Quaternion.identity);
        sharkSpawnInstance = shark.GetComponent<SharkInstance>();
        isSharkSpawned = false;
        shark.SetActive(isSharkSpawned);
    }

    void SpawnShark()
    {
        isSharkSpawned = true;
        shark.SetActive(isSharkSpawned);
        //sharkSpawnInstance.ResetSharkStates();
        Debug.Log("Spawn Shark");
    }

    void StartSharkInstance()
    {
        //: Shark Spawn Instance
        //do
        //{
        if (isSharkSpawned)
        {
            //: approaching
            if (sharkSpawnInstance.SharkState < 1)
            {
                swimmerCharacter = swimmerObject.GetComponent<SwimmerCharacter2D>();
                sharkSpawnInstance.FindPlayerPosition(swimmerCharacter.PlayerPosition);
                sharkSpawnInstance.SpawnPreAttackState();
            }
            //: attacking
            if (sharkSpawnInstance.SharkState == 1)
            {
                sharkSpawnInstance.SpawnAttackState();
            }
            //: go away and defeated
            else if (sharkSpawnInstance.SharkState > 1)
            {
                //sharkSpawnInstance.SpawnGoAwayState();
                DeSpawnShark();
                ResetSharkSpawnTime();
            }

        }
        //if (isSharkSpawned)
        //{
        //    switch (sharkSpawnInstance.SharkState)
        //    {
        //        case 0:
        //            //: approaching
        //            swimmerCharacter = swimmerObject.GetComponent<SwimmerCharacter2D>();
        //            sharkSpawnInstance.FindPlayerPosition(swimmerCharacter.PlayerPosition);
        //            sharkSpawnInstance.SpawnPreAttackState();
        //            break;
        //        case 1:
        //            //: attacking
        //            sharkSpawnInstance.SpawnAttackState();
        //            break;
        //        case 2:
        //            //: go away and defeated
        //            //sharkSpawnInstance.SpawnGoAwayState();
        //            DeSpawnShark();
        //            //ResetSharkSpawnTime();
        //            break;
        //    }
        //}
        //} while (isSharkSpawned);
    }

    public void SharkTakeDamage()
    {
        shark.GetComponent<SharkInstance>().SharkHealth--;
    }

    public void SharkReturnDamage()
    {
        shark.GetComponent<SharkInstance>().SharkHealth = 2;
    }

    public void DeSpawnShark()
    {
        isSharkSpawned = false;
        sharkSpawnInstance.SharkState = 0;
        shark.SetActive(isSharkSpawned);
        Debug.Log("Spawn Shark");
    }
}
