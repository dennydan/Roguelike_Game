using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDemagable
{
    void Demage(float demage);
}


namespace RoguelikeGame
{
    enum AttackType
    {
        PLAYER = 1,
        MONSTER
    }

    public class Weapon : MonoBehaviour
    {
        [SerializeField] float m_demage = 2;
        [SerializeField] float m_demageRange = 1.0f;
        [SerializeField] LayerMask mask;
        Monster m_monster;


        public void Attack(int type, float demageFactor, float rangeFactor)
        {
            bool isAttacked = true;
            // 玩家攻擊
            if(type == (int)AttackType.PLAYER)
            {
                Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, m_demageRange * rangeFactor, LayerMask.GetMask("Monster"));
                foreach (Collider2D hit in hits)
                {
                    // Debug.Log("PLAYER");
                    Monster npc = hit.GetComponent<Monster>();
                    if (isAttacked && npc)
                    {
                        isAttacked = false;
                        TakeDemage(npc, demageFactor);
                    }
                }
            }
            // 怪物攻擊
            else if(type == (int)AttackType.MONSTER)
            {
                Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, m_demageRange * rangeFactor, LayerMask.GetMask("Player"));
                foreach (Collider2D hit in hits)
                {
                    PlayerCharacter pc = hit.GetComponent<PlayerCharacter>();
                    if (isAttacked && pc)
                    {
                        isAttacked = false;
                        TakeDemage(pc, demageFactor);
                    }
                }
            }
            
        }
        public void TakeDemage(IDemagable demagable, float demageFactor)
        {
            //Debug.Log("Demage : " + demageFactor.ToString());
            demagable.Demage(m_demage * demageFactor);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, m_demageRange * 1.0f);
        }
    }
}
