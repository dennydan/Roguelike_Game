using UnityEngine;

interface IDemagable
{
    void Demage();
}



public class Weapon : MonoBehaviour
{
    Monster m_monster;
    void Start()
    {

    }
    
    void TakeDemage(IDemagable demagable)
    {
        demagable.Demage();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Monster m_monster = other.GetComponent<Monster>();
        if(m_monster)
            TakeDemage(m_monster);
    }
}
