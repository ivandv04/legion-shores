using GameplayObjects;
using PlayerObjects;
using PoliticalEntities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSession
{
    public readonly World World;

    public readonly UnitWorld UnitMap;

    private readonly List<PlayerInstance> _players = new();

    public GameSession(World world, Realm humanControlled)
    {
        World = world;
        UnitMap = new(world.GetTerr().GetLength(0));
        foreach (Realm r in world.GetRealms())
            _players.Add(new(r == humanControlled, r));
    }

}