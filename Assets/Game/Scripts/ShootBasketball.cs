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
    void FixedUpdate()
    {
        target_xoz = target.position;
        target_xoz.y = player.transform.position.y;
        Vector3 player_targetXOZ = target_xoz - player.transform.position;
        player_target_distance = player_targetXOZ.magnitude;//���ݺ�����ľ���ı�������
        if (Input.GetMouseButtonDown(0)&&player.currentState==Player.CharacterState.Dribbling )
        {  
            animator.SetTrigger("Shoot");
            _bsketball.isdrib = false;
            GameObject basketball = GameObject.FindGameObjectWithTag("basketball");
            GameObject righthand = GameObject.FindGameObjectWithTag("rightHand");
            basketball.GetComponent<SphereCollider>().enabled = false;
            basketball.transform.parent = righthand.transform;
            basketball.transform.localPosition = new Vector3(0, 0, player.hand_baskrtball_distance);
           //Ͷ��ʱ���泯���� 
            
            GameObject _player = GameObject.FindGameObjectWithTag("Player");
            _player.transform.LookAt(target_xoz);
            //״̬��Ϊshooting
            player.switchToNewState(Player.CharacterState.Shooting);
            //����ʵ�����
            actualPos = GenerateActualPos(player_target_distance);
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
                actualPos = target;
            }
        }
        

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

    public Transform GenerateActualPos(float distance)//����ʵ�����
    {
        if (distance <= basket_ThreePointLine_distance)
        {
            float k = basket_ThreePointLine_distance / actual_target_distance_max;//���Ա���
            actual_target_distance = distance / k;
        }
        else
        {
            actual_target_distance = actual_target_distance_max;
        }
        Vector3 randomPosition = Random.onUnitSphere * actual_target_distance;
        randomPosition.y = Mathf.Abs(randomPosition.y);
        GameObject generatedActualPos = Instantiate(actualPosPrefab, target.position + randomPosition, Quaternion.identity);
        return generatedActualPos.transform;//����ʵ������transform
    }
}
