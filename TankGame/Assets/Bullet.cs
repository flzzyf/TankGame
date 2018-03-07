using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float launchForce = 100f;

	void Start ()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * launchForce, ForceMode.Impulse);

        Destroy(gameObject, 5f);
	}
	
	void Update () {
		
	}
}
