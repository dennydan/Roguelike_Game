using UnityEngine;

public class Monster : MonoBehaviour, IDemagable
{
    bool isFacingRight = false;
    [SerializeField]
    float m_health;
    float m_maxHealth = 5;

    private void Awake()
    {
        m_health = m_maxHealth;
    }

    void IDemagable.Demage(float demage)
    {
        if (m_health > 0)
        {
            m_health -= demage;
            Debug.Log("Health :　" + m_health);
        }
    }

    public void LookAt_Player(Transform player)
    {
        if (transform.position.x < player.position.x && !isFacingRight)
        {
            Filp();
            Debug.Log(isFacingRight);
        }
        else if (transform.position.x > player.position.x && isFacingRight)
        {
            Filp();
            Debug.Log(isFacingRight);
        }
    }

    void Filp()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
