using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEyeBossAiState
{
    Vector3 Initialise(Enemy bossController, Shooter bossWeapon, Transform bossTransform, IEyeBossAiState nextState);
    void GoToNextState();
}