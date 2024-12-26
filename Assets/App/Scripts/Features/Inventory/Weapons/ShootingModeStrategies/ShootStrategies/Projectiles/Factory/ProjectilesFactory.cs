using App.Scripts.Scenes.Gameplay.Weapons;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace App.Scripts.Features.Inventory.Weapons.ShootingModeStrategies.ShootStrategies.Projectiles.Factory
{
    public class ProjectilesFactory
    {
        private DiContainer _container;

        public ProjectilesFactory(DiContainer container)
        {
            _container = container;
        }

        public Projectile CreateProjectile(Projectile projectile)
        {
            var newProjectile 
                = PhotonNetwork.Instantiate(projectile.name, Vector3.zero, Quaternion.identity)
                    .GetComponent<Projectile>();
            newProjectile.gameObject.SetActive(false);
            return  newProjectile;
        }
        
        
    }
}