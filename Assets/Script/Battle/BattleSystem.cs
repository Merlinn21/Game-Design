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

        for(int i = 0; i < party[random].ghostParty.Length; i++)
        {
            enemies[i].Setup(party[random].ghostParty[i]);
        }
    }

    private void SetEventParty()
    {
        for (int i = 0; i < eventParty.ghostParty.Length; i++)
        {
            enemies[i].Setup(eventParty.ghostParty[i]);
        }
    }
}
