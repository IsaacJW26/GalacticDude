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
        private CharacterShoot thirdPhaseShooter1;
        private CharacterShoot thirdPhaseShooter2;
        private int phase = 0;

        public void SetPhase(int phase)
        {
            if(this.phase == 2 && phase == 3)
            {
                thirdPhaseShooter1 = Instantiate(thirdPhaseShooter);
                thirdPhaseShooter1.transform.SetParent(thirdPhaseShooter.transform.parent);
                thirdPhaseShooter2 = Instantiate(thirdPhaseShooter);
                thirdPhaseShooter2.transform.SetParent(thirdPhaseShooter.transform.parent);
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

        public void ShootFirstPhase()
        {
            Vector3 direction = Vector3.down;
            firstPhaseShooter.TryShoot(direction);
        }

        public void ShootSecondPhase(Vector3 direction)
        {
            secondPhaseShooter.TryShoot(direction);
        }

        public void ShootThirdPhase(Vector3 direction)
        {
            thirdPhaseShooter.TryShoot(direction);
            
            Vector3 directionL, directionR;
            directionL = (Vector3.down * 2f + Vector3.left).normalized;
            directionR = (Vector3.down * 2f + Vector3.right).normalized;

            thirdPhaseShooter1.TryShoot(directionL);
            thirdPhaseShooter2.TryShoot(directionR);
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