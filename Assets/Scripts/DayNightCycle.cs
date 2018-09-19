using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour {

    public float cycleSpeed = .5f;



	void Update ()
    {
        transform.Rotate(Vector3.right * (cycleSpeed * Time.deltaTime));

    }
}
