using UnityEngine;

public interface IWeaponManager
{
    void OnCharge();
    void OnChargeEnd();
    void SetEmissionLevel(float emissionPercent);
    Vector2 GetPlayerDirection();
    Vector3 GetLaunchPosition();
}