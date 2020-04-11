using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour, IDemagable
{
    [SerializeField] float m_health, m_maxHealth = 5;

    private void Awake()
    {
        m_health = m_maxHealth;
    }

    private void Update()
    {
        Debug.Log(m_health);
    }

    void IDemagable.Demage(float demage)
    {
        if (m_health > 0)
        {
            m_health -= demage;
            Debug.Log("Health :　" + m_health);
        }
    }
}
