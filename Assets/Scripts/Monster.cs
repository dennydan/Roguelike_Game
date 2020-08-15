using UnityEngine;
using UnityEngine.UI;

namespace RoguelikeGame
{
    public class Monster : MonoBehaviour, IDemagable
    {
        [SerializeField] Image m_healthBarImg; 
        [SerializeField] float m_maxHealth = 5;
        float m_health;
        float m_healthFactor = 1.0f;
        int m_NPCLevel = 1;

        bool m_isFacingRight = false;
        RLG_GameState m_gs;

        private void Awake()
        {
            SetHealth();
        }
        private void Update()
        {
            if(IsDead())
            {
                // 目前先直接刪除物件，之後要加死亡動 畫&特效，再刪除

                m_gs.MonsterDead(DeadImplement);
            }
        }

        void IDemagable.Demage(float demage)
        {
            if (IsDead() == false)
            {
                //Debug.Log("M Demage" + demage);
                m_health -= demage;
                m_healthBarImg.fillAmount = GetHealthPercentage();
            }
        }

        public void LookAtPlayer(Vector2 pos)
        {
            if (transform.position.x < pos.x && !m_isFacingRight)
            {
                Filp();
            }
            else if (transform.position.x > pos.x && m_isFacingRight)
            {
                Filp();
            }
        }

        public bool IsDead()
        {
            //Debug.Log("Monster Health" + m_health);
            return m_health <= 0.0f;
        }

        private void DeadImplement()
        {
            Destroy(gameObject);
        }

        void Filp()
        {
            m_isFacingRight = !m_isFacingRight;
            transform.Rotate(0f, 180f, 0f);
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        void SetHealth ()
        {
            m_health = m_maxHealth * (m_NPCLevel * m_healthFactor + 1);
        }

        float GetHealthPercentage()
        {
            float maxHealthFactor = m_maxHealth * (m_NPCLevel * m_healthFactor + 1);
            return m_health / maxHealthFactor;
        }

        public void SetGameState(RLG_GameState gs)
        {
            m_gs = gs;
        }


    }
}


