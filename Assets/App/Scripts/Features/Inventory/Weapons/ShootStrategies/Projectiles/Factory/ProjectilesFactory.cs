﻿using Photon.Pun;
using UnityEngine;
using Zenject;

namespace App.Scripts.Features.Inventory.Weapons.ShootStrategies.Projectiles.Factory
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
        
        public Projectile CreateProjectile(Projectile projectile, Vector3 position, Quaternion rotation)
        {
            var newProjectile 
                = PhotonNetwork.Instantiate(projectile.name, position, rotation)
                    .GetComponent<Projectile>();
            newProjectile.gameObject.SetActive(false);
            return  newProjectile;
        }
        
        
    }
}