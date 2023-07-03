using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Player player;          // ��Ҷ���
    private NavMeshAgent agent;       // ��������
    private Animator animator;        // ����������
    public float defence_distance;
    public ShootBasketball _shootbasketball;
    private float timer = 0f;
    public float defence_time;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // ���õ��������Ŀ��Ϊ��ҵ�λ��
        Vector3 offset = _shootbasketball.player_targetXOZ.normalized * defence_distance;
        agent.SetDestination(player.transform.position + offset);

        // ���¶��������������ٶȣ�
        float speed = agent.velocity.magnitude;
        animator.SetFloat("speed", speed);
        // �������˳������
        transform.LookAt(player.transform);
        //վ��ʱ��ʼ��ʱ����ʱ������
        if (judgeIsDestinationReached()&&player.currentState==Player.CharacterState.Dribbling)
        {
            // ��ʼ��ʱ
            timer += Time.deltaTime;
            // �ж��Ƿ񳬹�һ��ʱ��
            if (timer >= defence_time)
            {
                // ִ��ĳ������
                Debug.Log("successfully defend!");
                timer = 0f;
            }
        }
        else
        {
            // �����ʱ��
            timer = 0f;
        }
    }

    public bool judgeIsDestinationReached()
    {
        // ��鵼�������Ƿ񵽴�Ŀ�ĵ�
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}
