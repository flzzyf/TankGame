using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZyfCamera : MonoBehaviour {
    public bool canMove = false;
    public bool canRotate = false;

    [Header("WASD")]
    public float panSpeed = 10;
    public float slowMoveFactor = 0.25f;
    public float fastMoveFactor = 3;
    public float climbSpeed = 4;

    [Header("Mouse")]
    public float mouseSensitivity = 120;
    public float scrollSensitivity = 3f;
    [Range(6, 12)]
    public float scrollSpeed = 6f;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    Transform cam;
    Transform camOrigin;
    //FlyCam
    public Vector2 cameraDistanceLimit = new Vector2(2, 6);
    float cameraDistance = 3f;

    public Transform target;

    void Start()
    {
        camOrigin = transform;
        cam = transform.GetChild(0);

        cameraDistance = -cam.position.z;

        ToggleMouseVisible();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMouseVisible();
        }
    }

    void LateUpdate()
    {
        if (canMove)
            KeyboardControl();
        if(canRotate)
            MouseControl();

        //有注视目标则移动到目标位置
        if (target != null)
        {
            Transform origin = camOrigin;
            origin.position = Vector3.Lerp(origin.position, target.position, panSpeed * Time.deltaTime);

            //cam.LookAt(target);
        }
    }
    //切换鼠标可见性
    void ToggleMouseVisible()
    {
        Cursor.visible = !Cursor.visible;

        if (!Cursor.visible)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
    }

    void KeyboardControl()
    {
        //Shift加速，Ctrl减速
        if (Input.GetKeyDown(KeyCode.LeftShift))
            panSpeed *= fastMoveFactor;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            panSpeed /= fastMoveFactor;

        if (Input.GetKeyDown(KeyCode.LeftControl))
            panSpeed *= slowMoveFactor;
        if (Input.GetKeyUp(KeyCode.LeftControl))
            panSpeed /= slowMoveFactor;

        //WASD输入
        float inputV = Input.GetAxis("Vertical");
        float inputH = Input.GetAxis("Horizontal");
        //移动镜头
        if (inputV != 0 || inputH != 0)
        {
            camOrigin.position += inputV * cam.forward * panSpeed * Time.deltaTime;
            camOrigin.position += inputH * cam.right * panSpeed * Time.deltaTime;
        }

        //QE爬升降低
        if (Input.GetKey(KeyCode.E)) { camOrigin.position += camOrigin.up * climbSpeed * Time.deltaTime; }
        if (Input.GetKey(KeyCode.Q)) { camOrigin.position -= camOrigin.up * climbSpeed * Time.deltaTime; }
    }

    void MouseControl()
    {
        //鼠标输入
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        rotationX += mouseX * mouseSensitivity * Time.deltaTime;
        rotationY += mouseY * mouseSensitivity * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, -90, 90);
        //旋转镜头
        camOrigin.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        camOrigin.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

        //鼠标滚轮输入
        float scrollWheelAmount = Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;

        if (scrollWheelAmount != 0)
        {
            cameraDistance += scrollWheelAmount * -1 * scrollSensitivity;
            //限制镜头距离
            cameraDistance = Mathf.Clamp(cameraDistance, cameraDistanceLimit.x, cameraDistanceLimit.y);

        }
        Vector3 newPos = new Vector3(0, 0, -cameraDistance);

        cam.localPosition = Vector3.Lerp(cam.localPosition, newPos, Time.deltaTime * scrollSpeed);
    }
}
