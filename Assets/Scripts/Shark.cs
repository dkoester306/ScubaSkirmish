using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class Shark : MonoBehaviour
{
    private BoxCollider2D[] boxColliders;
    private float[] _mMaxX;
    private float[] _mMaxY;
    private float[] _mMinX;
    private float[] _mMinY;
    private int numberofColliders = 2;

    private Vector3 startingPosition = default(Vector3);
    private Vector3 acceleration = default(Vector3);
    private Vector3 direction = default(Vector3);
    private Vector3 velocity = default(Vector3);
    private Vector3 position = default(Vector3);
    private Vector3 playerpositionRef;
    private float accelerationRate;
    private float maximumSpeed =.2f;
    private bool attackDoneSpan;
    private bool postattackDoneSpan;
    private bool preattackDoneSpan;

    private SpriteRenderer sharkSpriteRenderer;
    private Animator sharkAnimator;
    private AudioSource sharkAudioSource;
    private int sharkState = 0;
    private int sharkHealth = 3;
    private bool sharkDamage;
    private bool sharkDamaged;
    private bool flip;
    private bool start;
    private bool preattackStart;
    [SerializeField]
    private float ienumeratorTimeConstant = 1.5f;
    [SerializeField]
    private float speedConstant = .3f;

    public int SharkState
    {
        get { return sharkState; }
        set { sharkState = value; }
    }

    public int SharkHealth { get { return sharkHealth; } set { sharkHealth = value; } }

    public Vector3 StartingPosition
    {
        get { return startingPosition; }
    }

    public bool Flip
    {
        get { return flip; }
    }

    // Start is called before the first frame update
    private void Start()
    {
        startingPosition = new Vector3(-10.21f, 0, 0);
        accelerationRate = .5f * Time.deltaTime;
        sharkState = 0;
        sharkHealth = 3;
        start = true;
        preattackStart = true;
    }

    public void ResetSharkHealth()
    {
        sharkHealth = 3;
    }

    public void ResetSharkStates()
    {
        //health = 3;
        sharkState = 0;
        velocity = default(Vector3);
        preattackDoneSpan = true;
        attackDoneSpan = true;
        postattackDoneSpan = true;
        sharkDamaged = false;
        sharkDamage = false;
        start = true;
        preattackStart = true;
        
        int rand = Random.Range(0,2);
        if (rand == 1)
        {
            flip = true;
            sharkSpriteRenderer.flipX = false;
        }
        else
        {
            flip = false;
            sharkSpriteRenderer.flipX = true;
        }
        if (flip)
        {
            startingPosition.x = 10.21f;
        }
        else
        {
            startingPosition = new Vector3(-10.21f, 0, 0);
        }
        transform.position = startingPosition;
    }

    public void GetComponentRefs()
    {
        sharkAnimator = gameObject.GetComponent<Animator>();
        sharkAudioSource = gameObject.GetComponent<AudioSource>();
        sharkSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void GetBoxColliders()
    {
        boxColliders = new BoxCollider2D[numberofColliders];
        for (int i = 0; i < numberofColliders; i++)
        {
            boxColliders[i] = gameObject.GetComponents<BoxCollider2D>()[i];
        }

        _mMinX = new float[2];
        _mMaxX = new float[2];
        _mMinY = new float[2];
        _mMaxY = new float[2];

        for (int i = 0; i < numberofColliders; i++)
        {
            _mMinX[i] = boxColliders[i].bounds.min.x;
            _mMaxX[i] = boxColliders[i].bounds.max.x;
            _mMinY[i] = boxColliders[i].bounds.min.y;
            _mMaxY[i] = boxColliders[i].bounds.max.y;
        }
    }

    // Checks if these gameobjects are intersecting
    // will retrieve both sprite renderers
    // return a true statement
    private void CheckIntersect()
    {
        // Outside Collider
        bool maxXOut = _mMinX[1] < playerpositionRef.x;
        bool minXOut = _mMaxX[1] > playerpositionRef.x;
        bool maxYOut = _mMinY[1] < playerpositionRef.y;
        bool minYOut = _mMaxY[1] > playerpositionRef.y;

        bool playerAttack = GameObject.Find("Swimmer").GetComponent<Swimmer2DUserControl>().Attack;
        if (maxXOut && minXOut && maxYOut && minYOut && playerAttack)
        {
            sharkDamage = true;
            SharkHealth--;
            StartCoroutine(SpawnDamagedState());
            Debug.Log("Hit Outside Box");
            Debug.Log("Shark Health " + SharkHealth);
        }

        // Inner Collider
        bool maxXIn = _mMinX[0] < playerpositionRef.x;
        bool minXIn = _mMaxX[0] > playerpositionRef.x;
        bool maxIn = _mMinY[0] < playerpositionRef.y;
        bool minIn = _mMaxY[0] > playerpositionRef.y;

        if (maxXIn && minXIn && maxIn && minIn && !sharkDamaged)
        {
            sharkDamaged = true;
            GameObject.Find("Swimmer").GetComponent<SwimmerCharacter2D>().PlayerHealth -= 2;
            Debug.Log("Hit Inside Box");
            Debug.Log("Interacted with Shark " +
                      GameObject.Find("Swimmer").GetComponent<SwimmerCharacter2D>().PlayerHealth);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            bool playerAttack = GameObject.Find("Swimmer").GetComponent<Swimmer2DUserControl>().Attack;
            if (playerAttack && attackDoneSpan == false && !sharkDamage)
            {
                sharkDamage = true;
                SharkHealth--;
                StartCoroutine(SpawnDamagedState());
                Debug.Log("Shark Health " + SharkHealth);
            }
            else if (attackDoneSpan == false && !sharkDamaged && !playerAttack)
            {
                sharkDamaged = true;
                GameObject.Find("Swimmer").GetComponent<Swimmer2DUserControl>().SwimmerDamaged();
                GameObject.Find("Swimmer").GetComponent<SwimmerCharacter2D>().PlayerHealth -= 2;
                Debug.Log("Interacted with Shark " +
                          GameObject.Find("Swimmer").GetComponent<SwimmerCharacter2D>().PlayerHealth);
            }
        }
    }

    public void FindPlayerPosition(Vector3 playerPosition)
    {
        float clampY = Mathf.Clamp(playerPosition.y, -5, 2.5f);
        Vector3 newplayerPosition = new Vector3(playerPosition.x, clampY);
        playerpositionRef = newplayerPosition;
    }

    // Update is called once per frame
    private void Update()
    {
        //: player attack
        if (sharkHealth <= 0)
        {
            InstantGoAwayState();
            ResetSharkHealth();
        }

        //: move shark forward
        if (preattackDoneSpan == false && sharkState == 0)
        {
            StartSharkMovement(1f);
        }

        //: 
        if (attackDoneSpan == false && sharkState == 1)
        {
            GetBoxColliders();
            //CheckIntersect();
        }

        //: works to move shark forward
        if (attackDoneSpan == false && sharkState == 1)
        {
            StartSharkMovement(1f);
        }

        if (postattackDoneSpan == false && (sharkState == 2 || sharkState == 3))
        {
            StartSharkMovement(0f);
            //var shootVector = new Vector3(0, 0, 0);
            //position = shootVector;
            //transform.position = position;
        }
    }

    #region Shark States

    public void SpawnPreAttackState()
    {

        GetComponentRefs();
        if (preattackStart)
        {
            ResetSharkStates();
            sharkAnimator.SetBool("preattack", true);
            preattackStart = false;
            preattackDoneSpan = false;
            sharkSpriteRenderer.sortingLayerName = "BehindBackground";
            StartCoroutine(DeSpawnPreAttackState());
        }
    }

    public IEnumerator DeSpawnPreAttackState()
    {
        yield return new WaitForSeconds(ienumeratorTimeConstant);
        sharkAnimator.SetBool("preattack", false);
        sharkAnimator.SetBool("attack", true);
        preattackDoneSpan = true;
        sharkState = 1;
    }

    public void SpawnAttackState()
    {
        sharkAnimator.SetBool("attack", true);

        if (start)
        {
            Vector3 sharkStartPosition = startingPosition;
            Vector3 newSharkPosition = Vector3.zero;

            if (flip)
            {
                newSharkPosition = new Vector3(sharkStartPosition.x, playerpositionRef.y, playerpositionRef.z);
            }
            else
            {
                newSharkPosition = new Vector3(sharkStartPosition.x, playerpositionRef.y, playerpositionRef.z);
            }
            
            position = newSharkPosition;
            transform.position = position;

            start = false;
            attackDoneSpan = false;
            sharkSpriteRenderer.sortingLayerName = "Shark";
            StartCoroutine(DeSpawnAttackState());
        }
    }

    public IEnumerator DeSpawnAttackState()
    {
        yield return new WaitForSeconds(ienumeratorTimeConstant);
        sharkAnimator.SetBool("attack", false);
        sharkAnimator.SetBool("postattack", true);
        attackDoneSpan = true;
        sharkState = 2;
        start = true;
        preattackStart = true;
    }

    public IEnumerator SpawnDamagedState()
    {
        sharkAnimator.SetBool("damaged", true);
        yield return new WaitForSeconds(ienumeratorTimeConstant);
        sharkAnimator.SetBool("postattack", true);
        attackDoneSpan = true;
        sharkState = 2;
        start = true;
        preattackStart = true;
    }

    public void SpawnGoAwayState()
    {
        postattackDoneSpan = false;
        sharkSpriteRenderer.sortingLayerName = "BehindBackground";
        StartCoroutine(DeSpawnGoAwayState());
    }

    public IEnumerator DeSpawnGoAwayState()
    {
        //sharkAnimator.SetBool("damaged", true);
        yield return new WaitForSeconds(ienumeratorTimeConstant);
        sharkAnimator.SetBool("postattack", false);
        postattackDoneSpan = true;
        start = true;
        preattackStart = true;
        sharkState = 0;
    }

    public void InstantGoAwayState()
    {
        sharkAnimator.SetBool("postattack", true);
        sharkSpriteRenderer.sortingLayerName = "BehindBackground";
        attackDoneSpan = true;
        postattackDoneSpan = false;
        sharkState = 3;
        start = true;
        preattackStart = true;
    }

    //: works to move shark in a certain Direction
    public void StartSharkMovement(float directionf)
    {
        // check direction float
        if (directionf > 0f)
        {
            //set direction
            direction = new Vector3(directionf, 0, 0);

            // accelerationRate

            // Acceleration = direction * accelerationRate
            acceleration = direction;

            // Vector += Acceleration
            velocity += acceleration;

            // Velocity = Clamp Velocity, MaxSpeed
            velocity = Vector3.ClampMagnitude(velocity, maximumSpeed);

            // set the x Velocity to the Shark current position
            position = new Vector3(velocity.x, 0, 0);
        }
        else
        {
            Vector3 shootVector = Vector3.zero;
            position = shootVector;
            transform.position = position;
        }

        if (flip)
        {
            transform.position -= position;
        }
        else
        {
            transform.position += position;
        }
    }

    #endregion



}