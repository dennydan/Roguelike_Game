using UnityEngine;

/*
 * 已知BUG:
 *  1.踩到敵人算落地 可跳敵人(你馬力歐?)
 *  2.撞牆算落地 (延伸問題:斜牆可爬?)
 * */

namespace RoguelikeGame
{
    [RequireComponent(typeof(PlayerCharacter))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float maxSpeed = 1f;
        [SerializeField] float jumpForce = 100f;
        [SerializeField] LayerMask Ground_Layer;
        [SerializeField] string groundCheckName = "GroundCheck";
        [SerializeField] string ceilingCheckName = "CeilingCheck";
        [SerializeField] Transform centerOfMass;

        PlayerCharacter m_pCharacter;
        Transform m_groundCheck;                      //地面檢查
        const float k_GroundRadius = 0.2f;
        bool m_isGrounded;
        Transform m_ceilingCheck;                     //天花板檢查
        const float k_CeilingRadius = 0.1f;
        Animator m_characterAnim;
        Rigidbody2D characterRigidBody;
        bool m_bFacingRight = true;
        bool m_bJump = false;
        float m_speedFactor = 0f;

        private void Awake()
        {
            m_groundCheck = transform.Find(groundCheckName);
            m_ceilingCheck = transform.Find(ceilingCheckName);
            m_characterAnim = GetComponent<Animator>();
            characterRigidBody = GetComponent<Rigidbody2D>();
            characterRigidBody.centerOfMass = centerOfMass.position;
            m_pCharacter = GetComponent<PlayerCharacter>();
        }

        private void Update()
        {
            m_speedFactor = Mathf.Lerp(0f, maxSpeed, 0.01f) * Time.fixedDeltaTime;
            if (Input.GetButtonDown("Jump"))
            {
                m_bJump = true;
            }
            else if(Input.GetMouseButtonDown(0) && m_pCharacter.PlayerState.Current() == (int)GSDefine.PlayerState.IDLE)
            {
                m_pCharacter.PlayerState.NextState((int)GSDefine.PlayerState.ATTACK);
            }
        }

        private void FixedUpdate()
        {
            Set_isGrounded();
            m_characterAnim.SetFloat("vSpeed", characterRigidBody.velocity.y);
            CharacterAction();
        }

        void CharacterAction()
        {
            // 左右移動  
            Move(Input.GetAxisRaw("Horizontal") * m_speedFactor, false, m_bJump);
            m_bJump = false;
        }

        void Move(float moveSpeed, bool crouch, bool jump)
        {
            //Crouch,  接動畫(蹲下)
            //..

            //Moving, 接動畫(左右移動)
            //if (isGrounded)
            //{
            if (moveSpeed > 0 && !m_bFacingRight)
                Flip();
            else if (moveSpeed < 0 && m_bFacingRight)
                Flip();
            characterRigidBody.velocity = new Vector2(moveSpeed * maxSpeed, characterRigidBody.velocity.y);

            moveSpeed = Mathf.Abs(moveSpeed);
            m_characterAnim.SetFloat("Speed", moveSpeed);
            m_characterAnim.SetBool("FacingRight", m_bFacingRight);

            //}

            //Jump, 接動畫(跳躍)
            if (m_isGrounded && jump)
            {
                m_isGrounded = false;
                m_characterAnim.SetBool("Ground", m_isGrounded);
                characterRigidBody.AddForce(new Vector2(0f, jumpForce));
            }
        }

        void Set_isGrounded()
        {
            m_isGrounded = false;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_groundCheck.position, k_GroundRadius, Ground_Layer);    //  TODO:撞牆不能算落地; 踩到敵人算落地 可跳敵人(你馬力歐?)
            for (int i = 0; i < colliders.Length; i++)                                                                  //  p.s. 要設定tag 設定種類再討論
            {
                if (colliders[i].gameObject != gameObject)
                    m_isGrounded = true;
            }
            m_characterAnim.SetBool("Ground", m_isGrounded);
        }

        void Flip()
        {
            m_bFacingRight = !m_bFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}

