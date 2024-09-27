using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour, IPlayerComponent
{
    private Player _player;

	

	public void Initialize(Player player)
	{
		_player = player;
	}
}
