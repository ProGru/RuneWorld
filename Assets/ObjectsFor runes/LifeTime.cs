using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTime : MonoBehaviour {

    public float period = 60;
    private float nextActionTime;
    // Use this for initialization
    void Start () {
        nextActionTime = Time.time + period;
	}


    void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime = (Time.time + period);
            Destroy(this.gameObject);
        }
    }
}
