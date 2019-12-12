public interface IOnChargeCallback
{
    void OnCharge();
    void OnChargeEnd();
    void SetEmissionLevel(float emissionPercent);
}