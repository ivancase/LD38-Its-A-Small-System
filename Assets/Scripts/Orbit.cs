using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour {
    public GameObject primary;
    public float speed;
    public float gravityScale;
    public float camSize;

    public bool playerOn = false;

	void FixedUpdate () {
        if (!playerOn) {
            transform.RotateAround(primary.transform.position, -Vector3.forward, speed / 4);
        }

    }
}
