using RoguelikeGame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] m_monsterType;
    [SerializeField] int m_maxCount = 5;
    [SerializeField] float m_spawnTime = 2.0f;
    // Start is called before the first frame update
    RLG_GameState m_gs;
    Action m_spawnedCB;
    bool m_isSpawning = false;
    bool m_canSpawn = false;
    
    int m_spawnType = 0;
    int m_spawnCount = 0;
    int m_spawnLevel = 0;

    // 生怪種類判斷 等級判斷(目前訂生5次 生1等)
    void Awake()
    {
        m_gs = GameObject.Find("GameManager").GetComponent<RLG_GameState>();
    }
    void Start()
    {
        m_gs.InitSpawners(this);
    }

    public void StartSpawn(Action spawnCB)
    {
        
        m_canSpawn = true;
        m_spawnedCB = spawnCB;
        m_spawnType = UnityEngine.Random.Range(0, m_monsterType.Length - 1);
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

    void CalculateSpawnCount()
    {
        m_spawnCount++;
        if(m_spawnCount > m_maxCount)
        {
            m_spawnLevel++;
            m_spawnCount = 0;
        }
    }

    IEnumerator SpawnMonster(int type)
    {
        if(CanSpawn())
        {   
            m_isSpawning = true;
            yield return new WaitForSeconds(m_spawnTime);
            CalculateSpawnCount();
            m_isSpawning = false;
            if (type < m_monsterType.Length)
            {
                Monster monster = Instantiate(m_monsterType[type], transform).GetComponent<Monster>();
                monster.SetGameState(m_gs);
                monster.UpgradeLevel(m_spawnLevel);
                m_spawnedCB();
                m_gs.AddMonsterAmount(1);
            }
        }
    }
}
