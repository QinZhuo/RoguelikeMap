using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    // Use this for initialization

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.position += transform.forward*Time.deltaTime*100;
    }
}
