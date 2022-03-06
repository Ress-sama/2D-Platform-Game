using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData,
        string animBoolName) : base(player, playerStateMachine, playerData, animBoolName)
    {
    }
}