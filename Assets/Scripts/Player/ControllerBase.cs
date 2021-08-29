using UnityEngine;

/*
    Created by DennyLiu on 2021.08.07
    Base 不做動畫控制
    繼承者須自行處理
    1. 落地判斷
    2. 動畫、狀態處理
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

        // 落地相關
        bool m_bGrounded = false;
        const float m_groundRadius = 0.2f;
        // 跳躍相關
        bool m_bJump = false;
        bool m_bJumpingUp = false;
        float m_jumpForce = 10f;        // 跳躍力道
        float m_jumpCount = 0f;         // 要reset否則跳的高度會不一定
        float m_maxJumpTime = 2.0f;
        float m_gravityFactor = -20f;
        // 閃躲相關，目前遇到障礙物也會穿過去
        bool m_bDodge = false;
        bool m_bDodging = false;
        float m_dodgeCount = 0f;
        float m_dodgeFactor = 5f;
        float m_dodgeCD = 1f;
        float m_dodgeDuration = 0.3f;


        Rigidbody2D m_charactorRigidBody;
        Vector2 m_speedVec = new Vector2(0, 0);
        float m_maxSpeed = 60f;
        float m_speedFactor = 0f;
        bool m_bFacingRight = true;

        protected void Awake()
        {
            Debug.Log("PlayerControllerBase_Awake");
            m_groundCheck = transform.Find(m_groundCheckName);
            m_ceilingCheck = transform.Find(m_ceilingCheckName);
            m_charactorRigidBody = GetComponent<Rigidbody2D>();
            m_charactorRigidBody.centerOfMass = m_centerOfMass.position;

        }

        protected void Start()
        {
            
        }

        // Update is called once per frame
        protected void Update()
        {
            ActionImplement();
            if (Input.GetButtonUp("Jump")) m_bJumpingUp  = false;
        }

        protected void FixedUpdate()
        {
            SetOnTheGround();
        }

        // 實作角色動作移動
        // 怪物controller也應該繼承此類別，控制變數要再討論
        private void ActionImplement()
        {
            // 閃躲
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
            ResetAndCheckAction();
        }
        private void ResetAndCheckAction()
        {
            JumpingCheck();
            DodgeCheck();
            m_bJump = false;
            m_bDodge = false;
        }

        // 移動
        protected virtual void Move(float moveSpeed)
        {
            Debug.Log("ControllerBase_Move");
            Flip(moveSpeed);
            m_charactorRigidBody.velocity = new Vector2(moveSpeed, m_charactorRigidBody.velocity.y);
            MaxSpeedCheck();
        }

        // 跳躍高度判斷
        private bool JumpingCheck()
        {
            if (m_jumpCount < m_maxJumpTime && m_bJumpingUp)
            {
                m_jumpCount += Time.fixedDeltaTime;    //跳高計時
            }
            else
            {
                m_jumpCount = 0;
                m_bJumpingUp = false;
            }
            if ( m_bJump )
            {
                m_bJumpingUp = true;
            }

            if (m_bJumpingUp)
            {
                Debug.Log("jumpingUp");
                m_charactorRigidBody.velocity = new Vector2(m_charactorRigidBody.velocity.x, m_jumpForce);
            }
            else if (!m_bJumpingUp)
            {
                Debug.Log("jumpingDown");
                m_charactorRigidBody.AddForce(new Vector2(0f, m_gravityFactor));
            }
            return false;
        }

        // 最大限速
        private void MaxSpeedCheck()
        {
            float speedX = m_charactorRigidBody.velocity.x > m_maxSpeed ? m_maxSpeed : m_charactorRigidBody.velocity.x;
            float speedY = m_charactorRigidBody.velocity.y > m_maxSpeed ? m_maxSpeed : m_charactorRigidBody.velocity.y;
            speedX = speedX < -m_maxSpeed ? -m_maxSpeed : speedX;
            speedY = speedY < -m_maxSpeed ? -m_maxSpeed : speedY;
            m_charactorRigidBody.velocity = new Vector2(speedX, speedY);
        }

        // 閃躲判斷
        private void DodgeCheck()
        {
            if (m_dodgeCount < m_dodgeCD) m_dodgeCount += Time.fixedDeltaTime;    //冷卻判斷計時
            if (m_dodgeCount >= m_dodgeDuration)
            {
                m_bDodging = false;    //無敵(類似Buff)判斷
            }
            //if (m_isGrounded && dodge && dodgeCount >= dodgeCD)
            if (m_bDodge && m_dodgeCount >= m_dodgeCD)                       //迴避判斷
            {
                m_dodgeCount = 0;
                m_bDodging = true;
            }
            if (m_bDodging)
            {
                //TODO: 如何變更碰撞Layer? 與碰撞回彈扣血狀況類似
                //characterBoxCollider.
            }
           
            //迴避中欲變更方向
            if (m_bDodging)
            {
                if ((!m_bFacingRight && Input.GetKey(KeyCode.RightArrow)) || (m_bFacingRight && Input.GetKey(KeyCode.LeftArrow)))
                {
                    m_bDodging = false;
                }
                else
                {
                    //m_characterRigidBody.velocity = new Vector2(moveSpeed * speed * dodgeFactor, characterRigidBody.velocity.y);
                }
                //if (m_isGrounded && jump) dodging = false;   //跳躍取消
            }
            //if (!dodging)
            //{
            //    if (!m_isGrounded && jumpingUp)
            //    {
            //        Debug.Log("jumpingUp");
            //        characterRigidBody.velocity = new Vector2(characterRigidBody.velocity.x, jumpForce);
            //    }
            //    else if (!m_isGrounded && !jumpingUp)
            //    {
            //        Debug.Log("jumpingDown");
            //        characterRigidBody.AddForce(new Vector2(0f, gravityFactor));
            //    }
            //}
            //else
            //{
            //    characterRigidBody.velocity = new Vector2(characterRigidBody.velocity.x, 0f);
            //}
        }

        //跳躍
        protected virtual void Jump()
        {
            Debug.Log("ControllerBase_Jump");
            m_bJump = true;
        }

        // 閃躲
        protected virtual void Dodge()
        {
            Debug.Log("ControllerBase_Dodge");
            m_bDodge = true;
        }

        // 攻擊
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

        private void SetOnTheGround()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_groundCheck.position, m_groundRadius, m_groundLayer);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    m_bGrounded = true;
                }
            }
        }

    }


}
