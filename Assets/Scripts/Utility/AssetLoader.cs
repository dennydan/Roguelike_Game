using System;
using UnityEngine;

/**<summary> 
 載入資源，統一存放，減少網路請求和資源丟失   
 </summary> */
// 2020/08/22 create by dennyliu
public class AssetLoader : MonoBehaviour
{
    [SerializeField] ParticleSystem[] m_hitParticles;
    [SerializeField] SkillCaster[] m_skillCasters;

    public ParticleSystem GetHitParticle(int index)
    {
        if (index >= m_hitParticles.Length)
            return null;
        else
        {
            return m_hitParticles[index];
        }
    }

    public SkillCaster GetSkillCaster(int type)
    {
        return m_skillCasters[type];
    }
}
