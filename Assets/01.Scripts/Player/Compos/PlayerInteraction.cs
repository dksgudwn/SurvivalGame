using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour, IPlayerComponent
{
    private GameObject[] InteractionObjects;
    private Player _player;

    public void Initialize(Player player)
    {
        _player = player;
    }

    public void AfterInitialize()
    {
    }

    private void Update()
    {
        
    }

    private void OnCollisionEnter(Collision Object)
    {
    }
}
