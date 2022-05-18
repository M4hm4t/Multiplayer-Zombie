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
        PhotonView view;

        [SerializeField] private ParticleController deadParticlePrefab;

        void Start()
        {
            view = GetComponent<PhotonView>();
            if (view.IsMine)
            {
                Enemy();
            }
            
            GameManager.Instance.EnemyAmount++;
        }

        public void Enemy() // enemies finds the Player
        {
         
            Invoke("Enemy", 1);
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

        }

        void FixedUpdate()
        {
            if (player != null && view.IsMine)
            {
                var delta = -transform.position + player.transform.position;
                delta.y = 0;
                var direction = delta.normalized;
                Move(direction);
                transform.LookAt(player.transform);
            }
        }

      
        private void OnTriggerEnter(Collider other)
        {

            if (other.transform.CompareTag($"Bullet"))
            {
                other.gameObject.SetActive(false);
                GetHit();
            }
        }
        [PunRPC]
        public void SetHit()
        {
            gameObject.SetActive(false);
            MatchManager.Instance.SetEnemyKill();
            Instantiate(deadParticlePrefab, transform.position, Quaternion.identity);
            GameManager.Instance.EnemyDeadCounter++;
          
          
        }
        public void GetHit()
        {
            if (view.IsMine&& PhotonNetwork.IsMasterClient)
            {
                view.RPC("SetHit",RpcTarget.All);
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}