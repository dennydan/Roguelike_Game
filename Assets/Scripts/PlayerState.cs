using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeGame
{
    [RequireComponent(typeof(PlayerCharacter))]
    public class PlayerState : MonoBehaviour
    {
        [SerializeField] float m_attackInterval = 0.5f;

        StateMachine m_playerState;
        PlayerCharacter m_pCharacter;
        Animator m_characterAnim;
        Monster m_npc = null;
        Weapon m_weapon;

        bool m_bStartAttack = false;
        float m_attackTime = 1.0f;
        float m_combatTime = 1.0f;
        int m_attackCount = 0;

        private void Awake()
        {
            m_pCharacter = GetComponent<PlayerCharacter>();
            m_characterAnim = GetComponent<Animator>();
            m_weapon =  GetComponentInChildren<Weapon>();
            m_playerState = new StateMachine((int)GSDefine.PlayerState.IDLE);
            m_pCharacter.PlayerState = m_playerState;
        }

        void Start()
        {
            m_attackTime = m_attackInterval;
            m_combatTime = m_attackInterval;
        }

        void Update()
        {
            int currentState = m_playerState.Tick();
            ResetCombo();
            switch (currentState)
            {
                case (int)GSDefine.PlayerState.IDLE:
                    {
                        if (m_playerState.IsEntering())
                        {
                            Debug.Log("IDLE");
                        }

                        break;
                    }
                case (int)GSDefine.PlayerState.ATTACK:
                    {
                        if (m_playerState.IsEntering())
                        {
                            m_characterAnim.SetTrigger("Attack");
                            AttackCombat();
                        }
                        if(CanAttack())
                        {
                            m_playerState.NextState((int)GSDefine.PlayerState.IDLE);
                        }
                        break;
                    }
                case (int)GSDefine.PlayerState.DODGE:
                    // 暫不知道怎麼用
                    break;
                case (int)GSDefine.PlayerState.JUMP:
                    // 暫不使用 
                    break;
                case (int)GSDefine.PlayerState.DIE:
                    break;
                default:
                    break;

            }

        }


        private void AttackCombat()
        {

            m_combatTime = m_attackInterval;
            switch (m_attackCount)
            {
                case 0:
                    {
                        m_bStartAttack = true;
                        m_weapon.Attack((int)AttackType.PLAYER, 1.0f, 1.0f);
                        m_attackCount++;
                        break;
                    }
                case 1:
                    {
                        m_weapon.Attack((int)AttackType.PLAYER, 1.2f, 1.2f);
                        m_attackCount++;
                        break;
                    }
                case 2:
                    {
                        m_weapon.Attack((int)AttackType.PLAYER, 2.0f, 2.0f);
                        m_attackCount = 0;
                        break;
                    }

                default:
                    break;
            }
        }

        private bool CanAttack()
        {
            m_attackTime -= Time.deltaTime;
            if (m_attackTime <= 0)
            {
                m_attackTime = m_attackInterval;
                m_combatTime = m_attackInterval;
                return true;
            }
            else
               return false;
        }

        private void ResetCombo()
        {
            if(m_bStartAttack)
            {
                m_combatTime -= Time.deltaTime;
                if (m_combatTime <= 0)
                {
                    m_bStartAttack = false;
                    m_attackCount = 0;
                }
            }
        }
        /*
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
                m_npc = other.GetComponent<Monster>();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
                m_npc = null;
        }
        */
    }
}

