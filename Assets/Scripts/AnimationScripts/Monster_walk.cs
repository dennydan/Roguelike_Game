﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeGame
{
    public class Monster_walk : StateMachineBehaviour
    {
        [SerializeField] float moveSpeed = 1f;
        [SerializeField] float attackRange = 1f;

        Transform m_player;
        Rigidbody2D m_rigidbody;
        Monster m_monster;
        Weapon m_weapon;
        PlayerCharacter m_playerCharacter;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_player = GameObject.FindGameObjectWithTag("Player").transform;
            m_rigidbody = animator.GetComponent<Rigidbody2D>();
            m_monster = animator.GetComponent<Monster>();
            m_playerCharacter = m_player.GetComponent<PlayerCharacter>();
            m_weapon = animator.GetComponent<Weapon>();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (m_monster.IsDead() || m_player == null)
            {
                return;
            }
            /*  
            if (m_playerCharacter.IsDead())
            {
                IdleWalk();
                return;
            }
            */
            //TODO: 怪物攻擊速度過快( or 玩家受傷速度過快)
            else if (CanAttack())
            {
                animator.SetTrigger("NPCAttack");
                m_weapon.Attack((int)AttackType.MONSTER, 1.0f, 1.0f);
                return;
            }
            Vector2 target = new Vector2(m_player.position.x, m_rigidbody.position.y);
            Vector2 newPosition = Vector2.MoveTowards(m_rigidbody.position, target, moveSpeed * Time.fixedDeltaTime);
            m_rigidbody.MovePosition(newPosition);
            m_monster.LookAtPlayer(m_player.position);
            
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

        void IdleWalk()
        {
            //  腳色死亡後隨機亂走 for test 
            //Debug.Log("Die");
            Vector2 target_ = new Vector2(Random.Range(-5, 5), m_rigidbody.position.y);
            Vector2 newPosition_ = Vector2.MoveTowards(m_rigidbody.position, target_, moveSpeed * Time.fixedDeltaTime);
            m_rigidbody.MovePosition(newPosition_);
            m_monster.LookAtPlayer(target_);
        }

        bool CanAttack()
        {
            return m_rigidbody && m_player && 
                Mathf.Abs(m_rigidbody.position.x - m_player.position.x) < attackRange;
        }
    }
}
