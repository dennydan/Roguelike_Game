using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RoguelikeGame
{
    public class PlayerCharacter : MonoBehaviour, IDemagable
    {
        [SerializeField] float m_maxHealth = 5;
        [SerializeField] float m_maxSpeed = 10.0f;
        float m_exp = 0;
        float m_health;
        float m_healthFactor = 0.1f;
        int CharacterLevel = 1;
        StateMachine FSM;
        public StateMachine  PlayerState { set { FSM = value; } get { return FSM; } }

        private void Awake()
        {
            SetHealth();
        }

        private void Update()
        {
            //Debug.Log(m_health);
        }

        public bool IsDead()
        {
            return m_health <= 0;
        }

        void IDemagable.Demage(float demage)
        {
            if (!IsDead())
            {
                m_health -= demage;
                Debug.Log("Health :　" + m_health);
            }
            else
            {
                PlayerState.NextState((int)GSDefine.PlayerState.DIE);
            }
        }

        public float GetMaxSpeed()
        {
            //Debug.Log("Speed" + m_maxSpeed);
            return m_maxSpeed;
        }

        public void SetMaxSpeed(float speed)
        {
            m_maxSpeed = speed;
        }

        private void SetHealth()
        {
            m_health = m_maxHealth * (CharacterLevel * m_healthFactor + 1);
        }

        public float GetHealthPercentage()
        {
            float maxHealthFactor = m_maxHealth * (CharacterLevel * m_healthFactor + 1);
            return m_health / maxHealthFactor;
        }
        public bool CanAttack()
        {
            return PlayerState.Current() == (int)GSDefine.PlayerState.IDLE
                || PlayerState.Current() == (int)GSDefine.PlayerState.JUMP;
        }
    }
}
