using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class GridManager : Singleton<GridManager>
{
    public List<Transform> InitialMembers;

    BiDictionary<Vector3Int, Transform> grid = new BiDictionary<Vector3Int, Transform>();

    void Awake ()
    {
        SingletonOverwriteInstance(this);

        foreach (var member in InitialMembers)
        {
            SetPosition(member, Vector3Int.FloorToInt(member.position));
        }
    }

    public Transform GetMember (Vector3Int position)
    {
        Transform member;
        if (!grid.TryGetValue(position, out member)) return null;
        else return member;
    }

    public Vector3Int? GetPosition (Transform member)
    {
        Vector3Int position;
        if (!grid.Reverse.TryGetValue(member, out position)) return null;
        else return position;
    }

    public void SetPosition (Transform member, Vector3Int position)
    {
        if (GetMember(position) != null)
        {
            throw new ArgumentException($"position {position} already occupied");
        }

        grid.Reverse.Remove(member); // safe to call without check
        grid[position] = member;

        member.position = position;
    }
}
