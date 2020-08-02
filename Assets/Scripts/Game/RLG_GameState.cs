﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLG_GameState : MonoBehaviour
{
    private int m_maxMonsterAmount = 2;
    private int m_monsterAmount = 0;
    private Action m_spawnMonster;
    private Action m_stopSpawnMonster;
    // Start is called before the first frame update
    void Start()
    {
        // 執行生怪直到數量足夠
    }

    // Update is called once per frame
    void Update()
    {
        if (m_monsterAmount >= m_maxMonsterAmount)
        {
            Debug.Log("Too much !");
            m_stopSpawnMonster.Invoke();
        }
        else
        {
            m_monsterAmount++;
            m_spawnMonster.Invoke();
        }
    }

    public void MonsterSpawned(Action spawn, Action stopSpawn)
    {
        // 生怪調用方法
        m_monsterAmount++;
        spawn.Invoke();
        m_spawnMonster = spawn;
        m_stopSpawnMonster = stopSpawn;
    }

    // 名子要修改
    public void MonsterDead(Action dead)
    {
        Debug.Log("Monster Dead !!!");
        m_monsterAmount--;
        dead.Invoke();
    }
}