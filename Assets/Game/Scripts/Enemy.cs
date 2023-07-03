using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Player player;          // 玩家对象
    private NavMeshAgent agent;       // 导航代理
    private Animator animator;        // 动画控制器
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
        // 设置导航代理的目标为玩家的位置
        Vector3 offset = _shootbasketball.player_targetXOZ.normalized * defence_distance;
        agent.SetDestination(player.transform.position + offset);

        // 更新动画参数（行走速度）
        float speed = agent.velocity.magnitude;
        animator.SetFloat("speed", speed);
        // 调整敌人朝向玩家
        transform.LookAt(player.transform);
        //站定时开始计时，超时则抢断
        if (judgeIsDestinationReached()&&player.currentState==Player.CharacterState.Dribbling)
        {
            // 开始计时
            timer += Time.deltaTime;
            // 判断是否超过一定时间
            if (timer >= defence_time)
            {
                // 执行某个函数
                Debug.Log("successfully defend!");
                timer = 0f;
            }
        }
        else
        {
            // 清零计时器
            timer = 0f;
        }
    }

    public bool judgeIsDestinationReached()
    {
        // 检查导航代理是否到达目的地
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
