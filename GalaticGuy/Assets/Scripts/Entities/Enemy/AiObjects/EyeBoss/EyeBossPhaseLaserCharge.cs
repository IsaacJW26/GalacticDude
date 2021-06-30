using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EyeBoss
{
    public class EyeBossPhaseLaserCharge : IEyeBossAiState
    {
        private AIBoss bossController;
        private EyeBossShoot bossWeapon;
        private Transform bossTransform;
        
        private bool movingRight = true;
        //private const float MOVE_DOWN_DISTANCE_THRESHOLD = 0f;

        private const float LOWEST_POSITION = 3f;
        
        private int phaseNumber = 0;
        private int framesSinceLastCheck = 0;
        private AttackState attackingState;
        enum AttackState
        {
            entering,
            moving,
            looking,
            charging,
            shooting,
            cooldown,
        }

        // 🏃‍ PUBLIC METHODS ----------------------------------------------------------------
        public EyeBossPhaseLaserCharge(int phaseNumber)
        {
            this.phaseNumber = phaseNumber;
            attackingState = AttackState.entering;
        }

        public void Initialise(AIBoss bossController, EyeBossShoot bossWeapon, Transform bossTransform)
        {
            this.bossController = bossController;
            this.bossWeapon = bossWeapon;
            this.bossTransform = bossTransform;
            
            this.bossWeapon.SetPhase(phaseNumber);
            bossController.EyeAnimationObject.LookDirection = DirectionToPlayer;
        }

        public void GoToNextState()
        {
            IEyeBossAiState nextState;

            nextState = new EyeBossPhaseRings(phaseNumber+1);

            nextState.Initialise(bossController, bossWeapon, bossTransform);

            bossController.AiState = nextState;
            Debug.Log("Phase number "+ phaseNumber);
        }

        public void UpdateFrame()
        {
            switch(attackingState)
            {
                case AttackState.entering:

                    if (bossTransform.position.y > LOWEST_POSITION)
                        bossController.Move(Vector3.down);
                    else
                        attackingState = AttackState.moving;

                    break;
                case AttackState.moving:
                    // wait random time until attack again
                    if (framesSinceLastCheck >= 60)
                    {
                        bool attacking = Random.Range(0, 100) < 25;
                        framesSinceLastCheck = 0;
                        if(attacking)
                        {
                            bossController.EyeAnimationObject.StartFrantic();
                            attackingState = AttackState.looking;
                        }
                    }

                    framesSinceLastCheck++;

                    BossMove(bossTransform, bossTransform.position, DistanceToXBound());
                    break;
                case AttackState.looking:
                    if (framesSinceLastCheck >= 180)
                    {
                        framesSinceLastCheck = 0;
                        attackingState = AttackState.charging;
                        
                        bossController.EyeAnimationObject.StartTracking();
                    }
                    framesSinceLastCheck++;

                    break;
                case AttackState.charging:
                    if (framesSinceLastCheck >= 120)
                    {
                        framesSinceLastCheck = 0;
                        attackingState = AttackState.shooting;
                    }
                    framesSinceLastCheck++;

                    float percent = framesSinceLastCheck / 120f;
                    bossController.EyeAnimationObject.ChargeUpdate(percent);
                    break;
                case AttackState.shooting:
                    bossController.EyeAnimationObject.ChargeUpdate(0f);

                    // attack after 2 seconds
                    if (framesSinceLastCheck >= 120)
                    {
                        Shoot(bossTransform, bossWeapon, bossController);

                        attackingState = AttackState.cooldown;

                        framesSinceLastCheck = 0;
                        bossController.EyeAnimationObject.StartWandering();
                    }
                    framesSinceLastCheck++;

                    break;
                case AttackState.cooldown:
                    framesSinceLastCheck++;
                    if (framesSinceLastCheck >= 180)
                    {
                        framesSinceLastCheck = 0;
                        attackingState = AttackState.moving;
                    }

                    break;
            }            
        }

        // 🤫 PRIVATE METHODS ----------------------------------------------------------------
        private Vector3 DirectionToPlayer()
        {
            return (GameManager.INST.GetPlayerPos() - bossTransform.position).normalized;
        }

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
            Vector3 dir = (GameManager.INST.GetPlayerPos() - bossTransform.position).normalized;

            bossWeapon.ShootFirstPhase(dir);
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
