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
    public GameObject actualPosPrefab;
    private Vector3 initialDir,dir;
    public float g_min;
    public float g_max;
    public float g;
    public float speed;
    private float timer;
    private float time;
    public Player player;
    public pickupBasketball _pickupBasketball;
    public bsketball _bsketball;
    public Vector3 target_xoz;
    private float player_target_distance;
    private float actual_target_distance;
    public float actual_target_distance_max;
    private float basket_ThreePointLine_distance = 0.8f;
    public Transform actualPos;
    public Vector3 player_targetXOZ;
    private float chargingTime = 0f;
    public float maxChargeTime; // 设置蓄力上限时间

    private void Start()
    {
        g = g_min;
    }

    void FixedUpdate()
    {
        target_xoz = target.position;
        target_xoz.y = player.transform.position.y;
        player_targetXOZ = target_xoz - player.transform.position;
        player_target_distance = player_targetXOZ.magnitude;//根据和篮筐的距离改变命中率
        if (Input.GetMouseButton(0)&&player.currentState==Player.CharacterState.Dribbling )
        {
            chargingTime += Time.deltaTime;
            g = g_min + ((g_max - g_min) / maxChargeTime) * chargingTime;
        }
        if ((Input.GetMouseButtonUp(0) || chargingTime >= maxChargeTime)&& player.currentState == Player.CharacterState.Dribbling) // 鼠标松开或达到蓄力上限
        {
            // 进行投篮逻辑，使用当前的 g 值
            startShooting();
            // 重置计时和变量
            chargingTime = 0f;
            
        }

        if (isShoot)
        {
            transform.position += dir*Time.deltaTime;
            dir -= Vector3.up * g * Time.deltaTime;
            if (Time.time - timer >= time)
            {
                stopShooting();
            }
        }
        

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isShoot) stopShooting();    
    }

    public void getInitialInformation()
    {
       //transform.LookAt(target);
        initialDir = actualPos.transform.position - this.transform.position;
        time = initialDir.magnitude / speed;
        initialDir =  initialDir.normalized*speed;
        dir = initialDir + 0.5f*g*time*Vector3.up;
        timer = Time.time;
    }

    public void stopShooting()
    {
        isShoot = false;
        GameObject basketball = GameObject.FindGameObjectWithTag("basketball");
        basketball.GetComponent<Rigidbody>().useGravity = true;
        _pickupBasketball.pickupAllowed = true;
        actualPos = target;
        g = g_min;
    }

    public Transform GenerateActualPos(float distance)//生成实际落点
    {
        if (distance <= basket_ThreePointLine_distance)
        {
            float k = basket_ThreePointLine_distance / actual_target_distance_max;//线性比例
            actual_target_distance = distance / k;
        }
        else
        {
            actual_target_distance = actual_target_distance_max;
        }
        Vector3 randomPosition = Random.onUnitSphere * actual_target_distance;
        randomPosition.y = Mathf.Abs(randomPosition.y);
        //randomPosition.z = -Mathf.Abs(randomPosition.z);
        GameObject generatedActualPos = Instantiate(actualPosPrefab, target.position + randomPosition, Quaternion.identity);
        return generatedActualPos.transform;//返回实际落点的transform
    }

    public void startShooting()
    {
        animator.SetTrigger("Shoot");
        _bsketball.isdrib = false;
        GameObject basketball = GameObject.FindGameObjectWithTag("basketball");
        GameObject righthand = GameObject.FindGameObjectWithTag("rightHand");
        basketball.GetComponent<SphereCollider>().enabled = false;
        basketball.transform.parent = righthand.transform;
        basketball.transform.localPosition = new Vector3(0, 0, player.hand_baskrtball_distance);
        //投篮时候面朝篮筐 

        GameObject _player = GameObject.FindGameObjectWithTag("Player");
        _player.transform.LookAt(target_xoz);
        //状态变为shooting
        player.switchToNewState(Player.CharacterState.Shooting);
        //生成实际落点
        actualPos = GenerateActualPos(player_target_distance);
    }
}
