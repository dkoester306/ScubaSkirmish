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
    private Vector3 offScreen = new Vector3(-3,0,0);

    // starting value for the Lerp
    private static float t = 0.0f;
    private GameObject pooledAnchor;

    // Lerping Variables
    public int newTarget = 0;
    private float journey;
    private float startTime;
    private float disCovered;
    private float fractionOfJourney;
    private bool restartTime = false;

    // Update is called once per frame
    void Update()
    {
        if (restartTime)
        {
            startTime = Time.time;
            restartTime = false;
        }

        if (pooledAnchor == null)
        {
            pooledAnchor = SpawnTrash.m_Anchors[Random.Range(0, SpawnTrash.m_Anchors.Count - 1)].gameObject;
            restartTime = true;
        }
        else if (!pooledAnchor.activeInHierarchy)
        {
            MoveToOffScreen();
        }
        else
        {
            MoveToAnchor();
        }

        if (journey <= .1f)
        {
            restartTime = true;
        }
    }

    public void SetIndexAndState(int index, bool state)
    {
        newTarget = index;
        gameObject.SetActive(state);
    }

    private void MoveToOffScreen()
    {
        lerpPosition = transform.position;
        nextVector3 = new Vector3(offScreen.x, transform.position.y, transform.position.z);
        disCovered = (Time.time - startTime) * 2f;
        journey = Mathf.Abs(lerpPosition.x - nextVector3.x);
        fractionOfJourney = disCovered / journey;
        if (float.IsNaN(fractionOfJourney)) return;
        transform.position = Vector3.Lerp(lerpPosition, nextVector3, fractionOfJourney);
    }

    public void MoveToAnchor()
    {
        // now check if the interpolator has reached 1.0
        // and swap maximum and minimum so game object moves
        // in the opposite direction.
        // animate the position of the game object...
        lerpPosition = transform.position;
        nextVector3 = new Vector3(pooledAnchor.transform.position.x, transform.position.y, transform.position.z);
        disCovered = (Time.time - startTime) * 2f;
        journey = Mathf.Abs(lerpPosition.x - nextVector3.x);
        fractionOfJourney = disCovered / journey;
        if (float.IsNaN(fractionOfJourney)) return;
        transform.position = Vector3.Lerp(lerpPosition, nextVector3, fractionOfJourney);
    }
}
