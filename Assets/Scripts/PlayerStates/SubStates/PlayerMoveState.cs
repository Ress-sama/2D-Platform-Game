using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData,
        string animBoolName) : base(player, playerStateMachine, playerData, animBoolName)
    {
    }
}