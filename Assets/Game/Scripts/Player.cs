using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float move_speed=2;
    public bsketball _bsketball;
    private float horizontal, vertical;
    public CharacterController cc;
    public Transform FollowedCamera;
    private float gravity = -9.8f;
    public float jump_speed;
    private Vector3 dir_init,moveDir;
    private Animator animator;
    public bool ispicking = false;
    public float hand_baskrtball_distance;
    public ShootBasketball _shootbasketball;
    public pickupBasketball _pickupBasketball;
    public enum CharacterState { Normal,Dribbling}
    public CharacterState currentState = CharacterState.Normal;
    private void Start()
    {
        animator = GetComponent<Animator>();
        
    }
    private void FixedUpdate()
    {
        
        switch(currentState)
        {
            case CharacterState.Normal:
                PlayerMovement();
                animator.SetFloat("Speed", dir_init.magnitude);
                
                break;
            case CharacterState.Dribbling:
                PlayerMovement();
                /* if (Input.GetKeyDown(KeyCode.LeftShift))
                 {
                     move_speed = 4;
                     animator.SetBool("iserupt", true);  //运球爆发
                     animator.SetBool("Walk", false);
                 } //使用幻数，危险
                 if (Input.GetKeyUp(KeyCode.LeftShift))
                 {
                     move_speed = 2;
                     animator.SetBool("iserupt", false);
                 }
                 if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && Input.GetKey(KeyCode.LeftShift))
                 {
                     animator.SetBool("Walk", false);
                     Debug.Log("false");
                 }
                     if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && !Input.GetKey(KeyCode.LeftShift))
                 {
                     animator.SetBool("Walk", true);
                     Debug.Log("ja");
                 }
                 else
                 {
                     animator.SetBool("Walk", false);
                 } */
                bool isShiftPressed = Input.GetKey(KeyCode.LeftShift);
                bool isMovementKeyPressed = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);

                if (isShiftPressed && isMovementKeyPressed)
                {
                    animator.SetBool("Walk", false);
                    animator.SetBool("iserupt", true);
                    move_speed = 4;
                }
                else if (isShiftPressed)
                {
                    animator.SetBool("Walk", false);
                    animator.SetBool("iserupt", true);
                    move_speed = 2;
                }
                else if (isMovementKeyPressed)
                {
                    animator.SetBool("Walk", true);
                    animator.SetBool("iserupt", false);
                    move_speed = 2;
                }
                else
                {
                    animator.SetBool("Walk", false);
                    animator.SetBool("iserupt", false);
                }
                break;
        }
    }

    void PlayerMovement()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        dir_init = new Vector3(horizontal, 0, vertical);
        
        moveDir = FollowedCamera.forward * vertical + FollowedCamera.right * horizontal;
        moveDir.y = 0;
        if (moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDir);
            animator.SetBool("Walk", true);
        }
        cc.Move( transform.forward*dir_init.magnitude * move_speed * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Space)&&!ispicking)
        {
            animator.SetTrigger("Jump");
            
        }
        if (!cc.isGrounded)
        {

            cc.Move(Vector3.up * gravity * Time.deltaTime);
        }
    }

    public void switchToNewState(CharacterState newState)
    {
        switch (currentState)
        {
            case CharacterState.Normal:
                break;
            case CharacterState.Dribbling:
                break;
        }

        switch (newState)
        {
            case CharacterState.Normal:
                ispicking = false;
                break;
            case CharacterState.Dribbling:
                animator.SetBool("pickup", true);
                ispicking = true;
                _bsketball.isdrib = true;
                break;
        }
        currentState = newState;
    }
    void shoot()
    {
        _shootbasketball.isShoot = true;
        _pickupBasketball.pickupAllowed = false;
       // _bsketball.isdrib = false;
        animator.SetBool("pickup", false);
        switchToNewState(CharacterState.Normal);
        GameObject basketball = GameObject.FindGameObjectWithTag("basketball");
        basketball.transform.parent = null;
        basketball.GetComponent<SphereCollider>().enabled = true;
        _shootbasketball.getInitialInformation();
    }
}
