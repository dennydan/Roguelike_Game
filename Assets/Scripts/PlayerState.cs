using UnityEngine;
using UnityEngine.UI;

namespace RoguelikeGame
{
    
    enum ParticleEffects
    {
        NORMAL = 0,
        COMBO_1,
        COMBO_2
    }
    [RequireComponent(typeof(PlayerCharacter))]
    public class PlayerState : MonoBehaviour
    {
        [SerializeField] float m_attackInterval = 0.5f;

        StateMachine m_playerState;
        PlayerCharacter m_pc;
        Animator m_characterAnim;
        Monster m_npc = null;
        Weapon m_weapon;
        StatusWidget m_statusWidget;
        ParticleSystem[] m_effects;
        SkillCaster[] m_skillTree;

        bool m_bStartAttack = false;
        float m_attackTime = 1.0f;
        float m_combatTime = 1.0f;
        int m_attackCount = 0;
        int m_hitEffectsAmount = 2;
        Vector3 m_hitOffset = new Vector3(0, 0.5f, 0);

        private void Awake()
        {
            m_pc = GetComponent<PlayerCharacter>();
            m_statusWidget = GetComponentInChildren<StatusWidget>();
            m_characterAnim = GetComponent<Animator>();
            m_weapon =  GetComponentInChildren<Weapon>();
            m_playerState = new StateMachine((int)GSDefine.PlayerState.IDLE);
            m_pc.PlayerState = m_playerState;

            SetHitEffect();
            SetSkillTree();
        }

        void Start()
        {
        }

        void Update()
        {
            int currentState = m_playerState.Tick();
            
            switch (currentState)
            {
                case (int)GSDefine.PlayerState.IDLE:
                    {
                        if (m_playerState.IsEntering())
                        {
                            Debug.Log("IDLE");
                        }

                        break;
                    }
                case (int)GSDefine.PlayerState.CASTSPELL:
                    {
                        // 技能詠唱
                        if (m_playerState.IsEntering())
                        {
                            Debug.Log("CASTSPELL");
                        }

                        break;
                    }
                case (int)GSDefine.PlayerState.ATTACK:
                    {
                        if (m_playerState.IsEntering())
                        {
                            m_characterAnim.SetTrigger("Attack");
                            AttackCombat();
                        }
                        if (HasColdDown())
                        {
                            m_playerState.NextState((int)GSDefine.PlayerState.IDLE);
                        }
                        break;
                    }
                case (int)GSDefine.PlayerState.DODGE:
                    {
                        if (m_playerState.IsEntering())
                        {
                            m_characterAnim.SetTrigger("Dodge");
                            Debug.Log("DODGE");
                        }
                        else
                        {
                            m_playerState.NextState((int)GSDefine.PlayerState.IDLE);
                        }
                        break;
                    }
                case (int)GSDefine.PlayerState.JUMP:
                    {
                        if (m_playerState.IsEntering())
                        {
                            Debug.Log("JUMP");
                        }
                        break;
                    }
                case (int)GSDefine.PlayerState.DIE:
                    {
                        if (m_playerState.IsEntering())
                        {
                            Debug.Log("DIE");
                            Destroy(gameObject);
                        }
                        break;
                    }
                default:
                    break;

            }

        }

        private void FixedUpdate()
        {
            UpdateStatus();
        }


        private void AttackCombat()
        {
            m_combatTime = m_attackInterval;

            switch (m_attackCount)
            {
                case 0:
                    {
                        Debug.Log("Attack_0");

                        m_bStartAttack = true;
                        Transform target = m_weapon.Attack((int)AttackType.PLAYER, 1.0f, 1.0f);
                        CreateHitEffect((int)GSDefine.AttackEffect.Hit_POOF, target);
                        m_attackCount++;
                        break;
                    }
                case 1:
                    {
                        Debug.Log("Attack_1");
                        Transform target = m_weapon.Attack((int)AttackType.PLAYER, 1.2f, 1.2f);
                        CreateHitEffect((int)GSDefine.AttackEffect.Hit_POOF, target);
                        m_attackCount++;
                        break;
                    }
                case 2:
                    {
                        Debug.Log("Attack_2");
                        Transform target = m_weapon.Attack((int)AttackType.PLAYER, 2.0f, 2.0f);
                        CreateHitEffect((int)GSDefine.AttackEffect.HIT_VIRUS, target);
                        m_attackCount = 0;
                        break;
                    }

                default:
                    break;
            }
        }

        // 設定連擊CD
        private bool HasColdDown()
        {
            m_attackTime -= Time.deltaTime;
            if (m_attackTime <= 0)
            {
                switch (m_attackCount)
                {
                    case 0:
                        {
                            m_attackTime = m_attackInterval/5;
                            break;
                        }
                    case 1:
                        {
                            m_attackTime = m_attackInterval;
                            break;
                        }
                    case 2:
                        {
                            m_attackTime = m_attackInterval * 2;
                            break;
                        }

                    default:
                        break;
                }
                m_combatTime = m_attackInterval;
                return true;
            }
            else
               return false;
        }

        // 重置連擊
        private void ResetCombo()
        {
            if (m_bStartAttack)
            {
                m_combatTime -= Time.deltaTime;
                if (m_combatTime <= 0 || m_playerState.Current() == (int)GSDefine.PlayerState.JUMP)
                {
                    m_bStartAttack = false;
                    m_attackCount = 0;
                }
            }
        }

        // 更新狀態
        private void UpdateStatus()
        {
            ResetCombo();
            m_statusWidget.UpdateStatus(m_pc.GetLevel(), m_pc.GetHealthPercentage(), m_pc.GetExpPercentage());
        }

        // 初始化攻擊粒子特效
        private void SetHitEffect()
        {
            m_effects = new ParticleSystem[m_hitEffectsAmount];
            
            for(int effectIndex = 0; effectIndex < m_hitEffectsAmount; effectIndex++)
            {
               m_effects[effectIndex] = Resources.Load<AssetLoader>("AssetLoader").GetHitParticle(effectIndex);
            }
        }
        
        // 產生攻擊粒子特效
        private bool CreateHitEffect(int effectIndex, Transform target)
        {
            if(target != null && effectIndex < m_effects.Length)
            {
                ParticleSystem effect = Instantiate(m_effects[effectIndex]);
                effect.transform.position = target.position + m_hitOffset;
                return true;
            }
            return false;
        }

        // 設定技能樹
        private void SetSkillTree()
        {
            m_skillTree = new SkillCaster[5];
            m_skillTree[0] = Resources.Load<AssetLoader>("AssetLoader").GetSkillCaster((int)GSDefine.SkillType.SPELL_1);
        }

        // 技能施放
        public void CastSpellImplement(int type)
        {
            SkillCaster spell = Instantiate(m_skillTree[type]);
            float castRange = transform.localScale.x * spell.SpellRange;
            Vector3 position = new Vector3(transform.position.x + spell.transform.position.x * castRange, transform.position.y, transform.position.z);
            spell.transform.position = position;
            spell.CastSpell(1.0f);
 
        }
    }
}

