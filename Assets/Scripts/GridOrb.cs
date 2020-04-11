using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridOrb : MonoBehaviour
{
    public bool Hovered { get; private set; }
    public Vector3Int Position => Vector3Int.FloorToInt(transform.position);

    public Color NormalColor, HoveredColor;
    public MeshRenderer Renderer;

    MomentumGridObject player;

    void Update ()
    {
        Renderer.material.color = Hovered ? HoveredColor : NormalColor;
    }

    public void SetPlayer (MomentumGridObject player)
    {
        this.player = player;
    }

	public void OnMouseDown ()
	{
        player.AccelerateToPosition(Position);
	}

	public void OnMouseEnter ()
	{
        Hovered = true;
	}

	public void OnMouseExit ()
	{
        Hovered = false;
	}
}
