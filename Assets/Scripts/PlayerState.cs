using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoguelikeGame
{
    [RequireComponent(typeof(PlayerCharacter))]
    public class PlayerState : MonoBehaviour
    {
        StateMachine m_playerState;
        PlayerCharacter m_pCharacter;
        Animator m_characterAnim;
        Monster m_npc = null;
        Weapon m_weapon;

        private void Awake()
        {
            m_pCharacter = GetComponent<PlayerCharacter>();
            m_characterAnim = GetComponent<Animator>();
            m_weapon = GetComponent<Weapon>();
            m_playerState = new StateMachine((int)GSDefine.PlayerState.IDLE);
            m_pCharacter.PlayerState = m_playerState;
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

                        }
                        break;
                    }
                case (int)GSDefine.PlayerState.ATTACK:
                    {
                        if(m_playerState.IsEntering())
                        {
                            m_characterAnim.SetTrigger("Attack");
                            if (m_npc)
                                m_weapon.TakeDemage(m_npc);
                        }
                        else
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
    }
}

