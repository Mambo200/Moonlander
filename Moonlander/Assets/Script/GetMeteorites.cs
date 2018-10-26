using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMeteorites : MonoBehaviour {
    private Transform[] m_ObstaclesWithEmpty;
    public static Transform[] m_ChildrenObstacles;
	// Use this for initialization
	void Awake () {
        // get all Components
        m_ObstaclesWithEmpty = GetComponentsInChildren<Transform>();

        // set all components exept the first one
        m_ChildrenObstacles = new Transform[m_ObstaclesWithEmpty.GetLength(0) - 1];
        for (int i = 1; i < m_ObstaclesWithEmpty.GetLength(0); i++)
        {
            m_ChildrenObstacles[i - 1] = m_ObstaclesWithEmpty[i];
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	}
}
