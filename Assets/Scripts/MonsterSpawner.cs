using RoguelikeGame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] m_monsterType;
    [SerializeField] RLG_GameState m_gs;
    // Start is called before the first frame update
    bool m_isSpawning = false;
    bool m_canSpawn = false;
    float m_spawnTime = 5.0f;
    int m_spawnType = 0;
    
    // 生怪種類判斷 等級判斷(目前訂生5次 生1等)
    void Start()
    {
        m_gs.MonsterSpawned(StartSpawn, StopSpawn);
    }

    public void StartSpawn()
    {
        m_canSpawn = true;
        StartCoroutine(SpawnMonster(m_spawnType));
    }

    public void StopSpawn()
    {
        m_canSpawn = false;
        StopCoroutine(SpawnMonster(m_spawnType));
    }

    bool CanSpawn()
    {
        return !m_isSpawning && m_canSpawn;
    }

    IEnumerator SpawnMonster(int type)
    {
        if(CanSpawn())
        {
            m_isSpawning = true;
            yield return new WaitForSeconds(m_spawnTime);
            m_isSpawning = false;
            if (type < m_monsterType.Length)
            {
                Instantiate(m_monsterType[type], transform).GetComponent<Monster>().SetGameState(m_gs);
                m_gs.AddMonsterAmount(1);
            }
        }
    }
}
