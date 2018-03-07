using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Attack : MonoBehaviour {

    public GameObject bulletPrefab;

    public Transform launchPoint;

	void Start () {
		
	}
	
	void Update ()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Fire");

            GameObject bullet = Instantiate(bulletPrefab, launchPoint.position, launchPoint.rotation);

            //bullet.transform.rotation = launchPoint.rotation;
            //bullet.transform.parent = null;

        }
    }
}
