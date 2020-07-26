using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RoguelikeGame
{
    [RequireComponent(typeof(PlayerCharacter))]
    public class PlayerState : MonoBehaviour
    {
        [SerializeField] float m_attackInterval = 0.5f;
        [SerializeField] Image m_healthBarImg; 

        StateMachine m_playerState;
        PlayerCharacter m_pc;
        Animator m_characterAnim;
        Monster m_npc = null;
        Weapon m_weapon;

        bool m_bStartAttack = false;
        float m_attackTime = 1.0f;
        float m_combatTime = 1.0f;
        int m_attackCount = 0;

        private void Awake()
        {
            m_pc = GetComponent<PlayerCharacter>();
            m_characterAnim = GetComponent<Animator>();
            m_weapon =  GetComponentInChildren<Weapon>();
            m_playerState = new StateMachine((int)GSDefine.PlayerState.IDLE);
            m_pc.PlayerState = m_playerState;
        }

        void Start()
        {
        }

        void Update()
        {
            int currentState = m_playerState.Tick();
            
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
                        if(HasCooledDown())
                        {
                            m_playerState.NextState((int)GSDefine.PlayerState.IDLE);
                        }
                        break;
                    }
                case (int)GSDefine.PlayerState.DODGE:
                    // 暫不知道怎麼用
                    break;
                case (int)GSDefine.PlayerState.JUMP:
                    {
                        if (m_playerState.IsEntering())
                        {
                            Debug.Log("JUMP");
                        }
                        break;
                    }
                case (int)GSDefine.PlayerState.DIE:
                    {
                        if(m_playerState.IsEntering())
                        {
                            Debug.Log("DIE");
                            Destroy(gameObject);
                        }
                        break;
                    }
                default:
                    break;

            }

        }

        private void FixedUpdate()
        {
            UpdateStatus();
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

        private bool HasCooledDown()
        {
            m_attackTime -= Time.deltaTime;
            if (m_attackTime <= 0)
            {
                switch (m_attackCount)
                {
                    case 0:
                        {
                            m_attackTime = m_attackInterval / 5;
                            break;
                        }
                    case 1:
                        {
                            m_attackTime = m_attackInterval;
                            break;
                        }
                    case 2:
                        {
                            m_attackTime = m_attackInterval;
                            break;
                        }

                    default:
                        break;
                }
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
                if (m_combatTime <= 0 || m_playerState.Current() == (int)GSDefine.PlayerState.JUMP)
                {
                    m_bStartAttack = false;
                    m_attackCount = 0;
                }
            }
        }

        private void UpdateStatus()
        {
            ResetCombo();
            m_healthBarImg.fillAmount = m_pc.GetHealthPercentage();
        }
    }
}

