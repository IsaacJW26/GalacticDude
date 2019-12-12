using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUpgradeSpeedUp : PlayerUpgrade
{
    public TestUpgradeSpeedUp(Sprite sprite) : base(sprite)
    {
    }

    public override void Initialise(ISpeed movement, MainCharacter player, IWeapon weapon)
    {
        base.Initialise(movement, player, weapon);

    }

    public override void OnChargeStart()
    {
        
    }

    public override void OnCharging()
    {

    }

    public override void OnMove(Vector3 direction)
    {

    }

    public override void OnRemove()
    {

    }

    public override void OnShoot()
    {

    }

    public override void UpdateFrame(Vector3 currentPosition)
    {
        this.movement.SpeedUp(5f, 60);
    }
}
