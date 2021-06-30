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

        private Vector3? savedPlayerPosition = null;
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
                    framesSinceLastCheck++;

                    // wait random time until attack again
                    if (framesSinceLastCheck > 60)
                    {
                        bool attacking = Random.Range(0, 100) < 25;
                        framesSinceLastCheck = 0;
                        if(attacking)
                        {
                            bossController.EyeAnimationObject.StartFrantic();
                            attackingState = AttackState.looking;
                        }
                    }

                    BossMove(bossTransform, bossTransform.position, DistanceToXBound());
                    break;
                case AttackState.looking:
                    framesSinceLastCheck++;

                    if (framesSinceLastCheck > 180)
                    {
                        framesSinceLastCheck = 0;
                        attackingState = AttackState.charging;
                        
                        bossController.EyeAnimationObject.StartTracking();
                    }

                    break;
                case AttackState.charging:
                    const int chargeDuration = 180;
                    framesSinceLastCheck++;

                    // Stop following player after 90 frames
                    if(framesSinceLastCheck == 1)
                        UnlockPlayerPosition();
                    if(framesSinceLastCheck == 90)
                        LockPlayerPosition();


                    if (framesSinceLastCheck > chargeDuration)
                    {
                        framesSinceLastCheck = 0;
                        attackingState = AttackState.shooting;
                    }

                    float percent = (float)framesSinceLastCheck / (float)chargeDuration;
                    bossController.EyeAnimationObject.ChargeUpdate(percent);
                    break;
                case AttackState.shooting:
                    framesSinceLastCheck++;

                    // attack for 1 frame
                    if (framesSinceLastCheck == 7)
                    {
                        bossController.EyeAnimationObject.ChargeUpdate(0f);

                        Shoot(bossTransform, bossWeapon, bossController);
                    }
                    // delay a little after attack
                    else if(framesSinceLastCheck > 60)
                    {
                        framesSinceLastCheck = 0;
                        bossController.EyeAnimationObject.StartWandering();
                        attackingState = AttackState.cooldown;
                    }

                    break;
                case AttackState.cooldown:
                    framesSinceLastCheck++;
                    if (framesSinceLastCheck > 180)
                    {
                        framesSinceLastCheck = 0;
                        attackingState = AttackState.moving;
                    }

                    break;
            }            
        }

        // 🤫 PRIVATE METHODS ----------------------------------------------------------------
        // when player position is saved return that, otherwise get current
        private Vector3 DirectionToPlayer()
        {
            return savedPlayerPosition ?? (GameManager.INST.GetPlayerPos() - bossTransform.position).normalized;
        }

        private void LockPlayerPosition()
        {
            savedPlayerPosition = DirectionToPlayer();
        }

        private void UnlockPlayerPosition()
        {
            savedPlayerPosition = null;
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
            bossWeapon.ShootFirstPhase(DirectionToPlayer());
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
