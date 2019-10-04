//using System.Collections;
//using System.Runtime.InteropServices;
//using UnityEngine;

//public class Shark : MonoBehaviour
//{
//    private Vector3 startingPosition = default(Vector3);
//    private Vector3 acceleration = default(Vector3);
//    private Vector3 direction = default(Vector3);
//    private Vector3 velocity = default(Vector3);
//    private Vector3 position = default(Vector3);
//    private Vector3 playerpositionRef;
//    private float accelerationRate;
//    private float maximumSpeed = .2f;
//    private bool attackDoneSpan;
//    private bool postattackDoneSpan;
//    private bool preattackDoneSpan;

//    private SpriteRenderer sharkSpriteRenderer;
//    private Animator sharkAnimator;
//    private AudioSource sharkAudioSource;
//    private int sharkState = 0;
//    private int sharkHealth = 3;
//    private bool sharkDamage;
//    private bool sharkDamaged;
//    private bool flip;
//    private bool start;
//    private bool transition;
//    private bool preattackStart;
//    private float ienumeratorTimeConstant = 2f;
//    private float speedConstant = .3f;

//    public int SharkState
//    {
//        get { return sharkState; }
//        set { sharkState = value; }
//    }

//    public int SharkHealth
//    {
//        get { return sharkHealth; }
//        set { sharkHealth = value; }
//    }

//    public Vector3 StartingPosition
//    {
//        get { return startingPosition; }
//    }

//    public bool Flip
//    {
//        get { return flip; }
//    }

//    // Start is called before the first frame update
//    private void Start()
//    {
//        accelerationRate = .5f * Time.deltaTime;
//        sharkState = 0;
//        sharkHealth = 3;
//        start = true;
//        preattackStart = true;
//    }

//    public void GetComponentRefs()
//    {
//        sharkAnimator = gameObject.GetComponent<Animator>();
//        sharkAudioSource = gameObject.GetComponent<AudioSource>();
//        sharkSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
//    }

//    private Canvas worldCanvas;
//    public void GetWorldCanvas(Canvas worldCanvas)
//    {
//        this.worldCanvas = worldCanvas;
//    }

//    private void SetStartingPosition()
//    {
//        Vector3 targetScreenPoint = Camera.main.ScreenToWorldPoint(worldCanvas.pixelRect.position);
//        if (flip)
//        {
//            float canvasViewportMaxX = ( -1 *targetScreenPoint.x);
//            startingPosition.x = canvasViewportMaxX;
//        }
//        else
//        {
//            float canvasViewportMinX = targetScreenPoint.x;
//            startingPosition.x = canvasViewportMinX;

//        }
//    }

//    public void ResetSharkHealth()
//    {
//        sharkHealth = 3;
//    }

//    public void ResetSharkStates()
//    {
//        //health = 3;
//        sharkState = 0;
//        velocity = default(Vector3);
//        preattackDoneSpan = true;
//        attackDoneSpan = true;
//        postattackDoneSpan = true;
//        sharkDamaged = false;
//        sharkDamage = false;
//        start = true;
//        preattackStart = true;

//        int rand = Random.Range(0, 2);
//        if (rand > 0)
//        {
//            flip = true;
//            sharkSpriteRenderer.flipX = false;
//        }
//        else
//        {
//            flip = false;
//            sharkSpriteRenderer.flipX = true;
//        }

//        SetStartingPosition();
//        transform.position = startingPosition;
//    }

//    // Checks if these gameobjects are intersecting
//    // will retrieve both sprite renderers
//    // return a true statement
//    private bool CheckIntersect(GameObject firstObject, GameObject secondObject)
//    {
//        SpriteRenderer firstSpriteR = firstObject.GetComponent<SpriteRenderer>();
//        SpriteRenderer secondSpriteR = secondObject.GetComponent<SpriteRenderer>();
//        bool intersect = false;

//        if (firstSpriteR.bounds.Intersects(secondSpriteR.bounds))
//        {
//            intersect = true;
//        }
//        else
//        {
//            intersect = false;
//        }

//        return intersect;
//    }

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.tag == "Player")
//        {
//            bool playerAttack = GameObject.Find("Swimmer").GetComponent<Swimmer2DUserControl>().Attack;
//            if (playerAttack && attackDoneSpan == false && !sharkDamaged)
//            {
//                sharkDamaged = true;
//                SharkHealth--;
//                StartCoroutine(SpawnDamagedState());
//                Debug.Log("Shark Health " + SharkHealth);
//            }
//            else if (attackDoneSpan == false && !sharkDamage && !playerAttack && !sharkDamaged)
//            {
//                sharkDamage = true;
//                GameObject.Find("Swimmer").GetComponent<Swimmer2DUserControl>().SwimmerDamaged();
//                GameObject.Find("Swimmer").GetComponent<SwimmerCharacter2D>().PlayerHealth -= 2;
//                GameAudio.PlaySwimmerDamaged();
//                Debug.Log("Interacted with Shark " +
//                          GameObject.Find("Swimmer").GetComponent<SwimmerCharacter2D>().PlayerHealth);
//            }
//        }
//    }

//    public void FindPlayerPosition(Vector3 playerPosition)
//    {
//        float clampY = Mathf.Clamp(playerPosition.y, -5, 2.5f);
//        Vector3 newplayerPosition = new Vector3(playerPosition.x, clampY);
//        playerpositionRef = newplayerPosition;
//    }

//    // Update is called once per frame
//    private void Update()
//    {
//        //: player attack
//        if (sharkHealth == 0)
//        {
//            InstantGoAwayState();
//            ResetSharkHealth();
//            return;
//        }

