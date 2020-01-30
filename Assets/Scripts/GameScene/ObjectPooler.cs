using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Will Pool Objects to be reused instead of instantaiting all the time
// Object Pooling
// Can be used multiple times
public class ObjectPooler : MonoBehaviour {

    // multiple scripts will access it so it needs to be static
    // otherwise we will need a gameobject
    public static ObjectPooler sharedInstance;
    private void Awake()
    {
        sharedInstance = this;
    }

    // varaibles for pooling
    // can also use a dictionary<Gameobject, Stack<Pool>>
    public List<GameObject> pooledObjects;
    public List<ObjectPoolItem> itemsToPool;

    // Use this for initialization
    void Start () {
        // * This will create the amountToPool amount of objects at the Start of runtime
        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            // create gameobjects and set to fasle
            for (int i = 0; i < item.amountToPool; i++)
            {
                CreatePooledObject(item, false);
            }
        }

        // Add GameObjects to TrashLists
        SpawnTrash.m_Anchors = new List<GameObject>();
        foreach (var anchor in ObjectPooler.sharedInstance.pooledObjects)
        {
            if (anchor.GetComponent<Anchor>())
            {
                SpawnTrash.m_Anchors.Add(anchor);
            }
        }
        SpawnTrash.m_Mines = new List<GameObject>();
        foreach (var mine in ObjectPooler.sharedInstance.pooledObjects)
        {
            if (mine.GetComponent<Mine>())
            {
                SpawnTrash.m_Mines.Add(mine);
            }
        }

        // Create Boats
        for (int i = 0; i < 2 ; i++)
        {
            GameObject tempBoat = ObjectPooler.sharedInstance.GetPoolObject("Boat");
            tempBoat.GetComponent<Boat>().newTarget = i;
            tempBoat.SetActive(true);
        }
    }

    // creates a new instance of a specific gameobject
    private GameObject CreatePooledObject(ObjectPoolItem item, bool active)
    {
        GameObject obj = (GameObject)Instantiate(item.objectToPool); // set to Gameobject and Instantiate
        pooledObjects.Add(obj);   // add object to the list
        obj.SetActive(active);   // set to false
        obj.transform.SetParent(gameObject.transform);  // add TrashSpawner as parent
        return obj;
    }

    public GameObject GetPoolObject(string tag)
    {
        // step 1
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            // check all objects in the pool if they are setactive = false
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag)
            {
                return pooledObjects[i];
            }
        }

        // check each ObjectPoolItem reference
        // see if it should expand
        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectToPool.tag == tag)
            {
                // if there are no current active objects return null
                if (item.shouldExpand)
                {
                    return CreatePooledObject(item, true);
                }
            }
        }

        // return nothing 
        return null;
    }

    // very interesting this works
    // Creates a new class inside existing Script
    // Gets objectToPool
    // amount of Clones to Instantiate
    // the ability to Expand beyound the poolamount
    [System.Serializable]
    public class ObjectPoolItem
    {
        [SerializeField]
        internal GameObject objectToPool;
        [SerializeField]
        internal int amountToPool;      
        public bool shouldExpand = true;
    }
}
