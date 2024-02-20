using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids
{
    public class Bullet : MonoBehaviour
    {
        [Header("Fired from player Ship Cannon Transform")]
        Coroutine coroutine;  //this Coroutines, the need to be shut off.
        [SerializeField] private Rigidbody2D rb; //this Rigidbody2D.

        [SerializeField] private float depreciateTime = 1.75f; //time for bullets to disappear.
        [SerializeField] private float bulletSpeed = 750f; //Speed of bullet after rigidbody force added.

        private void Start()
        {
            rb.AddRelativeForce(new Vector2(0f, 1f) * bulletSpeed * Time.deltaTime, ForceMode2D.Impulse);
            coroutine = StartCoroutine(DepreciateBullet());
        }

        //destory bullet after 1.25 seconds.
        IEnumerator DepreciateBullet()
        {
            yield return new WaitForSeconds(depreciateTime);
            Destroy(gameObject);
        }

        //On trigger Enter due to ship Rigidbody collision unwanted.
        //activate asteroid break apart script.
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Asteroid")
            {
                other.gameObject.GetComponent<Asteroid>().BreakApart();
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
                Destroy(gameObject);
            }
        }

        //Keep bullets inside the play area.
        private void LateUpdate()
        {
            GameAssets.ContainingArea(gameObject);
        }
    }
}