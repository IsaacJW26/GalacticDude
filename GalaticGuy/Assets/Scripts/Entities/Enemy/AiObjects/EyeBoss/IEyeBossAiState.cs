using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EyeBoss
{
    public interface AiState
    {
        void Initialise(AIBoss bossController, EyeBossShoot bossWeapon, Transform bossTransform);
        void GoToNextState();
        void UpdateFrame();
        int GetPhaseNumber();
    }
}