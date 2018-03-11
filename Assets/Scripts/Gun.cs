using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    [Range(0.5f,100)]
    public float shootSpeed=8.6f;
    public float caliber = 7.62f;
    public GameObject fireParticle;
    public Automation auto=Automation.Fully_automatic;
    public GameObject bullet;
    public Transform gunShootPositon;
    public float angle=5;
    float shootTime;
    public float range = 10;
   
    // Use this for initialization
    void Start () {
       
        
    }
	public void Fire()
    {
        if (Time.time- shootTime >1/ shootSpeed)
        {
            fireParticle.SetActive(false);
            fireParticle.SetActive(true);
            Quaternion bulletDirection=transform.rotation;
            float rate = (1 - (Time.time - shootTime)) / 1;
            rate= Mathf.Clamp01(rate);
            
            bulletDirection.eulerAngles += new Vector3(0, Random.Range(-angle * rate, angle*rate), 0);
            TrailRenderer trail= Instantiate(bullet, gunShootPositon.position, bulletDirection).GetComponent<TrailRenderer>();
           
            trail.widthMultiplier = caliber / 100;
            shootTime = Time.time;
        }
    }

    
    // Update is called once per frame
    private void FixedUpdate()
    {
       
    }
  
}
public enum Automation
{
    Non_automatic,
    Semi_automatic,
    Fully_automatic
}