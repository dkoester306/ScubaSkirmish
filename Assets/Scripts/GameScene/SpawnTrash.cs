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

    public List<GameObject> m_Mines;
    private List<GameObject> m_Fishes;
    public static List<GameObject> m_Anchors;

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

    [SerializeField] private float[] fishTimer = {0.3f, 2.5f};
    [SerializeField] private float[] mineTimer = {.3f, 2.5f};
    [SerializeField] private float[] anchorTimer = {0.2f, 2f};
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
    private bool spawnChange = false;
    private int spawnChangeValue = 0;
    private int postcheck = 0;

    float fishRand;
    float anchorRand;
    float mineRand;

    // Use this for initialization
    void Start()
    {
        // 20 Change Spawn Rates

        swimmerCharacter = swimmerObject.GetComponent<SwimmerCharacter2D>();
        allTimes = new float[4];
        m_Anchors = new List<GameObject>();

        fishRand = Random.Range(fishTimer[0], fishTimer[1]);
        anchorRand = Random.Range(anchorTimer[0], anchorTimer[1]);
        mineRand = Random.Range(mineTimer[0], mineTimer[1]);
        ResetSharkSpawnTime(750f, 90f);

        //// spawn fish
        //for (int i = 0; i < Random.Range(1, 3); i++)
        //    SpawnFish();
        //// spawn mine
        //for (int i = 0; i < Random.Range(1, 3); i++)
        //    SpawnMine();
        //// spawn anchor
        //SpawnAnchor();

    }

    // Update is called once per frame
    void Update()
    {

        if (swimmerCharacter.FishCount >= 15 &&  !spawnChange)
        {
            spawnChange = true;
            spawnChangeValue = 1;

            fishTimer = new[] {0.5f, 2.2f};
            anchorTimer = new[] {0.4f, 2.3f};
            mineTimer = new[] {0.3f, 1.7f};

            fishRand = Random.Range(fishTimer[0], fishTimer[1]);
            anchorRand = Random.Range(anchorTimer[0], anchorTimer[1]);
            mineRand = Random.Range(mineTimer[0], mineTimer[1]);
        }

        if (swimmerCharacter.FishCount >= 35 && spawnChange)
        {
            spawnChange = true;
            spawnChangeValue = 2;

            fishTimer = new[] { 0.4f, 2f };
            anchorTimer = new[] { 0.5f, 2.3f };
            mineTimer = new[] { 0.3f, 1.75f };

            fishRand = Random.Range(fishTimer[0], fishTimer[1]);
            anchorRand = Random.Range(anchorTimer[0], anchorTimer[1]);
            mineRand = Random.Range(mineTimer[0], mineTimer[1]);
        }


        if (spawnChange)
        {

            // if timer goes over, spawn new fish
            if (Time.time >= allTimes[0] + fishRand)
            {
                SpawnFish();
                fishRand = Random.Range(fishTimer[0], fishTimer[1]);
                allTimes[0] = Time.time;
            }

            // spawn new mine if timer runs over
            if (Time.time >= allTimes[1] + fishRand)
            {
                SpawnMine();
                mineRand = Random.Range(mineTimer[0], mineTimer[1]);
                allTimes[1] = Time.time;
            }

            // spawn the anchors if timer goes over
            if (Time.time >= allTimes[2] + fishRand)
            {
                SpawnAnchor();
                anchorRand = Random.Range(anchorTimer[0], anchorTimer[1]);
                allTimes[2] = Time.time;
            }
        }
        else
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
                mineRand = Random.Range(mineTimer[0], mineTimer[1]);
                allTimes[1] = Time.time;
            }

            // spawn the anchors if timer goes over
            if (Time.time >= allTimes[2] + anchorRand)
            {
                SpawnAnchor();
                anchorRand = Random.Range(anchorTimer[0], anchorTimer[1]);
                allTimes[2] = Time.time;
            }
        }
        

        #region SharkUpdateConditionals

        // Shark Timer
        if (sharkTimer > 0f)
        {
            sharkTimer--;
        }

        // Spawn Shark
        if (sharkTimer <= 0f && !isSharkSpawned)
        {
            PoolShark();
            SpawnShark();
        }

        // Continue Shark Instance if Spawned
        if (isSharkSpawned)
        {
            StartSharkInstance();
        }

        // Reset Shark Spawn Timer
        if (despawnsharkTimer <= 0f)
        {
            ResetSharkSpawnTime(sharkInstance.SharkState == 3 ? 1500f : 500f, 90f);
            DeSpawnShark();
        }

        #endregion
    }

    GameObject SpawnFish()
    {
        Vector3 newPos = RandomPositionFish();
        GameObject tempFish = ObjectPooler.sharedInstance.GetPoolObject("Fish");
        tempFish.transform.position = newPos;
        tempFish.SetActive(true);
        return tempFish;
    }

    GameObject SpawnMine()
    {
        Vector3 newPos = RandomPositionMine();
        GameObject tempMine = ObjectPooler.sharedInstance.GetPoolObject("Mine");
        tempMine.transform.position = newPos;
        tempMine.SetActive(true);
        return tempMine;
    }

    GameObject SpawnAnchor()
    {
        Vector3 newPos = RandomPositionAnchor();
        GameObject tempAnchor = ObjectPooler.sharedInstance.GetPoolObject("Anchor");
        tempAnchor.transform.position = newPos;
        tempAnchor.SetActive(true);
        m_Anchors.Add(tempAnchor);
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

    /// <summary>
    /// Reset Shark Spawn Time and DeSpawn Timer
    /// </summary>
    void ResetSharkSpawnTime(float sharktimer, float despawnTimer)
    {
        sharkTimer = sharktimer;
        despawnsharkTimer = despawnTimer;
    }

    /// <summary>
    /// Create one new instance of a Shark
    /// </summary>
    void PoolShark()
    {
        shark = ObjectPooler.sharedInstance.GetPoolObject("Shark");
        sharkInstance = shark.GetComponent<Shark>();
        sharkInstance.GetWorldCanvas(ParentCanvas);
    }

    /// <summary>
    /// Activate the instance of the Shark
    /// </summary>
    void SpawnShark()
    {
        isSharkSpawned = true;
        shark.SetActive(isSharkSpawned);
    }

    public Canvas ParentCanvas;
    /// <summary>
    /// Instance used to control the loop of the Shark
    /// PreAttack, Attack, PostAttack are all instantiated here
    /// </summary>
    void StartSharkInstance()
    {
        if (!isSharkSpawned) return;

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
            DeSpawnIndicator();
            if (postcheck == 0)
            {
                sharkInstance.SpawnGoAwayState();
                postcheck = 1;
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
        shark.SetActive(isSharkSpawned);
        sharkInstance.SharkState = 0;
        postcheck = 0;
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