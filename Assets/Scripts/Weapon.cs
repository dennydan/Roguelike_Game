using UnityEngine;

public interface IDemagable
{
    void Demage(float demage);
}



public class Weapon : MonoBehaviour
{
    [SerializeField] float m_demage = 2;
    Monster m_monster;
    void Start()
    {

    }
    
    public void TakeDemage(IDemagable demagable)
    {
        demagable.Demage(m_demage);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Monster m_monster = other.GetComponent<Monster>();
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        if (m_monster)
            TakeDemage(m_monster);
    }
}
