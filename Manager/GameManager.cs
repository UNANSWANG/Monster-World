using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingleTon<GameManager>
{
    public CharacterStats playerStats;
    public Transform rootPoint;
    List<IEndGameObserve> endGameObserves = new List<IEndGameObserve>();
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        rootPoint = Resources.Load<GameObject>("出生点").transform;
    }
    public void RigisterPlayer(CharacterStats player)
    {
        playerStats = player;
    }
    public void AddObserve(IEndGameObserve observe)
    {
        endGameObserves.Add(observe);
    }
    public void RemoveObserve(IEndGameObserve observe)
    {
        endGameObserves.Remove(observe);
    }
    public void NotifyObserve()
    {
        foreach (var obseve in endGameObserves)
        {
            obseve.EndNotify();
        }
    }
    public Transform GetEntrence()
    {
        return rootPoint;
    }
}
