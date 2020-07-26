using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] m_monsterType;
    // Start is called before the first frame update
    bool m_isSpawning = false;
    float m_spawnTime = 10.0f;

    void Start()
    {
        StartCoroutine(SpawnMonster());
    }
    IEnumerator SpawnMonster()
    {
        yield return new WaitForSeconds(m_spawnTime);
        Instantiate(m_monsterType[0], transform);
        m_isSpawning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!m_isSpawning)
        {
            m_isSpawning = true;
            StartCoroutine(SpawnMonster());
        }
    }
}
