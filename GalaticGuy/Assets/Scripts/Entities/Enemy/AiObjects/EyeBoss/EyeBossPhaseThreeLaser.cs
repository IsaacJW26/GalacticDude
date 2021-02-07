using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EyeBoss
{
    public class EyeBossPhaseThreeLaser : IEyeBossAiState
    {
        private AIBoss bossController;
        private EyeBossShoot bossWeapon;
        private Transform bossTransform;
        
        private bool movingRight = true;
        //private const float MOVE_DOWN_DISTANCE_THRESHOLD = 0f;

        private const float LOWEST_POSITION = 1f;

        private int phaseNumber = 0;
        private bool attacking = false;

        // 🏃‍♂️ PUBLIC METHODS ----------------------------------------------------------------
        public EyeBossPhaseThreeLaser(int phaseNumber)
        {
            this.phaseNumber = phaseNumber;
            attacking = false;
        }

        public void Initialise(AIBoss bossController, EyeBossShoot bossWeapon, Transform bossTransform)
        {
            this.bossController = bossController;
            this.bossWeapon = bossWeapon;
            this.bossTransform = bossTransform;
            
            this.bossWeapon.SetPhase(phaseNumber);
        }
        Vector3 moveDirection = Vector3.zero;
        int framesSinceLastCheck = 0;

        public void GoToNextState()
        {
            IEyeBossAiState nextState;

            nextState = new EyeBossPhaseThreeLaser(phaseNumber+1);

            nextState.Initialise(bossController, bossWeapon, bossTransform);

            bossController.AiState = nextState;
            Debug.Log("Phase number "+phaseNumber);
        }

        public void UpdateFrame()
        {
            // attacking mode
            if(attacking)
            {
                if(framesSinceLastCheck % 12 == 0)
                    moveDirection = GetDirectionToPlayer();   

                Shoot(bossTransform, bossWeapon, bossController);
                // attack for 3 seconds
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
            
            bossController.Move(moveDirection);
        }


        // 🤫 PRIVATE METHODS ----------------------------------------------------------------
        private Vector3 GetDirectionToPlayer()
        {
            //  move around side to side 
            Vector3 direction;
            Vector3 playerPos = GameManager.INST.GetPlayerPos();

            // follows player around
            if (bossTransform.position.x < playerPos.x)
            {
                movingRight = false;
                direction = Vector3.right;

            }
            else if (bossTransform.position.x > playerPos.x)
            {
                movingRight = true;
                direction = Vector3.left;
            }
            else
            {
                direction = Vector3.zero;
            }

            return direction;
        }

        private void Shoot(Transform bossTransform, EyeBossShoot bossWeapon, AIBoss bossController)
        {
            Debug.Log("Shoot eye boss");
            Vector3 dir = (GameManager.INST.GetPlayerPos() - bossTransform.position).normalized;

            bossWeapon.ShootThirdPhase(Vector3.down);
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
