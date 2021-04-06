using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EyeBoss
{
    public class AIBoss : EnemyAI
    {
        // editor visible fields ----------------

        [SerializeField]
        private GameObject[] rings = new GameObject[3];

        // hidden fields --------------------------------
        readonly float moveDownDistance = 0.6f;

        private IEyeBossAiState state;

        private EyeAnimation eyeAnimationCache = null;
        public EyeAnimation EyeAnimationObject
        {
            get
            {
                if(eyeAnimationCache == null)
                    eyeAnimationCache = GetComponentInChildren<EyeAnimation>();

                return eyeAnimationCache;
            }
        }
        // 🏃‍♂️ PUBLIC METHODS ----------------------------------------------------------------

        public override void Initialise(Movement movement, MainCharacter player, IShooter shoot)
        {
            base.Initialise(movement, player, shoot);
            Debug.Log("shooter "+shoot);
            state = new EyeBossPhaseLaserCharge(1);
            state.Initialise(this, (shoot as EyeBossShoot), transform);
            SetPhase(1);
        }

        public IEyeBossAiState AiState
        {
            get { return state; }
            set { state = value; }
        }

        //tries to shoot and move every frame
        public override void UpdateFrame(Vector3 currentPosition)
        {
            state.UpdateFrame();
        }

        // remove a ring on hit
        public override void OnDamage(CharacterHealth health)
        {
            // 4 stages
            // first is 3 rings to 2 rings
            // 2 rings to 1
            // 1 ring to none
            // no rings to dead
            
            int totalHpStages = rings.Length + 1;
            float hpStageInterval = (health.GetMaxHealth() / totalHpStages);
            int hpIntervalCount = Mathf.CeilToInt(
                health.GetHealth() / hpStageInterval);
            int newPhase = totalHpStages - hpIntervalCount;

            SetPhase(newPhase+1);
        }

        private void SetPhase(int newPhase)
        {
            int phaseNumber = state.GetPhaseNumber();

            // disable the rings from outer to inner
            while(phaseNumber < newPhase && phaseNumber < 4)
            {
                state.GoToNextState();
                rings[rings.Length - phaseNumber].SetActive(false);

                Debug.Log($"emergencyBreak phase {phaseNumber} newPhase {newPhase}");
                phaseNumber = state.GetPhaseNumber();
            }
        }

        public override void OnDeath()
        {
            
        }
    }
}