using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnTrash : MonoBehaviour
{
    //[SerializeField] private GameObject m_Mine;
    //[SerializeField] private GameObject m_Fish;
    //[SerializeField] private GameObject m_Anchor;
    //[SerializeField] private GameObject m_Shark;

    private List<GameObject> m_Mines;
    private List<GameObject> m_Fishes;
    private List<GameObject> m_Anchors;

    // bounds for the mine and fish objects
    [SerializeField] private float m_MineMinX;
    [SerializeField] private float m_MineMaxX;
    [SerializeField] private float m_MineMinY;
    [SerializeField] private float m_MineMaxY;

    [SerializeField] private float m_FishMinX;
    [SerializeField] private float m_FishMaxX;
    [SerializeField] private float m_FishMinY;
    [SerializeField] private float m_FishMaxY;

    // bounds for the anchor
    [SerializeField] private float m_AnchorMinX;
    [SerializeField] private float m_AnchorMaxX;
    [SerializeField] private float m_AnchorMinY;
    [SerializeField] private float m_AnchorMaxY;

    [SerializeField] private float[] fishTimer = {0.5f, 3f};
    [SerializeField] private float[] mineTimer = {1f, 4f};
    [SerializeField] private float[] anchorTimer = {1f, 3f};
    private float sharkTimer;
    private float despawnsharkTimer;
    private float[] allTimes;
    private float m_LastAnchorTime;

    [SerializeField] private GameObject swimmerObject;
    [SerializeField] private GameObject m_sharkIndicator;
    private SwimmerCharacter2D swimmerCharacter;
    private GameObject shark;
    private Shark sharkInstance;
    private bool isSharkSpawned = false;
    private bool firstShark = false;
    private bool resetShark = false;
    private int postcheck = 0;

    float fishRand;
    float anchorRand;
    float mineRand;

    // Use this for initialization
    void Start()
    {
        swimmerCharacter = swimmerObject.GetComponent<SwimmerCharacter2D>();
        allTimes = new float[4];

        fishRand = Random.Range(fishTimer[0], fishTimer[1]);
        anchorRand = Random.Range(fishTimer[0], fishTimer[1]);
        mineRand = Random.Range(fishTimer[0], fishTimer[1]);

        // spawn fish
        for (int i = 0; i < Random.Range(1, 3); i++)
            SpawnFish();
        // spawn mine
        for (int i = 0; i < Random.Range(1, 3); i++)
            SpawnMine();
        // spawn anchor
        SpawnAnchor();
        // spawn shark
        PoolShark();
    }

    // Update is called once per frame
    void Update()
    {
        // if timer goes over, spawn new fish
        if (Time.time >= allTimes[0] + fishRand)
        {
            SpawnFish();
            fishRand = Random.Range(fishTimer[0], fishTimer[1]);
            allTimes[0] = Time.time;
        }

        // spawn new mine if timer runs over
        if (Time.time >= allTimes[1] + mineRand)
        {
            SpawnMine();
            anchorRand = Random.Range(fishTimer[0], fishTimer[1]);
            allTimes[1] = Time.time;
        }

        // spawn the anchors if timer goes over
        if (Time.time >= allTimes[2] + anchorRand)
        {
            SpawnAnchor();
            mineRand = Random.Range(fishTimer[0], fishTimer[1]);
            allTimes[2] = Time.time;
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
        Vector3 newPos = RandomPositionFish();
        //GameObject temp = Instantiate(m_Fish, newPos, this.transform.rotation);
        //! Object Pooling
        GameObject tempFish = ObjectPooler.sharedInstance.GetPoolObject("Fish");
        tempFish.SetActive(true);
        tempFish.transform.position = newPos;
        return tempFish;
    }

    GameObject SpawnMine()
    {
        Vector3 newPos = RandomPositionMine();
        //GameObject temp = Instantiate(m_Mine, newPos, new Quaternion(0, 0, 0, 0));
        //! Object Pooling
        GameObject tempMine = ObjectPooler.sharedInstance.GetPoolObject("Mine");
        tempMine.SetActive(true);
        tempMine.transform.position = newPos;
        return tempMine;
    }

    GameObject SpawnAnchor()
    {
        Vector3 newPos = RandomPositionAnchor();
        //GameObject temp = Instantiate(m_Anchor, newPos, this.transform.rotation);
        //! Object Pooling
        GameObject tempAnchor = ObjectPooler.sharedInstance.GetPoolObject("Anchor");
        tempAnchor.SetActive(true);
        tempAnchor.transform.position = newPos;
        return tempAnchor;
    }

    Vector3 RandomPositionFish()
    {
        float x = Random.Range(m_FishMinX, m_FishMaxX);
        float y = Random.Range(m_FishMinY, m_FishMaxY);
        Vector3 newPos = new Vector3(x, y, 0);
        return newPos;
    }

    Vector3 RandomPositionMine()
    {
        float x = Random.Range(m_MineMinX, m_MineMaxX);
        float y = Random.Range(m_MineMinY, m_MineMaxY);
        Vector3 newPos = new Vector3(x, y, 0);
        return newPos;
    }

    Vector3 RandomPositionAnchor()
    {
        float x = Random.Range(m_AnchorMinX, m_AnchorMaxX);
        float y = Random.Range(m_AnchorMinY, m_AnchorMaxY);
        Vector3 newPos = new Vector3(x, y, 0);
        return newPos;
    }

    // Creates a random time amount for each object
    float RandomTime(float min, float max)
    {
        float rand = Random.Range(min, max);
        return rand;
    }

    void RemoveOffScreenObject()
    {
    }

    void NewRandomTimers()
    {
    }

    /// <summary>
    /// Reset Shark Spawn Time and DeSpawn Timer
    /// </summary>
    void ResetSharkSpawnTime(float sharktimer, float despawnTimer)
    {
        sharkTimer = sharktimer;
        despawnsharkTimer = despawnTimer;
        sharkInstance.SharkState = 0;
        postcheck = 0;
    }

    /// <summary>
    /// Create one new instance of a Shark
    /// </summary>
    void PoolShark()
    {
        Vector3 parentInstantiatePosition = new Vector3(-10.21f, 0, 0);
        //shark = Instantiate(m_Shark, parentInstantiatePosition, Quaternion.identity);
        //! Object Pooling
        shark = ObjectPooler.sharedInstance.GetPoolObject("Shark");
        isSharkSpawned = true;
        shark.SetActive(isSharkSpawned);
        sharkInstance = shark.GetComponent<Shark>();
        ResetSharkSpawnTime(500f, 90f);
    }

    /// <summary>
    /// Activate the instance of the Shark
    /// </summary>
    void SpawnShark()
    {
        isSharkSpawned = true;
        //! Object Pooling
        //gameObject.SetActive(false);
        shark.SetActive(isSharkSpawned);
    }

    /// <summary>
    /// Instance used to control the loop of the Shark
    /// PreAttack, Attack, PostAttack are all instantiated here
    /// </summary>
    void StartSharkInstance()
    {
        if (isSharkSpawned)
        {
            //: approaching
            if (sharkInstance.SharkState == 0)
            {
                sharkInstance.FindPlayerPosition(swimmerCharacter.PlayerPosition);
                StartCoroutine(SpawnIndicator());
                sharkInstance.SpawnPreAttackState();
            }

            //: attacking
            if (sharkInstance.SharkState == 1)
            {
                DeSpawnIndicator();
                sharkInstance.SpawnAttackState();
            }

            //: go away and defeated
            if (sharkInstance.SharkState == 2 || sharkInstance.SharkState == 3)
            {
                despawnsharkTimer--;
                if (postcheck == 0)
                {
                    sharkInstance.SpawnGoAwayState();
                    postcheck = 1;
                }
            }
        }
    }

    /// <summary>
    /// Action for Shark to Take Damage
    /// </summary>
    public void SharkTakeDamage()
    {
        shark.GetComponent<Shark>().SharkHealth--;
    }

    /// <summary>
    /// Reset Shark Health
    /// </summary>
    public void ResetSharkHealth()
    {
        shark.GetComponent<Shark>().SharkHealth = 3;
    }

    /// <summary>
    /// Deactivate Shark Instance
    /// </summary>
    public void DeSpawnShark()
    {
        isSharkSpawned = false;
        //! Object Pooling
        //gameObject.SetActive(false);
        shark.SetActive(isSharkSpawned);
        ResetSharkSpawnTime(sharkInstance.SharkState == 3 ? 10000f : 500f, 90f);
    }

    /// <summary>
    /// Spawn Shark Indicator at SwimmerX Location
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnIndicator()
    {
        Canvas parentCanvas = m_sharkIndicator.gameObject.GetComponentInParent<Canvas>();
        Vector3 targetScreenPoint = Camera.main.WorldToScreenPoint(swimmerObject.transform.position);
        Vector3 indicatorPosition = Vector3.zero;
        float canvasViewportMinX = parentCanvas.pixelRect.min.x;
        float canvasViewportMaxX = parentCanvas.pixelRect.max.x;

        if (sharkInstance.Flip)
        {
            indicatorPosition = new Vector3(canvasViewportMaxX - 50, targetScreenPoint.y,
                m_sharkIndicator.transform.position.z);
        }
        else
        {
            indicatorPosition = new Vector3(canvasViewportMinX + 50, targetScreenPoint.y,
                m_sharkIndicator.transform.position.z);
        }

        m_sharkIndicator.transform.position = indicatorPosition;
        yield return new WaitForSeconds(.3f);
        m_sharkIndicator.SetActive(true);
    }

    private void DeSpawnIndicator()
    {
        m_sharkIndicator.SetActive(false);
    }
}