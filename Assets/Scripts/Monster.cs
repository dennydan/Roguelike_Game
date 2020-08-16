using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;

namespace RoguelikeGame
{
    public class Monster : MonoBehaviour, IDemagable
    {
        [SerializeField] StatusWidget m_statusWidget;
        [SerializeField] float m_maxHealth = 5;
        float m_health;
        float m_healthFactor = 1.0f;
        float m_expValue = 1.0f;
        int m_level = 1;

        bool m_isFacingRight = false;
        RLG_GameState m_gs;

        private void Awake()
        {
            SetHealth();
        }
        private void Start()
        {
            m_statusWidget.UpdateStatus(m_level, GetHealthPercentage(), 0.0f);
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
                // 怪物目前狀態只有更新血量，故放在這邊
                m_statusWidget.UpdateStatus(m_level, GetHealthPercentage(), 0.0f);
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
            m_gs.UpdateCharacterExp(m_expValue);
            Destroy(gameObject);
        }

        //TODO: 人物怪物轉向 血條也轉向
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
            m_health = m_maxHealth * (m_level * m_healthFactor + 1);
            m_expValue = m_level + 1.0f;      
        }

        float GetHealthPercentage()
        {
            float maxHealthFactor = m_maxHealth * (m_level * m_healthFactor + 1);
            return m_health / maxHealthFactor;
        }

        public void SetGameState(RLG_GameState gs)
        {
            m_gs = gs;
        }
        public void UpgradeLevel(int level)
        {
            m_level = m_level + level;
            SetHealth();
        }
    }
}


