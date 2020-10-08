using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private GhostParty[] party;
    [SerializeField] private GhostParty eventParty;
    [SerializeField] private BattleUnit[] enemies;
    [SerializeField] private BattleHud[] battleHuds;

    public event Action<bool> onBattleOver;

    private void SetupBattle()
    {
        for(int i = 0; i < enemies.Length; i++)
        {
            battleHuds[i].SetData(enemies[i].Ghost);
        }
    }

    public void StartBattle(bool _randomBattle)
    {
        RandomizeGhost();
        SetupBattle();
    }

    public void StartEventBattle(GhostParty _ghostParty)
    {
        eventParty = _ghostParty;
        SetEventParty();
        SetupBattle();
    }

    public void RunAway()
    {
        onBattleOver(true);
    }

    private void RandomizeGhost()
    {
        int random = Mathf.FloorToInt(UnityEngine.Random.Range(0, party.Length));
        Debug.Log(party[random].name);
        for(int i = 0; i < party[random].ghostParty.Length; i++)
        {
            int ghostLvl = UnityEngine.Random.Range(party[random].minLvl, party[random].maxLvl + 1);
            enemies[i].Setup(party[random].ghostParty[i], ghostLvl);
        }
    }

    private void SetEventParty()
    {
        for (int i = 0; i < eventParty.ghostParty.Length; i++)
        {
            int ghostLvl = UnityEngine.Random.Range(party[i].minLvl, party[i].maxLvl);
            enemies[i].Setup(eventParty.ghostParty[i], ghostLvl);
        }
    }
}
