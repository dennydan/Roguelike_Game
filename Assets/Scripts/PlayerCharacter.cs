using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RoguelikeGame
{
    public class PlayerCharacter : MonoBehaviour, IDemagable
    {
        [SerializeField] float m_maxHealth = 5;
        [SerializeField] float m_maxSpeed = 10.0f;
        float m_health;
        float m_healthFactor = 1;
        int CharacterLevel = 1;
        StateMachine FSM;
        public StateMachine  PlayerState { set { FSM = value; } get { return FSM; } }

        private void Awake()
        {
            m_health = m_maxHealth * (CharacterLevel * m_healthFactor + 1);
        }

        private void Update()
        {
            Debug.Log(m_health);
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
        }

        public float GetMaxSpeed()
        {
            Debug.Log("Speed" + m_maxSpeed);
            return m_maxSpeed;
        }

        public void SetMaxSpeed(float speed)
        {
            m_maxSpeed = speed;
        }
    }
}
