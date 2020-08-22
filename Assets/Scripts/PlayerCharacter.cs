using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RoguelikeGame
{
    public class PlayerCharacter : MonoBehaviour, IDemagable
    {
        [SerializeField] float m_maxHealth = 5;
        [SerializeField] float m_maxSpeed = 10.0f;
        [SerializeField] RLG_GameState m_gs;
        int m_level = 1;
        float m_health;
        float m_healthFactor = 0.1f;
        float m_exp = 0;
        float m_expFactor = 3.0f;
        float m_maxExp;
        StateMachine FSM;
        public StateMachine  PlayerState { set { FSM = value; } get { return FSM; } }

        private void Awake()
        {
            InitStatus();
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
            return m_maxSpeed;
        }

        public void SetMaxSpeed(float speed)
        {
            m_maxSpeed = speed;
        }

        private void InitStatus()
        {
            m_level = 1;    // 目前先寫死之後從存檔抓
            m_health = m_maxHealth * (m_level * m_healthFactor + 1);
            m_maxExp = m_level * m_expFactor;
        }
        public int GetLevel()
        {
            return m_level;
        }
        public float GetHealthPercentage()
        {
            float maxHealthFactor = m_maxHealth * (m_level * m_healthFactor + 1);
            return m_health / maxHealthFactor;
        }
        public float GetExpPercentage()
        {
            return m_exp / m_maxExp;
        }

        public void AddCharacterExp(float exp)
        {
            m_exp = m_exp + exp;
            if (m_exp >= m_maxExp)
                UpgradeLevel();
        }
        public void UpgradeLevel()
        {
            m_level++;
            m_exp = m_exp - m_maxExp;
            m_maxExp = m_level * m_expFactor;
        
        }
        public bool CanAttack()
        {
            return PlayerState.Current() == (int)GSDefine.PlayerState.IDLE
                || PlayerState.Current() == (int)GSDefine.PlayerState.JUMP;
        }
    }
}