//        //: move shark forward
//        if (preattackDoneSpan == false && sharkState == 0)
//        {
//            StartSharkMovement(1f);
//        }

//        //: 
//        if (attackDoneSpan == false && sharkState == 1)
//        {
//            //CheckIntersect();
//        }

//        //: works to move shark forward
//        if (attackDoneSpan == false && sharkState == 1)
//        {
//            StartSharkMovement(1f);
//        }

//        if (postattackDoneSpan == false && (sharkState == 2 || sharkState == 3))
//        {
//            StartSharkMovement(0f);
//        }
//    }

//    #region Shark States

//    public void SpawnPreAttackState()
//    {
//        GetComponentRefs();
//        if (preattackStart)
//        {
//            ResetSharkStates();
//            sharkAnimator.SetBool("preattack", true);
//            preattackStart = false;
//            preattackDoneSpan = false;
//            sharkSpriteRenderer.sortingLayerName = "BehindBackground";
//            StartCoroutine(DeSpawnPreAttackState());
//        }
//    }

//    public IEnumerator DeSpawnPreAttackState()
//    {
//        yield return new WaitForSeconds(ienumeratorTimeConstant);
//        sharkAnimator.SetBool("preattack", false);
//        sharkAnimator.SetBool("attack", true);
//        preattackDoneSpan = true;
//        sharkState = 1;
//    }

//    public void SpawnAttackState()
//    {
//        sharkAnimator.SetBool("attack", true);

//        if (!start) return;
//        Vector3 sharkStartPosition = startingPosition;
//        Vector3 newSharkPosition = Vector3.zero;

//        if (flip)
//        {
//            newSharkPosition = new Vector3(sharkStartPosition.x, playerpositionRef.y, playerpositionRef.z);
//        }
//        else
//        {
//            newSharkPosition = new Vector3(sharkStartPosition.x, playerpositionRef.y, playerpositionRef.z);
//        }

//        position = newSharkPosition;
//        transform.position = position;

//        start = false;
//        attackDoneSpan = false;
//        sharkSpriteRenderer.sortingLayerName = "Shark";
//        StartCoroutine(DeSpawnAttackState());
//    }

//    public IEnumerator DeSpawnAttackState()
//    {
//        yield return new WaitForSeconds(ienumeratorTimeConstant);
//        sharkAnimator.SetBool("attack", false);
//        sharkAnimator.SetBool("postattack", true);
//        attackDoneSpan = true;
//        sharkState = 2;
//        start = true;
//        preattackStart = true;
//    }

//    public IEnumerator SpawnDamagedState()
//    {
//        sharkAnimator.SetBool("damaged", true);
//        yield return new WaitForSeconds(ienumeratorTimeConstant);
//        sharkAnimator.SetBool("postattack", true);
//        attackDoneSpan = true;
//        sharkState = 2;
//        start = true;
//        preattackStart = true;
//    }

//    public void SpawnGoAwayState()
//    {
//        postattackDoneSpan = false;
//        sharkSpriteRenderer.sortingLayerName = "BehindBackground";
//        StartCoroutine(DeSpawnGoAwayState());
//    }

//    public IEnumerator DeSpawnGoAwayState()
//    {
//        //sharkAnimator.SetBool("damaged", true);
//        yield return new WaitForSeconds(ienumeratorTimeConstant);
//        sharkAnimator.SetBool("postattack", false);
//        postattackDoneSpan = true;
//        start = true;
//        preattackStart = true;
//        sharkState = 0;
//    }

//    public void InstantGoAwayState()
//    {
//        sharkAnimator.SetBool("postattack", true);
//        sharkSpriteRenderer.sortingLayerName = "BehindBackground";
//        attackDoneSpan = true;
//        postattackDoneSpan = false;
//        sharkState = 3;
//        start = true;
//        preattackStart = true;
//    }

//    //: works to move shark in a certain Direction
//    public void StartSharkMovement(float directionf)
//    {
//        // check direction float
//        if (directionf > 0f)
//        {
//            //set direction
//            direction = new Vector3(directionf, 0, 0);

//            // accelerationRate
//            // Acceleration = direction * accelerationRate
//            acceleration = direction;

//            // Vector += Acceleration
//            velocity += acceleration;

//            // Velocity = Clamp Velocity, MaxSpeed
//            velocity = Vector3.ClampMagnitude(velocity, maximumSpeed);

//            // set the x Velocity to the Shark current position
//            position = new Vector3(velocity.x, 0, 0);
//        }
//        else
//        {
//            Vector3 shootVector = Vector3.zero;
//            position = shootVector;
//            transform.position = position;
//        }

//        if (flip)
//        {
//            transform.position -= position;
//        }
//        else
//        {
//            transform.position += position;
//        }
//    }

//    #endregion

//    //TODO Allow State Transition
//    private bool StateTransition()
//    {
//        // Needs: SharkEndXofSprite, flip, EndOfCanvasPosition
//        // check side shark entered : sharkPosition
//        float sharkEndPosition = gameObject.GetComponent<Sprite>().rect.maxX;
//        Vector3 targetScreenPoint = Camera.main.ScreenToWorldPoint(worldCanvas.pixelRect.position);
//        if (flip)
//        {
//            // get other side 
//            // <—-
//            float canvasViewportMaxX = (-1 * targetScreenPoint.x);
//	    if(canvasViewportMaxX < sharkEndPosition){
              
//              return true;

//            }
//        }
//        else
//        {
//            // get reg side
//            // —->
//            float canvasViewportMinX = targetScreenPoint.x;
//            if(canvasViewportMinX > sharkEndPosition){
              
//              return true;

//            }

//        }
//        // check if sharkPosition collides with other side of the screen : flip
//        // return false if not correct
//        return false;
//    }
//}