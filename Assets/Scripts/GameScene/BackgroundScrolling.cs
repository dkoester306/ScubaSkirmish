using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour {
	public float backgroundSize;

	private Transform m_CameraTransform;
	private Transform[] m_Layers;
	private float m_ViewZone=10;
	private int m_LeftIndex;
	private int m_RightIndex;

    [SerializeField] private float m_ScrollingSpeed = 0.3f;

    void Start()
    {
        m_CameraTransform = Camera.main.transform;
        m_Layers = new Transform[transform.childCount];
        // get children
        for (int i = 0; i < transform.childCount; i++)
            m_Layers[i] = transform.GetChild(i);

        // set indexes
        m_LeftIndex = 0;
        m_RightIndex = m_Layers.Length - 1;
    }

    void Update()
    {
        MoveBackgrounds();
        if (m_CameraTransform.position.x > (m_Layers[m_LeftIndex].transform.position.x + m_ViewZone))
            ScrollLeft();
    }

    private void ScrollLeft()
    {
        int lastLeft = m_LeftIndex;
        Vector3 newPos = m_Layers[m_LeftIndex].position;
        newPos.x = m_Layers[m_RightIndex].position.x + m_Layers[m_LeftIndex].GetComponent<SpriteRenderer>().bounds.size.x;
        m_Layers[m_LeftIndex].position = newPos;
        m_LeftIndex = m_RightIndex;
        m_RightIndex--;
        if (m_RightIndex < 0)
            m_RightIndex = m_Layers.Length - 1;
    }

    void MoveBackgrounds()
    {
        for (int i = 0; i < m_Layers.Length; i++)
        {
            Vector3 newPos = m_Layers[i].transform.position;
            newPos.x -= m_ScrollingSpeed * Time.deltaTime;
            m_Layers[i].transform.position = newPos;
        }
    }
}
