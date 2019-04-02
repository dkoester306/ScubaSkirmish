using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SharkInstance : MonoBehaviour
{
    private int sharkstate;
    private int health;
    private Vector3 position;
    private Vector3 playerpositionRef;
    private Vector3 startingPosition;
    private bool preattackDoneSpan; 
    private bool attackDoneSpan;
    private bool postattackDoneSpan;
    private bool sharkDamage;
    private bool sharkDamaged;
    private bool start;

    private Animator sharkAnimator;
    private AudioSource sharkAudioSource;
    private BoxCollider2D[] boxColliders;
    private float[] m_MinX;
    private float[] m_MaxX;
    private float[] m_MinY;
    private float[] m_MaxY;
    private int numberofColliders = 2;

    public int SharkState
    {
        get { return sharkstate; }
        set { sharkstate = value; }
    }

    public int SharkHealth
    {
        get { return health; }
        set { health = value; }
    }

    public Vector3 StartingPosition
    {
        get
        {
            return startingPosition;
        }

        set
        {
            startingPosition = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = new Vector3(-10.21f, 0, 0);
        sharkstate = 0;
        health = 3;
        start = true;
    }

    public void ResetSharkHealth()
    {
        health = 3;
    }

    public void ResetSharkStates()
    {
        //health = 3;
        sharkstate = 0;
        postattackDoneSpan = false;
        attackDoneSpan = false;
        preattackDoneSpan = false;
        sharkDamaged = false;
        sharkDamage = false;
        //transform.position = startingPosition;
    }

    public void GetComponentRefs()
    {
        sharkAnimator = gameObject.GetComponent<Animator>();
        sharkAudioSource = gameObject.GetComponent<AudioSource>();
    }

    private void CapturingBoxColliders()
    {
        boxColliders = new BoxCollider2D[numberofColliders];
        for (int i = 0; i < numberofColliders; i++)
        {
            boxColliders[i] = gameObject.GetComponents<BoxCollider2D>()[i];
            Debug.Log(boxColliders.Length);
        }

        m_MinX = new float[2];
        m_MaxX = new float[2];
        m_MinY = new float[2];
        m_MaxY = new float[2];

        for (int i = 0; i < numberofColliders; i++)
        {
            m_MinX[i] = boxColliders[i].bounds.min.x;
            m_MaxX[i] = boxColliders[i].bounds.max.x;
            m_MinY[i] = boxColliders[i].bounds.min.y;
            m_MaxY[i] = boxColliders[i].bounds.max.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //: works to move shark forward
        if (preattackDoneSpan == false && sharkstate == 0)
        {
            Vector3 shootVector = new Vector3(.2f, 0, 0);
            position = shootVector;
            transform.position += position;
        }

        //transform.position = position
        if (attackDoneSpan == false && sharkstate == 1)
        {
            CapturingBoxColliders();
            CheckIntersect();
        }

        //: works to move shark forward
        if (attackDoneSpan == false && sharkstate == 1)
        {
            Vector3 shootVector = new Vector3(.2f, 0, 0);
            position = shootVector;
            transform.position += position;
        }

        //: interaction
        if (sharkDamage && sharkstate == 1)
        {
            //GameObject.Find("Swimmer").GetComponent<SwimmerCharacter2D>().PlayerHealth--;
            //sharkDamage = false;
            sharkDamage = false;
        }

        //: player attack
        if (sharkDamaged && attackDoneSpan == false && sharkstate == 1)
        {
            if (health <= 0)
            {
                //InstantGoAwayState();
            }
        }

        if (postattackDoneSpan == false && sharkstate == 2)
        {
            Vector3 shootVector = new Vector3(0, 0, 0);
            position = shootVector;
            transform.position = position;
        }
    }

    public void SpawnPreAttackState()
    {
        GetComponentRefs();
        ResetSharkStates();
        sharkAnimator.SetBool("preattack", true);
        //: Call Find Player Position (Called Already)

        //: Call Animation from Animator ( Background Swimming )
        StartCoroutine(DeSpawnPreAttackState());

        //: Check if Shark Reached Other Side of the Screen
        // if so switch to attack state
        if (true)
        {
            //sharkstate = 1;
        }
        //position = this.transform.position;
        
    }

    public IEnumerator DeSpawnPreAttackState()
    {
        yield return new WaitForSeconds(5f);
        sharkAnimator.SetBool("preattack", false);
        sharkAnimator.SetBool("attack", true);
        preattackDoneSpan = true;
        sharkstate = 1;
    }

    public void SpawnAttackState()
    {
        attackDoneSpan = false;
        sharkAnimator.SetBool("attack", true);

        if (start)
        {
            Vector3 sharkStartPosition = startingPosition;
            //position = sharkStartPosition;
            //transform.position = sharkStartPosition;
            Vector3 newSharkPosition = new Vector3(sharkStartPosition.x, playerpositionRef.y, playerpositionRef.z);
            position = newSharkPosition;
            transform.position = position;
            //position = Vector3.zero;

            SpawnGoAwayState();
            start = false;
        }

        //SpawnSharkAttackState();

        //attackDoneSpan = true;
        //attackDoneSpan = true;
    }

    //: works to move shark forward
    public void SpawnSharkAttackState()
    {
        Vector3 newSharkPosition = new Vector3(.1f, 0, 0);
        position = newSharkPosition;
        transform.position += position;
        //position = Vector3.zero;
    }

    //? Possible IEnumerator Method
    public IEnumerator SpawnDamagedState()
    {
        sharkAnimator.SetBool("damaged", true);
        yield return new WaitForSeconds(5f);
        sharkAnimator.SetBool("damaged", false);
    }

    public void SpawnGoAwayState()
    {
        //StartCoroutine(SpawnSharkGoAwayState());
        //sharkAnimator.SetBool("goaway", true);
        //sharkAnimator.SetBool("goaway", false);
        postattackDoneSpan = false;
        StartCoroutine(SpawnSharkGoAwayState());
    }

    public void InstantGoAwayState()
    {
        sharkAnimator.SetBool("attack", false);
        attackDoneSpan = true;
        sharkAnimator.SetBool("postattack", true);
        postattackDoneSpan = true;
        sharkstate = 2;
        start = true;
        Debug.Log("InstantGoAway");
    }

    public IEnumerator SpawnSharkGoAwayState()
    {
        //sharkAnimator.SetBool("damaged", true);
        yield return new WaitForSeconds(5f);
        sharkAnimator.SetBool("attack", false);
        attackDoneSpan = true;
        sharkAnimator.SetBool("postattack", true);
        postattackDoneSpan = true;
        sharkstate = 2;
        start = true;
        Debug.Log("EventGoAway");
    }

    public void FindPlayerPosition(Vector3 playerPosition)
    {
        playerpositionRef = playerPosition;
    }

    // Checks if these gameobjects are intersecting
    // will retrieve both sprite renderers
    // return a true statement
    private void CheckIntersect()
    {
        // Outside Collider
        bool maxXOut = m_MinX[1] < playerpositionRef.x;
        bool minXOut = m_MaxX[1] > playerpositionRef.x;
        bool maxYOut = m_MinY[1] < playerpositionRef.y;
        bool minYOut = m_MaxY[1] > playerpositionRef.y;

        bool playerAttack = GameObject.Find("Swimmer").GetComponent<Swimmer2DUserControl>().Attack;
        if (maxXOut && minXOut && maxYOut && minYOut && !playerAttack)
        {
            //health--;
            StartCoroutine(SpawnDamagedState());
            //sharkDamage = true;
            Debug.Log("Hit Outside Box");
            Debug.Log("Shark Health " + health);
        }

        // Inner Collider
        bool maxXIn = m_MinX[0] < playerpositionRef.x;
        bool minXIn = m_MaxX[0]> playerpositionRef.x;
        bool maxIn =m_MinY[0] < playerpositionRef.y;
        bool minIn = m_MaxY[0] > playerpositionRef.y;

        if (maxXIn && minXIn && maxIn && minIn && !sharkDamaged)
        {
            //GameObject.Find("Swimmer").GetComponent<SwimmerCharacter2D>().PlayerHealth--;
            //sharkDamaged = true;
            Debug.Log("Hit Inside Box");
            Debug.Log("Interacted with Shark " + GameObject.Find("Swimmer").GetComponent<SwimmerCharacter2D>().PlayerHealth);
        }

        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            bool playerAttack = GameObject.Find("Swimmer").GetComponent<Swimmer2DUserControl>().Attack;
            if (playerAttack && attackDoneSpan == false)
            {
                //sharkDamaged = true;
                //Debug.Log("Shark Health " + health );
            }
            else if(attackDoneSpan == false)
            {
                //sharkDamage = true;
                //Debug.Log("Interacted with Shark " + GameObject.Find("Swimmer").GetComponent<SwimmerCharacter2D>().PlayerHealth);
            }
            
        }

    }
}
