using RoguelikeGame;
using UnityEngine;

/**
 * 原地施放 (done)
 * 按鍵
 * 範圍     (done)
 * 方向
 * CD
 * 詠唱時間
 * 詠唱特效
 * 擊中特效
 * 鎖定
 * 
 * 2020.08.25 creat by dennyliu.
 */
public class SkillCaster : MonoBehaviour
{
    [SerializeField] float m_demage = 5;
    [SerializeField] float m_rang = 1.0f;
    [SerializeField] float m_coldTime = 5.0f;
    [SerializeField] int m_effectIndex = 1;     // GSDefine.AttackEffect
    float m_maintainTime = 3.0f;
    float m_spellSpeed = 1.0f;
    public float SpellSpeed { get { return m_spellSpeed; } }
    public float SpellRange { get { return m_rang; } }
    public float ColdDown { get { return m_coldTime; } }
    ParticleSystem m_skillEffect;
    void Awake()
    {
        m_skillEffect = Resources.Load<AssetLoader>("AssetLoader").GetHitParticle(m_effectIndex);
    }

    private void Start()
    {
        Destroy(gameObject, 2.0f);
    }

    // 施放技能
    public void CastSpell(float demageFactor)
    {
        Instantiate(m_skillEffect, transform);
        
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, m_rang);
        foreach (Collider2D hit in hits)
        {
            Monster npc = hit.GetComponent<Monster>();
            if (npc)
            {
                TakeDemage(npc, demageFactor);
            }
        }
    }

    private void TakeDemage(IDemagable demagable, float demageFactor)
    {
        demagable.Demage(m_demage * demageFactor);
    }

    void Destory()
    {

    }
}
