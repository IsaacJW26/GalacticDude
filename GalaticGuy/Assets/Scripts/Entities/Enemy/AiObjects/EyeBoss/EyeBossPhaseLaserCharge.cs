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
        // phase number usd for debugging
        private int phaseNumber = 0;
        private int framesSinceLastCheck = 0;

        private Vector3? savedPlayerPosition = null;

        private List<BossAnimation> animationParts;
        private IEnumerator<BossAnimation> animEnumerator;

        // 🏃‍ PUBLIC METHODS ----------------------------------------------------------------
        public EyeBossPhaseLaserCharge(int phaseNumber)
        {
            this.phaseNumber = phaseNumber;
        }

        public void Initialise(AIBoss bossController, EyeBossShoot bossWeapon, Transform bossTransform)
        {
            this.bossController = bossController;
            this.bossWeapon = bossWeapon;
            this.bossTransform = bossTransform;
            
            this.bossWeapon.SetPhase(phaseNumber);
            bossController.EyeAnimationObject.LookDirection = DirectionToPlayer;
            InitialiseAnimation();
        }

        public void GoToNextState()
        {
            IEyeBossAiState nextState;

            nextState = new EyeBossPhaseRings(phaseNumber+1);
            bossController.EyeAnimationObject.StartWandering();
            bossController.EyeAnimationObject.ChargeUpdate(0f);

            nextState.Initialise(bossController, bossWeapon, bossTransform);

            bossController.AiState = nextState;
            Debug.Log("Phase number "+ phaseNumber);
        }

        private void InitialiseAnimation()
        {
            animationParts = new List<BossAnimation>();

            BossAnimation enterstate = new BossAnimation().AddAnimationPart(
                    animationDuration: 600,
                    animationFunction: _ => { 
                        Debug.Log("enterstate");
                        if (bossTransform.position.y > LOWEST_POSITION)
                        {
                            // Debug.Log("entering");
                            bossController.Move(Vector3.down);
                        }   
                        else
                        {
                            animEnumerator.MoveNext();
                            // Debug.Log("move to move state");
                        }
                }).AddEnd();
            animationParts.Add(enterstate);

            BossAnimation movestate = new BossAnimation().AddAnimationPart(
                //animationDuration: 60 * Random.Range(1, 4),
                animationDuration: 20,
                animationFunction: _ =>
                {
                    BossMove(bossTransform, bossTransform.position, DistanceToXBound());
                    Debug.Log("moving");
                })
                .AddAnimationFrame( _ => {
                    bossController.EyeAnimationObject.StartFrantic();
                    Debug.Log("move to look state");

                });
            animationParts.Add(movestate);

            BossAnimation lookingState = new BossAnimation().
                AddDelay(180).
                AddAnimationFrame( _ => {
                    UnlockPlayerPosition();
                    bossController.EyeAnimationObject.StartTracking();
                    Debug.Log("move to charge state");
                });
            animationParts.Add(lookingState);

            // BossAnimation chargeSubstates = new BossAnimation().
            //     AddAnimationFrame( _ => UnlockPlayerPosition()).
            //     AddDelay(89).
            //     AddAnimationFrame( _ => LockPlayerPosition()).
            //     AddEnd();

            const int chargeDuration = 180;
            BossAnimation chargeState = new BossAnimation().
                AddAnimationPart(
                    chargeDuration,
                    (int f) => {
                        if(f == 0) {
                            UnlockPlayerPosition();
                        }
                        else if(f == 89) {
                            LockPlayerPosition();
                        }
                        // chargeSubstates.FrameUpdate();
                        float percent = (float) f / (float) chargeDuration;
                        bossController.EyeAnimationObject.ChargeUpdate(percent);
                }).AddAnimationFrame( _ => {
                    // Debug.Log("move to attack state");
                });
            animationParts.Add(chargeState);

            BossAnimation attackState = new BossAnimation().
                AddDelay(7).
                AddAnimationFrame(_ => {
                    bossController.EyeAnimationObject.ChargeUpdate(0f);
                    Shoot(bossTransform, bossWeapon, bossController);
                }).AddDelay(60).
                AddAnimationFrame(_ => {
                    bossController.EyeAnimationObject.StartWandering();
                    // Debug.Log("move to cool down");
                });
            animationParts.Add(attackState);

            BossAnimation cooldownState = new BossAnimation().
                AddDelay(10).
                AddAnimationFrame(_ => { 
                    // Debug.Log("reset from cool down");
                }).AddEnd(_ => {
                    animEnumerator.Reset();
                    animEnumerator.MoveNext();
                });
            animationParts.Add(cooldownState);

            for (int i = 1; i < animationParts.Count - 1; i++)
            {
                animationParts[i].AddEnd(_ => {
                    animEnumerator.MoveNext();
                });
            }

            animEnumerator = animationParts.GetEnumerator();
            animEnumerator.MoveNext();
        }

        public void UpdateFrame()
        {
            animEnumerator.Current.FrameUpdate();    
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
