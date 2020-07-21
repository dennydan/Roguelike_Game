using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDemagable
{
    void Demage(float demage);
}


namespace RoguelikeGame
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] float m_demage = 2;
        [SerializeField] float m_demageRange = 1.0f;

        int m_attackCount = 0;

        Monster m_monster;
        void Start()
        {
          
        }

        public void TakeDemage(IDemagable demagable)
        {
            switch(m_attackCount)
            {
                case 0:
                    // add Delay
                    StartCoroutine(AttackCounter());
                    demagable.Demage(m_demage);
                    break;
                case 1:
                    demagable.Demage(m_demage*1.2f);
                    break;
                case 2:
                    demagable.Demage(m_demage*2);
                    break;

                default:
                    break;
            }
        }

        IEnumerator AttackCounter()
        {
            m_attackCount++;
            yield return new WaitForSeconds(1.0f);
            m_attackCount = 0;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Monster m_monster = other.GetComponent<Monster>();
            PlayerCharacter pc = other.GetComponent<PlayerCharacter>();
            if (m_monster)
                TakeDemage(m_monster);
        }
    }
}
