namespace App.Scripts.Features.Inventory.Weapons.ShootEffects
{
    public class PlayMuzzleFlash:ShootEffect
    {
        public override void Effect()
        {
            Weapon.RPCPlayMuzzleFlash(Weapon.ShootPointProvider.ShotPoint);
        }

        public override void Update()
        {
        }

        public override void Default()
        {
        }
    }
}