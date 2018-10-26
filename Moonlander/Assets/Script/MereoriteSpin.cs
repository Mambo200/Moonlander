using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MereoriteSpin : MonoBehaviour {
    public float m_RotationSpeed;
    private float rnd;
	// Use this for initialization
	void Start ()
    {
        rnd = Random.Range(-5f, 5f);
	}
	
	// Update is called once per frame
	void Update ()
    {
        // setzt neue Rotation
        transform.localRotation = Quaternion.Euler(
            0,
            transform.rotation.eulerAngles.y + (m_RotationSpeed * Time.deltaTime * rnd),
            transform.rotation.eulerAngles.z + (m_RotationSpeed * Time.deltaTime * rnd)
            );
    }
}
