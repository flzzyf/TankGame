using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_TPS : MonoBehaviour {

    public float distance = 10;
    public Vector2 cameraDistanceLimit = new Vector2(2, 6);

    public Transform target;

    float rotationX = 0.0f;
    float rotationY = 0.0f;

    public float mouseSensitivity = 3;

    public Vector2 yMinMax = new Vector2(-85, 85);

    public float rotationSmoothTime = .1f;

    public float scrollSensitivity = 3f;

    bool lockCursor = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        distance = -transform.position.z;

        newDistance = distance;

        currentRotation = transform.eulerAngles;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            lockCursor = !lockCursor;

            if (lockCursor)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;
        }
    }

    void FixedUpdate()
    {
        MouseScroll();
        MouseMove();
        MoveCamera();
    }

    #region 鼠标旋转镜头

    void MouseMove()
    {
        if (!lockCursor)
            return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotationX += mouseX;
        rotationY -= mouseY;
        //Y角度约束
        rotationY = Mathf.Clamp(rotationY, yMinMax.x, yMinMax.y);

    }
    #endregion

    #region 鼠标滚轮

    void MouseScroll()
    {
        if (!lockCursor)
            return;

        float scrollWheelAmount = Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;

        if (scrollWheelAmount != 0)
        {
            distance += scrollWheelAmount * -1 * scrollSensitivity;
            //限制镜头距离
            distance = Mathf.Clamp(distance, cameraDistanceLimit.x, cameraDistanceLimit.y);
        }
    }

    #endregion

    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    float newDistance;
    float smoothV;
    float scrollSmoothTime = 0.1f;

    void MoveCamera()
    {
        //旋转镜头
        Vector3 rotation = new Vector3(rotationY, rotationX);
        //平滑旋转
        currentRotation = Vector3.SmoothDamp(currentRotation, rotation, ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        newDistance = Mathf.SmoothDamp(newDistance, distance, ref smoothV, scrollSmoothTime);
        //移动镜头
        Vector3 newPos = target.position - transform.forward * newDistance;
        //Vector3 newPos = target.position - transform.forward * distance;
        //newPos.z = Mathf.Lerp(newPos.z, distance, scrollSmoothTime);
        transform.position = newPos;
    }
}
