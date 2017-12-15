using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Ctrl : MonoBehaviour {

    public List<AxleInfo> axleInfos;
    //动力、刹车、转向
    float motor = 0;
    public float maxMotorTorque = 100;

    float brakeTorque = 0;
    public float maxBrakeTorque = 100;

    float steer = 0;
    public float maxSteerAngle = 40;

    public Transform tracks;

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        motor = v * maxMotorTorque;
        steer = h * maxSteerAngle;

        foreach (var item in axleInfos)
        {
            //转向轮
            if(item.steer){
                item.leftWheel.steerAngle = steer;
                item.rightWheel.steerAngle = steer;
            }
            //动力轮
            if(item.motor){
                item.leftWheel.motorTorque = motor;
                item.rightWheel.motorTorque = motor;
            }
        }

        foreach (Transform item in tracks)
        {
            MeshRenderer mr = item.gameObject.GetComponent<MeshRenderer>();

            Material mtl = mr.material;

            mtl.mainTextureOffset = new Vector3(-1 * v * Time.time, 0, 0);
        }


    }


}
