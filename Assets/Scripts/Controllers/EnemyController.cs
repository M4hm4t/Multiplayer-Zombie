using System;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;
using Code;

namespace Code
{
    public class EnemyController : MyCharacterController
    {
        PlayerController player;

        [SerializeField] private ParticleController deadParticlePrefab;

        private void Start()
        {

            GameManager.Instance.EnemyAmount++;
        }

        private void FixedUpdate()
        {
            var hits = Physics.OverlapSphere(transform.position, 40f);

            foreach (var hit in hits)
            {
                if (hit.transform.CompareTag($"Player"))
                {
                    player = hit.GetComponent<PlayerController>();
                    if (player.isDead)
                    {
                        player = null;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (player == null)
            {
                return;
            }
            var delta = -transform.position + player.transform.position;
            delta.y = 0;
            var direction = delta.normalized;
            Move(direction);
            transform.LookAt(player.transform);
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.transform.CompareTag($"Bullet"))
            {
            other.gameObject.SetActive(false);
                GetHit();
            }
        }

        public void GetHit()
        {
            gameObject.SetActive(false);
            MatchManager.Instance.SetEnemyKill();
            Instantiate(deadParticlePrefab, transform.position, Quaternion.identity);
            GameManager.Instance.EnemyDeadCounter++;
        }
    }
}