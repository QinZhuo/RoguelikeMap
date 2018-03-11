using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFallow : MonoBehaviour {
    public Transform target;
    public Transform aim;
    public float rate = 1;
    Vector3 offset;
	// Use this for initialization
	void Start () {
        offset = transform.position - target.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (target)
        {
            if (aim)
            {
              
                if ((aim.position - target.position).magnitude > 15)
                {
                    transform.position = Vector3.Lerp(transform.position, (offset + ((aim.position - target.position).magnitude - 15)/1.5f * offset.normalized + (target.position + aim.position) / 2), Time.deltaTime * rate);
                }
                else
                {
                    transform.position = Vector3.Lerp(transform.position, (offset + (target.position + aim.position) / 2), Time.deltaTime * rate);
                }
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, offset + target.position, Time.deltaTime * rate);
            }
            
        }
	}
}
