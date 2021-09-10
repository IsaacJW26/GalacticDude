using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EyeBoss
{
    public class EyeBossShoot : MonoBehaviour, IShooter
    {
        [SerializeField]
        private CharacterShoot firstPhaseShooter;
        [SerializeField]
        private CharacterShoot secondPhaseShooter;
        [SerializeField]
        private CharacterShoot thirdPhaseShooter;
        private CharacterShoot[] thirdPhaseShooters;
        private const int SHOOTER_COUNT = 7;
        private const float SHOOT_SPREAD = 1f / (float)(SHOOTER_COUNT+1);
        private int phase = 0;

        public void SetPhase(int phase)
        {
            if(this.phase == 2 && phase == 3)
            {
                thirdPhaseShooters = new CharacterShoot[SHOOTER_COUNT];
                thirdPhaseShooters[0] = thirdPhaseShooter;
                for(int i = 1; i < SHOOTER_COUNT; i++)
                {
                    thirdPhaseShooters[i] = Instantiate(thirdPhaseShooter);
                    thirdPhaseShooters[i].transform.SetParent(thirdPhaseShooter.transform.parent);
                }
            }

            this.phase = phase;
        }

        public void TryShoot(Vector3 direction)
        {
            switch (phase)
            {
                case 0:
                    break;
                case 1:
                    firstPhaseShooter.TryShoot(direction);
                    break;
                case 2:
                    secondPhaseShooter.TryShoot(direction);
                    break;
                case 3:
                    thirdPhaseShooter.TryShoot(direction);
                    break;
                case 4:
                    thirdPhaseShooter.TryShoot(direction);
                    break;
            }
        }

        public void ShootFirstPhase(Vector3 direction)
        {
            firstPhaseShooter.TryShoot(direction);
        }

        public void ShootSecondPhase(Vector3 direction)
        {
            secondPhaseShooter.TryShoot(direction);
        }

        public void ShootThirdPhase()
        {
            for(int i = 0; i < SHOOTER_COUNT; i++)
            {
                Vector3 shootDirection = (Vector3.down + (((i+1) * SHOOT_SPREAD) - 0.5f) * 2f * Vector3.left).normalized;
                thirdPhaseShooters[i].TryShoot(shootDirection);
            }
        }

        public void DisableProjectile(Projectile proj)
        {
            switch (phase) {
                case 0:
                    break;
                case 1:
                    firstPhaseShooter.DisableProjectile(proj);
                    break;
                case 2:
                    secondPhaseShooter.DisableProjectile(proj);
                    break;
                case 3:
                    thirdPhaseShooter.DisableProjectile(proj);
                    break;
                case 4:
                    thirdPhaseShooter.DisableProjectile(proj);
                    break;
            }
        }
        
        public void OnShooterDestroy()
        {
            switch (phase) {
                case 0:
                    break;
                case 1:
                    firstPhaseShooter.OnShooterDestroy();
                    break;
                case 2:
                    secondPhaseShooter.OnShooterDestroy();
                    break;
                case 3:
                    thirdPhaseShooter.OnShooterDestroy();
                    break;
                case 4:
                    thirdPhaseShooter.OnShooterDestroy();
                    break;
            }
        }
    }
}