using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EyeBoss
{
    public class EyeBossPhaseOneRings : IEyeBossAiState
    {
        private AIBoss bossController;
        private EyeBossShoot bossWeapon;
        private Transform bossTransform;
        
        private bool movingRight = true;
        //private const float MOVE_DOWN_DISTANCE_THRESHOLD = 0f;

        private const float LOWEST_POSITION = 1f;

        private int phaseNumber = 0;
        private int framesSinceLastCheck = 0;
        private bool attacking = false;

        // 🏃‍♂️ PUBLIC METHODS ----------------------------------------------------------------
        public EyeBossPhaseOneRings(int phaseNumber)
        {
            this.phaseNumber = phaseNumber;
        }

        public void Initialise(AIBoss bossController, EyeBossShoot bossWeapon, Transform bossTransform)
        {
            this.bossController = bossController;
            this.bossWeapon = bossWeapon;
            this.bossTransform = bossTransform;
            
            this.bossWeapon.SetPhase(phaseNumber);
        }

        public void GoToNextState()
        {
            IEyeBossAiState nextState;

            nextState = new EyeBossPhaseTwoSlow(phaseNumber+1);

            nextState.Initialise(bossController, bossWeapon, bossTransform);

            bossController.AiState = nextState;
            Debug.Log("Phase number "+phaseNumber);
        }

        public void UpdateFrame()
        {

            // attacking mode
            if(attacking)
            {
                Shoot(bossTransform, bossWeapon, bossController);

                // attack for 2 seconds
                if(framesSinceLastCheck >= 360)
                {
                    attacking = false;
                    framesSinceLastCheck = 0;
                }
            }
            else
            {
                // wait random time until attack again
                if(framesSinceLastCheck >= 60)
                {
                    attacking = Random.Range(0,100) < 25;
                    framesSinceLastCheck = 0;
                }
            }

            framesSinceLastCheck++;
            
            BossMove(bossTransform, bossTransform.position, DistanceToXBound());

        }

        // 🤫 PRIVATE METHODS ----------------------------------------------------------------

        private void BossMove(Transform bossTransform, Vector3 currentPosition, float distanceToXBoundary)
        {
            //  move around side to side 
            Vector3 direction;
            if (movingRight)
                direction = Vector3.right;
            else
                direction = Vector3.left;

            //if it reaches right bound turn around
            if (bossTransform.position.x >= Movement.xBound && movingRight)
            {
                movingRight = false;
            }
            //when left bound is reached turn around
            else if (bossTransform.position.x <= -Movement.xBound && !movingRight)
            {
                movingRight = true;
            }

            bossController.Move(direction);
        }

        private void Shoot(Transform bossTransform, EyeBossShoot bossWeapon, AIBoss bossController)
        {
            Debug.Log("Shoot eye boss");
            Vector3 dir = (GameManager.INST.GetPlayerPos() - bossTransform.position).normalized;

            bossWeapon.ShootFirstPhase();
        }

        private float DistanceToXBound()
        {
            return Mathf.Abs(Mathf.Abs(bossTransform.position.x) - (Movement.xBound));
        }
        
        public int GetPhaseNumber()
        {
            return phaseNumber;
        }
    }
}
