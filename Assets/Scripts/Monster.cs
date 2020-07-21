using UnityEngine;

namespace RoguelikeGame
{
    public class Monster : MonoBehaviour, IDemagable
    {
        [SerializeField] float m_maxHealth = 5;
        float m_health;
        float m_healthFactor = 1.0f;
        int NPCLevel = 1;

        bool isFacingRight = false;

        private void Update()
        {
            if(IsDead())
            {
                // 目前先直接刪除物件，之後要加死亡動畫&特效，再刪除
                Destroy(gameObject);  
            }
        }

        private void Awake()
        {
            m_health = m_maxHealth * (NPCLevel * m_healthFactor + 1);
        }

        void IDemagable.Demage(float demage)
        {
            if (!IsDead())
            {
                Debug.Log("M Demage" + demage);
                m_health -= demage;
                Debug.Log("Monster Health :　" + m_health);
            }
        }

        public void LookAtPlayer(Vector2 pos)
        {
            if (transform.position.x < pos.x && !isFacingRight)
            {
                Filp();
            }
            else if (transform.position.x > pos.x && isFacingRight)
            {
                Filp();
            }
        }

        public bool IsDead()
        {
            return m_health <= 0;
        }

        void Filp()
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}


