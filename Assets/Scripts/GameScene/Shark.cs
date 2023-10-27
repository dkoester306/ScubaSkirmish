using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class Shark : MonoBehaviour
{
    private Vector3 startingPosition = default(Vector3);
    private Vector3 acceleration = default(Vector3);
    private Vector3 direction = default(Vector3);
    private Vector3 velocity = default(Vector3);
    private Vector3 position = default(Vector3);
    private Vector3 playerpositionRef;
    private float accelerationRate;
    private float maximumSpeed = 10f;
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
    private float ienumeratorTimeConstant = 3f;
    private float speedConstant = .3f;

    private List<BoxCollider2D> boxColliders = new List<BoxCollider2D>();

    public int SharkState
    {
        get { return sharkState; }
        set { sharkState = value; }
    }

    public int SharkHealth
    {
        get { return sharkHealth; }
        set { sharkHealth = value; }
    }

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
        // Removed multiplication by Time.deltaTime here
        accelerationRate = .5f;
        sharkState = 0;
        sharkHealth = 3;
        start = true;
        preattackStart = true;

        boxColliders = gameObject.GetComponents<BoxCollider2D>().ToList();
    }

    public void GetComponentRefs()
    {
        sharkAnimator = gameObject.GetComponent<Animator>();
        sharkAudioSource = gameObject.GetComponent<AudioSource>();
        sharkSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private Canvas worldCanvas;
    public void GetWorldCanvas(Canvas worldCanvas)
    {
        this.worldCanvas = worldCanvas;
    }

    /// <summary>
    /// Set Shark Starting Position
    /// </summary>
    private void SetStartingPosition()
    {
        Vector3 targetScreenPoint = Camera.main.ScreenToWorldPoint(worldCanvas.pixelRect.position);
        if (flip)
        {
            float canvasViewportMaxX = (-1 * targetScreenPoint.x);
            startingPosition.x = canvasViewportMaxX;
        }
        else
        {
            float canvasViewportMinX = targetScreenPoint.x;
            startingPosition.x = canvasViewportMinX;

        }
    }

    /// <summary>
    /// Reset Shark Health
    /// </summary>
    public void ResetSharkHealth(int num = 3)
    {
        sharkHealth = 3;
    }

    /// <summary>
    /// Reset All Shark States Variables
    /// </summary>
    public void ResetSharkStates()
    {
        sharkState = 0;
        velocity = default(Vector3);
        preattackDoneSpan = true;
        attackDoneSpan = true;
        postattackDoneSpan = true;
        sharkDamaged = false;
        sharkDamage = false;
        start = true;
        preattackStart = true;

        int rand = Random.Range(0, 2);
        if (rand > 0)
        {
            flip = true;
            sharkSpriteRenderer.flipX = false;
        }
        else
        {
            flip = false;
            sharkSpriteRenderer.flipX = true;
        }

        SetStartingPosition();
        transform.position = startingPosition;
    }


    /// <summary>
    /// Checks Interaction Between Player and Shark
    /// Determines if Player or Shark damaged on another
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            bool playerAttack = GameObject.Find("Swimmer").GetComponent<Swimmer2DUserControl>().Attack;
            if (playerAttack && attackDoneSpan == false && !sharkDamaged)
            {
                sharkDamaged = true;
                SharkHealth--;
                StartCoroutine(SpawnDamagedState());
                Debug.Log("Shark Health " + SharkHealth);
            }
            else if (attackDoneSpan == false && !sharkDamage && !playerAttack && !sharkDamaged)
            {
                sharkDamage = true;
                GameObject.Find("Swimmer").GetComponent<Swimmer2DUserControl>().SwimmerDamaged();
                GameObject.Find("Swimmer").GetComponent<SwimmerCharacter2D>().PlayerHealth -= 2;
                GameAudio.PlaySwimmerDamaged();
                Debug.Log("Interacted with Shark " +
                          GameObject.Find("Swimmer").GetComponent<SwimmerCharacter2D>().PlayerHealth);
            }
        }
    }

    /// <summary>
    /// When Needed, Ref to the Player Position
    /// </summary>
    /// <param name="playerPosition"></param>
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
        if (sharkHealth == 0)
        {
            InstantGoAwayState();
            ResetSharkHealth();
            return;
        }

        //: move shark forward
        if (preattackDoneSpan == false && sharkState == 0)
        {
            StartSharkMovement(1f);
            return;
        }

        //: works to move shark forward
        if (attackDoneSpan == false && sharkState == 1)
        {
            StartSharkMovement(1f);
            return;
        }

        //: stop shark if shark is in post attack
        if (postattackDoneSpan == false && (sharkState == 2 || sharkState == 3))
        {
            StartSharkMovement(0f);
            return;
        }
    }

    //: Starts the Movement of Shark in a certain Direction
    public void StartSharkMovement(float directionf)
    {
        // check direction float
        if (directionf > 0f)
        {
            //set direction
            direction = new Vector3(directionf, 0, 0);

            // Acceleration = direction * accelerationRate
            // Multiplied the entire expression by Time.deltaTime
            acceleration = direction * accelerationRate * Time.deltaTime;

            // Vector += Acceleration
            velocity += acceleration;

            // Velocity = Clamp Velocity, MaxSpeed
            // Multiplied maxSpeed by Time.deltaTime
            velocity = Vector3.ClampMagnitude(velocity, maximumSpeed * Time.deltaTime);

            // set the x Velocity to the Shark current position
            position = new Vector3(velocity.x, 0, 0);
        }
        else
        {
            Vector3 shootVector = Vector3.zero;
            position = shootVector;
            transform.position = position;
        }

        // Check Forward Movement Direction
        if (flip)
        {
            transform.position -= position;
        }
        else
        {
            transform.position += position;
        }
    }

    //: Allow State Transition
    private bool StateTransition()
    {
        // check side shark entered : sharkPosition
        // check if sharkPosition collides with other side of the screen : flip
        // return true
        Vector3 targetScreenPoint = Camera.main.ScreenToWorldPoint(worldCanvas.pixelRect.position);
        if (flip)
        {
            // get other side 
            float canvasViewportMaxX = (-1 * targetScreenPoint.x);
            float distance = canvasViewportMaxX - targetScreenPoint.x;
            if (distance < 2)
            {
                attackDoneSpan = true;
                return true;
            }
            return false;
        }
        else
        {
            float canvasViewportMinX = targetScreenPoint.x;
            float distance = canvasViewportMinX - targetScreenPoint.x;
            if (distance < 2)
            {
                attackDoneSpan = true;
                return true;
            }
            return false;
        }


    }

    #region Shark States

    public void SpawnPreAttackState()
    {
        GetComponentRefs();
        if (!preattackStart) return;

        ResetSharkStates();
        sharkAnimator.SetBool("preattack", true);
        preattackStart = false;
        preattackDoneSpan = false;
        sharkSpriteRenderer.sortingLayerName = "BehindBackground";
        StartCoroutine(DeSpawnPreAttackState());
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

        if (!start) return;
        Vector3 sharkStartPosition = startingPosition;
        Vector3 newSharkPosition = Vector3.zero;

        // Set new Shark Position based on Flip
        newSharkPosition = new Vector3(sharkStartPosition.x, playerpositionRef.y, playerpositionRef.z);
        position = newSharkPosition;
        transform.position = position;

        start = false;
        attackDoneSpan = false;
        sharkSpriteRenderer.sortingLayerName = "Shark";
        StartCoroutine(DeSpawnAttackState());
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

    #endregion
}