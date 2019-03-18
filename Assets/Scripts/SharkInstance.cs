using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkInstance : MonoBehaviour
{
    private int sharkstate;
    private int health;
    private Vector3 position;
    private Vector3 playerpositionRef;
    private bool preattackDoneSpan; 
    private bool attackDoneSpan;
    private bool postattackDoneSpan;
    private bool sharkDamage;
    private bool sharkDamaged;

    private Animator sharkAnimator;
    private AudioSource sharkAudioSource;
    private BoxCollider[] boxColliders;
    private float[] m_MinX;
    private float[] m_MaxX;
    private float[] m_MinY;
    private float[] m_MaxY;
    private int numberofColliders;

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

    // Start is called before the first frame update
    void Start()
    {
        sharkAnimator = this.gameObject.GetComponent<Animator>();
        sharkAudioSource = this.gameObject.GetComponent<AudioSource>();
        CapturingBoxColliders();
        sharkstate = 0;
        health = 3;
    }

    private void CapturingBoxColliders()
    {
        boxColliders = new BoxCollider[numberofColliders];
        for (int i = 0; i < numberofColliders; i++)
        {
            boxColliders[i] = this.gameObject.GetComponents<BoxCollider>()[i];
        }

        m_MinX = new float[2];
        m_MaxX = new float[2];
        m_MinY = new float[2];
        m_MaxY = new float[2];

        for (int i = 0; i < numberofColliders; i++)
        {
            m_MinX[i] = boxColliders[i].bounds.min.x;
            m_MaxX[i] = boxColliders[i].bounds.max.x;
            m_MinY[i] = boxColliders[i].bounds.min.x;
            m_MaxY[i] = boxColliders[i].bounds.max.x;
        }
    }

    // Update is called once per frame
    void Update()
    {
        position = this.transform.position;

        if (preattackDoneSpan == true && attackDoneSpan == false)
        {
            CheckIntersect();
        }

        while (sharkstate == 1)
        {

            //: interaction
            if (sharkDamage)
            {
                GameObject.Find("Swimmer").GetComponent<SwimmerCharacter2D>().PlayerHealth--;
                sharkDamage = false;
            }

            //: player attack
            if (sharkDamaged)
            {
                health--;
                if (health <= 0)
                {
                    sharkstate = 2;
                }
            }

            int randomInt = UnityEngine.Random.Range(0, 3);
            if (randomInt == 0)
            {
                //sharkstate = 0;
            }
            else if (randomInt == 1)
            {
                //sharkstate = 1;
            }
            else
            {
                //sharkstate = 2;
            }
        }
    }

    public void SpawnPreAttackState()
    {
        sharkstate = 0;
        postattackDoneSpan = false;
        attackDoneSpan = false;
        preattackDoneSpan = false;

        //: Call Find Player Position (Called Already)

        //: Call Animation from Animator ( Background Swimming )
        //sharkAnimator.SetBool("preattack",true);
        preattackDoneSpan = true;


        //: Check if Shark Reached Other Side of the Screen
        // if so switch to attack state
        if (true)
        {
            //sharkAnimator.SetBool("preattack", false);
            sharkstate = 1;
        }

        //position = this.transform.position;
    }

    public void SpawnAttackState()
    {
        //sharkAnimator.SetBool("attack", true);

        position = new Vector3(-8.21f,0,0);
        Vector3 newSharkPosition = new Vector3(position.x, playerpositionRef.y,playerpositionRef.z);
        this.gameObject.transform.position = newSharkPosition;

        SpawnSharkAttackState();

        attackDoneSpan = true;
        //sharkAnimator.SetBool("attack", false);

        if (false)
        {
            //sharkAnimator.SetBool("preattack", false);
            sharkstate = 2;
        }
    }

    public void SpawnGoAwayState()
    {
        //StartCoroutine(SpawnSharkGoAwayState());
        
        //sharkAnimator.SetBool("goaway", true);
        //sharkAnimator.SetBool("goaway", false);

        postattackDoneSpan = true;
    }

    //? Possible IEnumerator Method
    public IEnumerator SpawnDamagedState()
    {
        //sharkAnimator.SetBool("damaged", true);
        yield return new WaitForSeconds(2f);
        //sharkAnimator.SetBool("damaged", false);
    }

    public void SpawnSharkAttackState()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 newSharkPosition = new Vector3(gameObject.transform.position.x + 1, playerpositionRef.y, playerpositionRef.z);
            this.gameObject.transform.position = newSharkPosition;
        }
    }

    public IEnumerator SpawnSharkGoAwayState()
    {
        //sharkAnimator.SetBool("damaged", true);
        yield return new WaitForSeconds(1f);
        //sharkAnimator.SetBool("damaged", false);
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
        // Inner Collider
        bool maxXIn = m_MinX[0] < playerpositionRef.x;
        bool minXIn = m_MaxX[0]> playerpositionRef.x;
        bool maxIn =m_MinY[0] > playerpositionRef.y;
        bool minIn = m_MaxY[0] < playerpositionRef.y;

        if (maxXIn && minXIn && maxIn && minIn)
        {
            sharkDamage = true;
            Debug.Log("Hit");
        }

        bool maxXOut = m_MinX[1] < playerpositionRef.x;
        bool minXOut = m_MaxX[1] > playerpositionRef.x;
        bool maxYOut = m_MinY[1] > playerpositionRef.y;
        bool minYOut = m_MaxY[1] < playerpositionRef.y;

        if (maxXOut && minXOut && maxYOut && minYOut)
        {
            sharkDamaged = true;
            Debug.Log("Hit");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameObject.Find("Swimmer").GetComponent<Swimmer2DUserControl>().Attack = true;
            if (GameObject.Find("Swimmer").GetComponent<Swimmer2DUserControl>().Attack)
            {
                sharkDamaged = true;
                health--;
                Debug.Log("Shark Health " + health );
            }
            else
            {
                sharkDamage = true;
                GameObject.Find("Swimmer").GetComponent<SwimmerCharacter2D>().PlayerHealth--;
                Debug.Log("Interacted with Shark " + GameObject.Find("Swimmer").GetComponent<SwimmerCharacter2D>().PlayerHealth);
            }
            
        }

    }
}
