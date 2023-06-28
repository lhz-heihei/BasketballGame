using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private float mX=0, mY=0;
    public float sensitivity;
    public Transform player_position;
    public float distance_y, distance_z;
    public float mY_min, mY_max;
    void Start()
    {
        
    }


    private void FixedUpdate()
    {
        mX += Input.GetAxis("Mouse X") * sensitivity;
        mY -= Input.GetAxis("Mouse Y") * sensitivity;
        mY = Mathf.Clamp(mY, mY_min, mY_max);
        Quaternion mRotation = Quaternion.Euler(mY, mX, 0);
        transform.rotation = mRotation;
        Vector3 distance = new Vector3(0, distance_y, distance_z);
        transform.position = mRotation * distance + player_position.position;
    }
}
