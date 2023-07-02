using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bsketball : MonoBehaviour
{
    public GameObject righthand;
    float timer = 1.16f;
    Vector3 dir = new Vector3(0, 0.06f, -0.02f);
    public GameObject camera1;
    public Animator animator;
    public bool isdrib = false;
    public bool isshoot = false;
    Vector3 down = new Vector3(0, 1f, 0);
    public ShootBasketball _shootbasketball;
    public float bounceSpeed = 2f;
    public float bounceMagnitude = 2f;
    public long smoothSpeed = 1;
    public float targetBounceOffset;
    // Start is called before the first frame update
    void Start()
    {
        if(isdrib)
        {
            gameObject.transform.position = righthand.transform.position - dir;
        }
        if (_shootbasketball.isShoot)
        {
            gameObject.transform.position = righthand.transform.position - dir;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isdrib)
        {
            Vector3 targetPosition = righthand.transform.position - dir;
            Vector3 desiredPosition = new Vector3(targetPosition.x, targetBounceOffset, targetPosition.z);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            timer += Time.deltaTime;
            float deylaspeed = targetPosition.y/0.25f;
            if (timer >= 0.4f)
            {
                gameObject.transform.position = righthand.transform.position - dir;
                timer = 0;
            }
            if (timer >= 0.2f && timer <= 0.4f)
            {
                targetBounceOffset = deylaspeed * (timer-0.2f);
            }
            if (timer <= 0.2f)
            {
                targetBounceOffset = targetPosition.y - deylaspeed * timer;
            }
        }
    }
}
