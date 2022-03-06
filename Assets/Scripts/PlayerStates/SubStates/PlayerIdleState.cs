using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData,
        string animBoolName) : base(player, playerStateMachine, playerData, animBoolName)
    {
    }
}