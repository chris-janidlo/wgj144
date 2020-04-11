using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class MomentumGridObject : MonoBehaviour
{
    public event Action ReachablePositionsChanged;

    public Vector3Int Position
    {
        get => GridManager.Instance.GetPosition(transform).Value;
        private set => GridManager.Instance.SetPosition(transform, value);
    }

    public Vector3Int PastVelocity { get; private set; }
    public Vector3Int FutureVelocity { get; private set; }

    public Vector3Int FuturePosition => Position + FutureVelocity;

    [SerializeField]
    int _acceleration, _drag;

    public int Acceleration
    {
        get => _acceleration;
        set
        {
            if (_acceleration == value) return;

            _acceleration = value;
            invalidateCache();
        }
    }

    public int Drag
    {
        get => _drag;
        set
        {
            if (_drag == value) return;

            _drag = value;
            invalidateCache();
        }
    }

    HashSet<Vector3Int> reachablePosCache = null;

    public HashSet<Vector3Int> ReachablePositions () => reachablePosCache ?? (reachablePosCache = calculateReachablePositions());

    public bool PositionIsReachable (Vector3Int targetPosition) => ReachablePositions().Contains(targetPosition);

    public void AccelerateToPosition (Vector3Int targetPosition)
    {
        if (!PositionIsReachable(targetPosition))
        {
            throw new ArgumentException($"position {targetPosition} is outside of acceptable range");
        }

        FutureVelocity += targetPosition - (Position + FutureVelocity);

        reachablePosCache = null;
    }

    public void Move ()
    {
        Position += FutureVelocity;
        PastVelocity = FutureVelocity;

        invalidateCache();
    }

    void invalidateCache ()
    {
        reachablePosCache = null;
        ReachablePositionsChanged?.Invoke();
    }

    HashSet<Vector3Int> calculateReachablePositions ()
    {
        // next position if no acceleration is applied
        Vector3Int nextPos = Position + PastVelocity - (PastVelocity.Normalize() * PastVelocity.sqrMagnitude * Drag);

        HashSet<Vector3Int> accelSphere = new HashSet<Vector3Int>();

        for (int x = nextPos.x - Acceleration; x <= nextPos.x + Acceleration; x++)
        {
            for (int y = nextPos.y - Acceleration; y <= nextPos.y + Acceleration; y++)
            {
                for (int z = nextPos.z - Acceleration; z <= nextPos.z + Acceleration; z++)
                {
                    Vector3Int vox = new Vector3Int(x, y, z);

                    if (Vector3Int.Distance(nextPos, vox) <= Acceleration) accelSphere.Add(vox);
                }
            }
        }

        return accelSphere;
    }
}
