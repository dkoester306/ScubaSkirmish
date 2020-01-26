using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Boat is a Non-Interactable Object that selects a falling anchor
/// and lerps to that position
/// </summary>
public class Boat : MonoBehaviour
{
    // animate the game object from -1 to +1 and back
    private float minimum = -1.0F;
    private float maximum = 1.0F;
    private Vector3 lerpPosition = Vector3.zero;
    private Vector3 lastVector3;
    private Vector3 nextVector3;

    // starting value for the Lerp
    private static float t = 0.0f;
    private GameObject pooledAnchor;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnEnable()
    {
        //pooledAnchor = GameObject.FindGameObjectWithTag("Anchor");
        //lerpPosition = pooledAnchor.transform.position;
        //nextVector3 = new Vector3(lerpPosition.x, transform.position.y, transform.position.z);
        //lastVector3 = transform.position;
        //journey = Mathf.Abs(nextVector3.x - lastVector3.x);
        //startTime = Time.time;
        //disCovered = (Time.time - startTime) * 3f;
        //fractionOfJourney = disCovered / journey;
        //transform.position = Vector3.Lerp(lastVector3, nextVector3, fractionOfJourney);
    }

    public int newTarget = 0;
    private float journey;
    private float startTime;
    private float disCovered;
    private float fractionOfJourney;
    private Vector3 farOff = new Vector3(-5,0,0);

    // Update is called once per frame
    void Update()
    {
        // now check if the interpolator has reached 1.0
        // and swap maximum and minimum so game object moves
        // in the opposite direction.
        if (pooledAnchor == null)
        {
            pooledAnchor = SpawnTrash.m_Anchors[Random.Range(0, 3)].gameObject;

            lerpPosition = pooledAnchor.transform.position;
            nextVector3 = new Vector3(farOff.x, transform.position.y, transform.position.z);
            lastVector3 = transform.position;
            journey = Mathf.Abs(nextVector3.x - lastVector3.x);
            startTime = Time.time;
            transform.position = Vector3.Lerp(lastVector3, nextVector3, fractionOfJourney);
        }

        if (pooledAnchor.activeInHierarchy == false)
        {
            pooledAnchor = SpawnTrash.m_Anchors[Random.Range(0, 3)].gameObject;

            lerpPosition = pooledAnchor.transform.position;
            nextVector3 = new Vector3(farOff.x, transform.position.y, transform.position.z);
            lastVector3 = transform.position;
            journey = Mathf.Abs(nextVector3.x - lastVector3.x);
            startTime = Time.time;
            transform.position = Vector3.Lerp(lastVector3, nextVector3, fractionOfJourney);
        }

        // animate the position of the game object...
        disCovered = (Time.time - startTime) * 3f;
        fractionOfJourney = disCovered / journey;
        transform.position = Vector3.Lerp(lastVector3, nextVector3, fractionOfJourney);
        Debug.Log(newTarget);

        if (journey <= .01f) {
            pooledAnchor = SpawnTrash.m_Anchors[Random.Range(0, 3)].gameObject;

            lerpPosition = pooledAnchor.transform.position;
            nextVector3 = new Vector3(farOff.x, transform.position.y, transform.position.z);
            lastVector3 = transform.position;
            journey = Mathf.Abs(nextVector3.x - lastVector3.x);
            startTime = Time.time;
        }
    }
}
