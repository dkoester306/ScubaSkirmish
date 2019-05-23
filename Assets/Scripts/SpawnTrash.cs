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
    private float despawnsharkTimer = 0f;
    private bool firstShark = false;

	private float[] allTimes;

	private float m_LastAnchorTime;

    [SerializeField] private GameObject swimmerObject;
    [SerializeField] private GameObject m_sharkIndicator;
    private GameObject shark;
    private bool isSharkSpawned = false;
    private bool resetShark = false;
    private SharkInstance sharkSpawnInstance;
    private SwimmerCharacter2D swimmerCharacter;
    private Swimmer2DUserControl swimmerControl;
    private  Vector3 parentInstantiatePosition = new Vector3(-10.21f, 0, 0);
    private GameObject SharkIndicator;
    int postcheck = 0;

    float fishRand;
	float anchorRand;
	float mineRand;

    private IEnumerator SpawnIndicator()
    {
        //Vector3 targetScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 indicatorPosition;
        if (sharkSpawnInstance.Flip)
        {
            indicatorPosition = new Vector3(7.5f, swimmerObject.transform.position.y, m_sharkIndicator.transform.position.z);
        }
        else
        {
            indicatorPosition = new Vector3(-7.5f, swimmerObject.transform.position.y, m_sharkIndicator.transform.position.z);
        }

        m_sharkIndicator.transform.position = indicatorPosition;
        yield return new WaitForSeconds(.3f);
        m_sharkIndicator.SetActive(true);
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

		//SpawnAnchor();
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

        #region SharkUpdateConditionals

	    if (sharkTimer > 0f)
	    {
	        sharkTimer--;
	    }
	    if (sharkTimer <= 0f && !isSharkSpawned)
	    {
	        SpawnShark();
	    }
	    if (isSharkSpawned)
	    {
	        StartSharkInstance();
	    }
	    if (despawnsharkTimer <= 0f)
	    {
	        DeSpawnShark();
	    }

        #endregion
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
        sharkTimer = 500f;
        despawnsharkTimer = 90f;
    }

    void PoolShark()
    {
        shark = Instantiate(m_Shark, parentInstantiatePosition, Quaternion.identity);
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
        if (isSharkSpawned)
        {
            //: approaching
            if (sharkSpawnInstance.SharkState == 0)
            {
                swimmerCharacter = swimmerObject.GetComponent<SwimmerCharacter2D>();
                sharkSpawnInstance.FindPlayerPosition(swimmerCharacter.PlayerPosition);
                StartCoroutine(SpawnIndicator());
                sharkSpawnInstance.SpawnPreAttackState();
            }
            //: attacking
            if (sharkSpawnInstance.SharkState == 1)
            {
                DeSpawnIndicator();
                sharkSpawnInstance.SpawnAttackState();
            }
            //: go away and defeated
            if (sharkSpawnInstance.SharkState == 2)
            {
                despawnsharkTimer--;
                if (postcheck == 0)
                {
                    sharkSpawnInstance.SpawnGoAwayState();
                    postcheck = 1;
                }
            }
        }
    }

    public void SharkTakeDamage()
    {
        shark.GetComponent<SharkInstance>().SharkHealth--;
    }

    // I don't know what this does
    public void SharkReturnDamage()
    {
        shark.GetComponent<SharkInstance>().SharkHealth = 2;
    }

    public void DeSpawnShark()
    {
        isSharkSpawned = false;
        shark.SetActive(isSharkSpawned);
        postcheck = 0;
        sharkSpawnInstance.SharkState = 0;
        ResetSharkSpawnTime();
        Debug.Log("DeSpawn Shark");
    }
}
