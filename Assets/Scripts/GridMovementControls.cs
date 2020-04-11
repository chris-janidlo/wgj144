using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridMovementControls : MonoBehaviour
{
    public MomentumGridObject Player;
    public LineRenderer FutureVelocityIndicator;
    public Button MoveButton;
    public Transform OrbContainer;
    public GridOrb OrbPrefab;

    void Start ()
    {
        MoveButton.onClick.AddListener(move);
        Player.ReachablePositionsChanged += respawnOrbs;

        respawnOrbs();
    }

    void Update ()
    {
        FutureVelocityIndicator.SetPosition(2, Player.FuturePosition);
    }

    void move ()
    {
        Player.Move();

        FutureVelocityIndicator.SetPosition(0, FutureVelocityIndicator.GetPosition(1));
        FutureVelocityIndicator.SetPosition(1, FutureVelocityIndicator.GetPosition(2));
    }

    void respawnOrbs ()
    {
        foreach (Transform child in OrbContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Vector3Int position in Player.ReachablePositions())
        {
            Instantiate(OrbPrefab, position, Quaternion.identity, OrbContainer).SetPlayer(Player);
        }
    }
}
