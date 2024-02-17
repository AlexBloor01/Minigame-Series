using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids
{
    public class Cannon : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D ship;
        [SerializeField] private Transform cannon;
        [SerializeField] private GameObject bullet;

        public void FireCannon()
        {
            Quaternion bulletDirection = Quaternion.Euler(0f, 0f, ship.rotation);
            GameObject newBullet = Instantiate(bullet, cannon.position, bulletDirection);
        }
    }
}
