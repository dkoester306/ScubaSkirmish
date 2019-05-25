using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class SharkInstance : MonoBehaviour
{
    private bool attackDoneSpan;
    private BoxCollider2D[] boxColliders;
    private float[] _mMaxX;
    private float[] _mMaxY;
    private float[] _mMinX;
    private float[] _mMinY;
    private int numberofColliders = 2;
    private Vector3 startingPosition;
    private Vector3 resetMovingPosition;
    private Vector3 playerpositionRef;
    private Vector3 position = default(Vector3);
    private Vector3 acceleration = default(Vector3);
    private Vector3 direction = default(Vector3);
    private Vector3 velocity = default(Vector3);
    private float accelerationRate;
    float maximumSpeed =.2f;
    private bool postattackDoneSpan;
    private bool preattackDoneSpan;

    private Animator sharkAnimator;
    private AudioSource sharkAudioSource;
    private SpriteRenderer sharkSpriteRenderer;
    private int sharkState = 0;
    private int sharkHealth = 0;
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

    public int SharkHealth { get; set; }

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
        resetMovingPosition = new Vector3(-10.21f, 0, 0);
        accelerationRate = .5f * Time.deltaTime;
        sharkState = 0;
        SharkHealth = 3;
        start = true;
        preattackStart = true;
    }

    public void ResetSharkHealth()
    {
        SharkHealth = 3;
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
            startingPosition = new Vector3(10.21f, 0, 0);
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
        for (var i = 0; i < numberofColliders; i++)
        {
            boxColliders[i] = gameObject.GetComponents<BoxCollider2D>()[i];
        }

        _mMinX = new float[2];
        _mMaxX = new float[2];
        _mMinY = new float[2];
        _mMaxY = new float[2];

        for (var i = 0; i < numberofColliders; i++)
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
        // Inner Collider
        var maxXIn = _mMinX[0] < playerpositionRef.x;
        var minXIn = _mMaxX[0] > playerpositionRef.x;
        var maxIn = _mMinY[0] < playerpositionRef.y;
        var minIn = _mMaxY[0] > playerpositionRef.y;

        if (maxXIn && minXIn && maxIn && minIn && !sharkDamaged)
        {
            GameObject.Find("Swimmer").GetComponent<SwimmerCharacter2D>().PlayerHealth--;
            GameObject.Find("Swimmer").GetComponent<SwimmerCharacter2D>().PlayerHealth--;
            sharkDamaged = true;
            Debug.Log("Hit Inside Box");
            Debug.Log("Interacted with Shark " +
                      GameObject.Find("Swimmer").GetComponent<SwimmerCharacter2D>().PlayerHealth);
        }

        // Outside Collider
        var maxXOut = _mMinX[1] < playerpositionRef.x;
        var minXOut = _mMaxX[1] > playerpositionRef.x;
        var maxYOut = _mMinY[1] < playerpositionRef.y;
        var minYOut = _mMaxY[1] > playerpositionRef.y;

        var playerAttack = GameObject.Find("Swimmer").GetComponent<Swimmer2DUserControl>().Attack;
        if (maxXOut && minXOut && maxYOut && minYOut && playerAttack)
        {
            SharkHealth--;
            StartCoroutine(SpawnDamagedState());
            sharkDamage = true;
            Debug.Log("Hit Outside Box");
            Debug.Log("Shark Health " + SharkHealth);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            var playerAttack = GameObject.Find("Swimmer").GetComponent<Swimmer2DUserControl>().Attack;
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
                GameObject.Find("Swimmer").GetComponent<SwimmerCharacter2D>().PlayerHealth -= 2;
                Debug.Log("Interacted with Shark " +
                          GameObject.Find("Swimmer").GetComponent<SwimmerCharacter2D>().PlayerHealth);
            }
        }
    }

    public void FindPlayerPosition(Vector3 playerPosition)
    {
        playerpositionRef = playerPosition;
    }

    // Update is called once per frame
    private void Update()
    {
        //: works to move shark forward
        if (preattackDoneSpan == false && sharkState == 0)
        {
            StartSharkMovement();
        }

        //transform.position = position
        if (attackDoneSpan == false && sharkState == 1)
        {
            GetBoxColliders();
            //CheckIntersect();
        }

        //: works to move shark forward
        if (attackDoneSpan == false && sharkState == 1)
        {
            StartSharkMovement();
        }

        if (postattackDoneSpan == false && sharkState == 2)
        {
            var shootVector = new Vector3(0, 0, 0);
            position = shootVector;
            transform.position = position;
        }

        //: player attack
        //if (sharkDamaged && attackDoneSpan == false && SharkState == 1)
        if (SharkHealth <= 0)
        {
            InstantGoAwayState();
            ResetSharkHealth();
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
            //: Call Find Player Position (Called Already)
            
            //: Call Animation from Animator ( Background Swimming )
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
            var sharkStartPosition = startingPosition;

            Vector3 newSharkPosition;
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

        //SpawnSharkAttackState();

        //attackDoneSpan = true;
        //attackDoneSpan = true;
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
        Debug.Log("Ended PostAttack");
        sharkAnimator.SetBool("postattack", false);
        postattackDoneSpan = true;
        start = true;
        preattackStart = true;
        sharkState = 0;
        Debug.Log("SharkEventGoAway");
    }

    public void InstantGoAwayState()
    {
        sharkAnimator.SetBool("postattack", true);
        sharkSpriteRenderer.sortingLayerName = "BehindBackground";
        attackDoneSpan = true;
        postattackDoneSpan = false;
        sharkState = 2;
        start = true;
        preattackStart = true;
    }

    //: works to move shark forward
    //: Can be sued for:
    //@ PreAttack
    //@ Attack
    public void StartSharkMovement()
    {
        //set direction
        direction = new Vector3(1f, 0, 0);

        // accelerationRate

        // maxSpeed

        // Acceleration = direction * accelerationRate
        acceleration = direction;
        // Vector += Acceleration
        velocity += acceleration;
        // Velocity = Clamp Velocity, MaxSpeed
        velocity = Vector3.ClampMagnitude(velocity, maximumSpeed);

        position = new Vector3(velocity.x, 0, 0);

        if (Flip)
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