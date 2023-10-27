using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Will Pool Objects to be reused instead of instantiating all the time
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

    // variables for pooling
    // can also use a dictionary<Gameobject, Stack<Pool>>
    public List<GameObject> pooledObjects = new List<GameObject>();
    public List<ObjectPoolItem> itemsToPool;

    // creates a new instance of a specific gameobject
    private GameObject CreatePooledObject(ObjectPoolItem item, bool active)
    {
        GameObject obj = Instantiate(item.objectToPool); // set to Gameobject and Instantiate
        pooledObjects.Add(obj);   // add object to the list
        obj.SetActive(active);   // set to false
        obj.transform.SetParent(gameObject.transform);  // add TrashSpawner as parent
        return obj;
    }

    // Use this for initialization
    void Start () {

        // Add PoolCreationRequests to ParentPoolList
        foreach (var item in itemsToPool){
            // create gameobjects and set object inActive
            CreatePooledObject(item, false);
        }

        // #LINQ Expressions
        // Instantiate Anchor Trash Collection
        foreach (var anchor in sharedInstance.pooledObjects
            .Where(anchor => anchor.GetComponent<Anchor>())){
            SpawnTrash.m_Anchors.Add(anchor);
        }

        // Instantiate Mine Trash Collection
        foreach (var mine in sharedInstance.pooledObjects
            .Where(mine => mine.GetComponent<Mine>())){
            SpawnTrash.m_Mines.Add(mine);
        }

        // Instantiate Boat Collection
        for (int i = 0; i < SpawnTrash.boatAmount ; i++)
        {
            sharedInstance.GetPoolObject("Boat")
                .GetComponent<Boat>().SetIndexAndState(i,true);
        }
    }

    public GameObject GetPoolObject(string tag)
    {
        // Determine if Object is Not Active and Correct Tag
        foreach (var pObject in pooledObjects
            .Where(obj => !obj.activeInHierarchy && obj.tag == tag)){
            return pObject;
        }

        // check each ObjectPoolItem reference
        // see if Pool List should expand
        return (from item in itemsToPool where item.objectToPool.tag == tag 
            where item.shouldExpand 
            select CreatePooledObject(item, true))
            .FirstOrDefault();
    }

    // Gets objectToPool
    // amount of Clones to Instantiate
    // the ability to Expand beyond the pool amount
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
