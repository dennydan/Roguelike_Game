using System;
using UnityEngine;

/**<summary> 
 載入資源，統一存放，減少網路請求和資源丟失   
 </summary> */
// 2020/08/22 create by dennyliu
public class AssetLoader : MonoBehaviour
{
    [SerializeField] ParticleSystem[] hitParticles;

    public ParticleSystem GetHitParticle(int index)
    {
        if (index >= hitParticles.Length)
            return null;
        else
        {
            return hitParticles[index];
        }
    }
}
