using UnityEngine;

/*
 * 已知BUG:
 *  1.牆壁不算落地，只是不能跳躍，一樣會卡住
 *  2.低高度浮空可跳躍
 *  3.怪物傷害速度過快
 *  4.站在邊緣會被視為離地導致無法移動
 *  5.人物大小、地形與移動跳躍
 *  
 *  2020/09/28
 *  目前遊戲內尺寸測量: 
    1.跳躍高度約2.5格高
    2.跳躍長度約5.5格
    3.走路速度約5.5格/s
    TODO: 變數名稱規則
    TODO: 迴避無敵(等生命值)、納入技能系統
    TODO: Ground_1x1如何重設為標準?
    TODO: 統一生物Entity控制器
    TODO: 角色動畫

    +閃避納入狀態機
 *  
 * */

namespace RoguelikeGame
{
    [RequireComponent(typeof(PlayerCharacter))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float speed = 200f;
        [SerializeField] float jumpForce = 15f;
        [SerializeField] LayerMask Ground_Layer;
        [SerializeField] string groundCheckName = "GroundCheck";
        [SerializeField] string ceilingCheckName = "CeilingCheck";
        [SerializeField] Transform centerOfMass;
        [SerializeField] float dodgeFactor = 2f;
        [SerializeField] float dodgeCD = 1f;
        [SerializeField] float dodgeDuration = 0.3f;
        [SerializeField] float gravity = -1.25f;
        [SerializeField] float gravityFactor = 10f;
        [SerializeField] float MAX_JUMP_TIME = 0.1f;


        Transform m_plaerStateHUD;
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
        bool jumpingUp = false;
        float jumpCount = 0f;

        private void Awake()
        {
            m_groundCheck = transform.Find(groundCheckName);
            m_ceilingCheck = transform.Find(ceilingCheckName);
            m_characterAnim = GetComponent<Animator>();
            characterRigidBody = GetComponent<Rigidbody2D>();
            characterRigidBody.centerOfMass = centerOfMass.position;
            m_pc = GetComponent<PlayerCharacter>();
            m_plaerStateHUD = transform.Find("StatusWidget");
        }

        private void Update()
        {
            m_speedFactor = Mathf.Lerp(0f, speed, 0.01f) * Time.fixedDeltaTime;
            if (Input.GetButtonDown("Dodge"))
            {
                m_pc.PlayerState.NextState((int)GSDefine.PlayerState.DODGE);
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

            if (Input.GetButtonUp("Jump")) jumpingUp = false;
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
            }

            //人物轉向
            if (moveSpeed > 0 && !m_bFacingRight)
                Flip();
            else if (moveSpeed < 0 && m_bFacingRight)
                Flip();

            //TODO:是否能合成一行?  moveSpeed * maxSpeed * dodgeFactor * dodging
            //TODO: 牆壁不算落地，只是不能跳躍，一樣會卡住
            //TODO: 迴避時跳躍再落地可否迴避?
            if (dodging) characterRigidBody.velocity = new Vector2(moveSpeed * speed * dodgeFactor, characterRigidBody.velocity.y);
            else         characterRigidBody.velocity = new Vector2(moveSpeed * speed, characterRigidBody.velocity.y);

            moveSpeed = Mathf.Abs(moveSpeed);
            m_characterAnim.SetFloat("Speed", moveSpeed);

            //Jump, 接動畫(跳躍)
            //TODO:
            //已知BUG: 下落太快會卡進地板 (限制下落速度/寫出移動碰撞判定)
            //已知BUG: 人物大小、地形與移動跳躍
            //1.擬真跳躍(7 days、麥塊): 按下瞬間向上力, 未著陸時持續給予向下力
            //  優點: 較為現實
            //  缺點: 過於現實、由於無法調整高度，操作感、遊玩樂趣下降
            //2.蟲蟲跳躍(目前、哈囉奈): 按下時持續給力 隨時間給越少 放開給予向下力
            //  優點: 適合操作精膩的遊戲、此類遊戲常用
            //  缺點: 畫面產生不現實、不合理
            if (jumpCount < MAX_JUMP_TIME && jumpingUp) jumpCount += Time.fixedDeltaTime;    //冷卻判斷計時
            else
            {
                jumpCount = 0;
                jumpingUp = false;
            }
            if (m_isGrounded && jump)
            {
                gravityFactor = 0;
                m_isGrounded = false;
                jumpingUp = true;
                m_characterAnim.SetBool("Ground", m_isGrounded);
            }

            if (!m_isGrounded && jumpingUp) characterRigidBody.velocity = new Vector2(characterRigidBody.velocity.x, jumpForce);
            else if (!m_isGrounded && !jumpingUp)
            {
                if (characterRigidBody.velocity.y > 8f) characterRigidBody.velocity = new Vector2(characterRigidBody.velocity.x, 8f);
                characterRigidBody.AddForce(new Vector2(0, gravity * gravityFactor++));
            }
        }

        //已知BUG: 低高度浮空可跳躍、站在邊緣會被視為離地導致無法移動
        void Set_isGrounded()
        {
            m_isGrounded = false;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_groundCheck.position, k_GroundRadius, Ground_Layer);
            for (int i = 0; i < colliders.Length; i++)
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
            // 血量狀態不反轉
            Vector3 scaleHUD = m_plaerStateHUD.transform.localScale;
            scaleHUD.x *= -1;
            m_plaerStateHUD.localScale = scaleHUD;
        }
    }
}

