using UnityEngine;

/*
    Created by DennyLiu on 2021.08.07
    Base 不做動畫控制
*/

namespace RoguelikeGame
{
    public class ControllerBase : MonoBehaviour
    {
        [SerializeField] LayerMask m_groundLayer;                       
        [SerializeField] float m_speed = 150f;                       // 移動速度
        [SerializeField] string m_groundCheckName = "GroundCheck";   // 地板檢查  (名稱)
        [SerializeField] string m_ceilingCheckName = "CeilingCheck"; // 天花板檢查(名稱)
        [SerializeField] Transform m_centerOfMass;                   // 重心
        Transform m_groundCheck;
        Transform m_ceilingCheck;
        float m_jumpForce = 10f;            // 跳躍力道

        Rigidbody2D m_charactorRigidBody;
        Vector2 m_speedVec = new Vector2(0, 0);
        float m_maxSpeed = 60f;
        float m_speedFactor = 0f;
        bool m_bFacingRight = true;

        private void Awake()
        {
            m_groundCheck = transform.Find(m_groundCheckName);
            m_ceilingCheck = transform.Find(m_ceilingCheckName);
            m_charactorRigidBody = GetComponent<Rigidbody2D>();
            m_charactorRigidBody.centerOfMass = m_centerOfMass.position;
        }

        void Start()
        {
            
        }
    
        // Update is called once per frame
        void Update()
        {
            ActionImplement();
        }

        private void FixedUpdate()
        {
            
        }

        // 實作角色動作移動
        // 怪物controller也應該繼承此類別，控制變數要再討論
        private void ActionImplement()
        {
            // 閃避
            if (Input.GetButtonDown("Dodge"))
            {
                Dodge();
            }
            // 跳躍
            else if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
            // 攻擊
            else if (Input.GetMouseButtonDown(0))
            {
                Attack();
            }
            // 左右移動
            m_speedFactor = Mathf.Lerp(0f, m_speed, 0.01f);
            Move(Input.GetAxisRaw("Horizontal") * m_speedFactor);
        }
        protected virtual void Move(float moveSpeed)
        {
            Debug.Log("ControllerBase_Move");
            Flip(moveSpeed);
            m_charactorRigidBody.velocity = new Vector2(moveSpeed, m_charactorRigidBody.velocity.y);
            MaxSpeedCheck();
        }

        private void MaxSpeedCheck()
        {
            float speedX = m_charactorRigidBody.velocity.x > m_maxSpeed ? m_maxSpeed : m_charactorRigidBody.velocity.x;
            float speedY = m_charactorRigidBody.velocity.y > m_maxSpeed ? m_maxSpeed : m_charactorRigidBody.velocity.y;
            speedX = speedX < -m_maxSpeed ? -m_maxSpeed : speedX;
            speedY = speedY < -m_maxSpeed ? -m_maxSpeed : speedY;
            m_charactorRigidBody.velocity = new Vector2(speedX, speedY);
        }

        protected virtual void Jump()
        {
            Debug.Log("ControllerBase_Jump");
        }
        protected virtual void Dodge()
        {
            Debug.Log("ControllerBase_Dodge");
        }

        protected virtual void Attack()
        {
            Debug.Log("ControllerBase_Attack");
        }

        // 轉向
        private void Flip(float moveSpeed)
        {
            if ((moveSpeed > 0 && !m_bFacingRight) || (moveSpeed < 0 && m_bFacingRight))
            {
                m_bFacingRight = !m_bFacingRight;
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }
        }
    }


}
