using RoguelikeGame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class RLG_GameState : MonoBehaviour
{
    [SerializeField] PlayerCharacter m_pc;
    [SerializeField] int m_maxMonsterAmount = 2;
    private bool m_spawning = false;
    private int m_monsterAmount = 0;
    private int m_spawnerIndex;
    private List<MonsterSpawner> m_monsterSpawners = new List<MonsterSpawner>();
    // Start is called before the first frame update
    void Start()
    {
        // 執行生怪直到數量足夠
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMonsterAmount();
    }

    private void UpdateMonsterAmount()
    {
        if (m_spawning) return;

        if (m_monsterAmount >= m_maxMonsterAmount)
        {
            m_monsterSpawners[m_spawnerIndex].StopSpawn();
            m_spawning = false;
        }
        else
        {
            m_spawning = true;
            m_spawnerIndex = UnityEngine.Random.Range(0, m_monsterSpawners.Capacity - 1);
            m_monsterSpawners[m_spawnerIndex].StartSpawn(()=> { m_spawning = false; });
        }
    }

    public void AddMonsterAmount(int amount)
    {
        m_monsterAmount = m_monsterAmount + amount;
    }

    public void InitSpawners(MonsterSpawner spawner)
    {
        // monster spawner 自己加進來
        m_monsterSpawners.Add(spawner);
    }

    // 名子要修改
    public void MonsterDead(Action dead)
    {
        m_monsterAmount--;
        dead.Invoke();
    }

    public void UpdateCharacterExp(float exp)
    {
        m_pc.AddCharacterExp(exp);
    }
}
