using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour {


    void FixedUpdate () {
        transform.Rotate(new Vector3(0, 0, 100) * Time.deltaTime * -0.01f);
    }

}
