using UnityEngine;

/**
 * 原地施放特效
 * 按鍵
 * 範圍
 * 方向
 * CD
 * 詠唱時間
 * 
 * 2020.08.25 creat by dennyliu.
 */
public class SkillCaster : MonoBehaviour
{

    ParticleSystem m_skillEffect;
    void Awake()
    {
        m_skillEffect = Resources.Load<AssetLoader>("AssetLoader").GetHitParticle((int)GSDefine.AttackEffect.HIT_VIRUS);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Cast()
    {

    }
}
