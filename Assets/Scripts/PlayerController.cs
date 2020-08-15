﻿using UnityEngine;

/*
 * 已知BUG:
 *  1.踩到敵人算落地 可跳敵人(你馬力歐?)
 *  2.撞牆算落地
 *      (1)牆壁算是地板，因此貼牆算落地
 *      (2)牆壁不算落地，只是不能跳躍，一樣會卡住
 *  
 *  2020/08/09
 *  目前遊戲內尺寸測量: 
    1.跳躍高度約2格高
    2.跳躍長度約3格遠
    3.走路速度約3格/s
    TODO: 世界標準、變數名稱規則
    TODO: 迴避無敵(等生命值)、納入技能系統、納入狀態機

    +角色能力修正、統一(跑速、跳躍高度)
    +地形_1x1(Environment物件放地形)
    TODO:如何重設為標準? 地板跟牆壁如何區分?

    +prefab修正座標、旋轉
    +房間微調
 *  
 * */

namespace RoguelikeGame
{
    [RequireComponent(typeof(PlayerCharacter))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float speed = 150f;
        [SerializeField] float jumpForce = 320f;
        [SerializeField] LayerMask Ground_Layer;
        [SerializeField] string groundCheckName = "GroundCheck";
        [SerializeField] string ceilingCheckName = "CeilingCheck";
        [SerializeField] Transform centerOfMass;
        [SerializeField] float dodgeFactor = 2f;
        [SerializeField] float dodgeCD = 1f;
        [SerializeField] float dodgeDuration = 0.3f;

        PlayerCharacter m_pc;
        Transform m_groundCheck;                      //地面檢查
        const float k_GroundRadius = 0.2f;
        bool m_isGrounded;
        Transform m_ceilingCheck;                     //天花板檢查
        const float k_CeilingRadius = 0.1f;
        Animator m_characterAnim;
        Rigidbody2D characterRigidBody;
        bool m_bFacingRight = true;
        bool m_bJump = false;
        bool m_bDodge = false;
        float m_speedFactor = 0f;
        float dodgeCount = 0f;
        bool dodging = false;

        private void Awake()
        {
            m_groundCheck = transform.Find(groundCheckName);
            m_ceilingCheck = transform.Find(ceilingCheckName);
            m_characterAnim = GetComponent<Animator>();
            characterRigidBody = GetComponent<Rigidbody2D>();
            characterRigidBody.centerOfMass = centerOfMass.position;
            m_pc = GetComponent<PlayerCharacter>();
        }

        private void Update()
        {
            m_speedFactor = Mathf.Lerp(0f, speed, 0.01f) * Time.fixedDeltaTime;
            if (Input.GetButtonDown("Dodge"))
            {
                m_bDodge = true;
            }
            else if (Input.GetButtonDown("Jump"))
            {
                m_pc.PlayerState.NextState((int)GSDefine.PlayerState.JUMP);
                m_bJump = true;
            }
            else if(Input.GetMouseButtonDown(0) && m_pc.CanAttack())
            {
                m_pc.PlayerState.NextState((int)GSDefine.PlayerState.ATTACK);
            }
        }

        private void FixedUpdate()
        {
            Set_isGrounded();
            CharacterAction();
        }

        void CharacterAction()
        {
            // 左右移動  
            Move(Input.GetAxisRaw("Horizontal") * m_speedFactor, false, m_bJump, m_bDodge);
            m_bJump = false;
            m_bDodge = false;
        }

        void Move(float moveSpeed, bool crouch, bool jump, bool dodge)
        {
            //Crouch,  接動畫(蹲下)
            //..

            //Dodge, 接動畫(迴避)
            if (dodgeCount < dodgeCD) dodgeCount += Time.fixedDeltaTime;    //冷卻判斷計時
            if (dodgeCount >= dodgeDuration) dodging = false;    //無敵(類似Buff)判斷
            if (m_isGrounded && dodge && dodgeCount >= dodgeCD)     //迴避判斷
            {
                dodgeCount = 0;
                dodging = true;
                m_characterAnim.SetTrigger("Dodge");
            }

            if (moveSpeed > 0 && !m_bFacingRight)
                Flip();
            else if (moveSpeed < 0 && m_bFacingRight)
                Flip();

            //TODO:是否能合成一行?  moveSpeed * maxSpeed * dodgeFactor * dodging
            if (dodging) characterRigidBody.velocity = new Vector2(moveSpeed * speed * dodgeFactor, characterRigidBody.velocity.y);
            else        characterRigidBody.velocity = new Vector2(moveSpeed * speed, characterRigidBody.velocity.y);

            moveSpeed = Mathf.Abs(moveSpeed);
            m_characterAnim.SetFloat("Speed", moveSpeed);

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
                {
                    m_isGrounded = true;
                }
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

