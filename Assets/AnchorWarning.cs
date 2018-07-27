using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorWarning : MonoBehaviour {

	public static Stack<GameObject> AnchorWarnings;

	void Start () {
		// add all of the children objects in THIS to the list
		AnchorWarnings = new Stack<GameObject>();
		foreach(Transform child in transform)
		{
			AnchorWarnings.Push(child.gameObject);
		}
	}
	
	
	// Update is called once per frame
	void Update () {
	}
}
