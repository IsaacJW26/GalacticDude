public interface IWeapon
{
    void RapidFire(float duration);
    void ForceShot();
    void FirerateBuffPercent(float percent);
    void DecreaseDebuffFireratePercent(float percent);
    void DamageBuffPercent(float percent);
    void DamageDebuffPercent(float percent);
    void BoostDamageFixed(int increase);
}
