using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtr : MonoBehaviour {
    public CircleInput moveInput;
    public CircleInput lookInput;
    public Gun gun;
    public float speed=2;
    public float rotateSpeed = 2;
    public Transform aimPos;
   // public CircleInput input;
	// Use this for initialization
	void Start () {
        canFire = false;
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LookInputUp()
    {
        if (canFire == false && gun.auto == Automation.Non_automatic)
        {
            canFire = true;
        }
    }
    bool canFire;
    void Shoot()
    {

        if (lookInput.length > 0.8f && (canFire||gun.auto==Automation.Fully_automatic))
        {
            gun.Fire();
            canFire=false;
        }
        else if(canFire==false&& lookInput.length<0.8f&& gun.auto == Automation.Semi_automatic)
        {
            canFire = true;
        }
        
    }
    void Look(Vector2 direction)
    {
        if (direction == Vector2.zero) return;
        Quaternion look = new Quaternion();
        look.SetLookRotation(new Vector3(direction.x, 0, direction.y));
        transform.rotation =Quaternion.Lerp(transform.rotation, look,Time.deltaTime*rotateSpeed);
        aimPos.position = transform.position +new Vector3(lookInput.direction.x, 0, lookInput.direction.y) * lookInput.length*gun.range;
        
    }
    void Move(Vector2 direction)
    {
        transform.position += new Vector3(direction.x,0,direction.y)*Time.deltaTime* speed;
    }
    private void FixedUpdate()
    {
        Move(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
         Move(moveInput.direction*moveInput.length);
        Look(lookInput.direction);
       
        Shoot();
    }
}
