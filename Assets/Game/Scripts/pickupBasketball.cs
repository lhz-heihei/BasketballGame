using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class pickupBasketball : MonoBehaviour
{
    public Player _player;
    public Collider basketball_collider;
    public bool pickupAllowed = true;
    public bsketball _bsketball;

    private void OnTriggerEnter(Collider other)
    {
        if (pickupAllowed)
        {
            if (other.tag == "basketball")
            {
                basketball_collider = other.GetComponent<SphereCollider>();
                basketball_collider.enabled = false;    
                Rigidbody rigidbody = other.GetComponent<Rigidbody>();
                rigidbody.useGravity = false;

                //_bsketball.isdrib = true;
                //other.transform.parent = righthand.transform;
                //other.transform.localPosition = new Vector3(0, 0, _player.hand_baskrtball_distance);
                other.transform.localRotation = Quaternion.Euler(Vector3.zero);
                _player.switchToNewState(Player.CharacterState.Dribbling);
            }
        }

        
        
    }

}
