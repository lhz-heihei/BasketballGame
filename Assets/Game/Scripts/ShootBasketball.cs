using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShootBasketball : MonoBehaviour
{
    public bool isShoot = false;
    public Animator animator;
    
    public float delaytime;
    public Transform target;
    private Vector3 initialDir,dir;
    public float g;
    public float speed;
    private float timer;
    private float time;
    public Player player;
    public pickupBasketball _pickupBasketball;
    public bsketball _bsketball;
    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0)&&player.currentState==Player.CharacterState.Dribbling )
        {  
            animator.SetTrigger("Shoot");
            _bsketball.isdrib = false;
            GameObject basketball = GameObject.FindGameObjectWithTag("basketball");
            GameObject righthand = GameObject.FindGameObjectWithTag("rightHand");
            basketball.GetComponent<SphereCollider>().enabled = false;
            basketball.transform.parent = righthand.transform;
            basketball.transform.localPosition = new Vector3(0, 0, player.hand_baskrtball_distance);
            //basketball.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }

        if (isShoot)
        {
            transform.position += dir*Time.deltaTime;
            dir -= Vector3.up * g * Time.deltaTime;
            if (Time.time - timer >= time)
            {
                isShoot = false;
                GameObject basketball = GameObject.FindGameObjectWithTag("basketball");
                basketball.GetComponent<Rigidbody>().useGravity = true;
                _pickupBasketball.pickupAllowed = true;
            }
        }
        
    }

    public void getInitialInformation()
    {
       //transform.LookAt(target);
        initialDir = target.position - this.transform.position;
        time = initialDir.magnitude / speed;
        initialDir =  initialDir.normalized*speed;
        dir = initialDir + 0.5f*g*time*Vector3.up;
        timer = Time.time;
    }
}
