using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Turret : MonoBehaviour {

    public Transform turret;

    public float rotSpeed = 1f;

    Transform target;
    Vector3 targetPoint;

	void Start () {
		
	}
	
	void FixedUpdate ()
    {
        RaycastHit hit;
        Camera cam = Camera.main;

        //获取镜头目标点位置
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 500) && hit.transform != transform)
        {
            target = hit.transform;
            targetPoint = hit.point;

            Debug.Log(hit.transform.gameObject.name);

        }
        else
        {
            targetPoint = cam.transform.forward * 500;

        }

        //旋转炮塔
        Vector3 targetDirection = targetPoint - transform.position;

        Vector3 turretFacing = turret.rotation.eulerAngles;

        float targetAngle = Vector3.SignedAngle(targetDirection, transform.forward, -transform.up);

        turretFacing.y = targetAngle;
        turretFacing.y += transform.rotation.eulerAngles.y;

        //turret.rotation = Quaternion.Euler(turretFacing);
        turret.rotation = Quaternion.Lerp(turret.rotation, Quaternion.Euler(turretFacing), Time.deltaTime * rotSpeed);

    }
}
