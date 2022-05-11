using System;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;
using Code;

namespace Code
{
	public class EnemyController : MyCharacterController
	{
		[SerializeField] private ParticleController deadParticlePrefab;

		private void Start()
		{
			
			GameManager.Instance.EnemyAmount++;
		}

		private void FixedUpdate()
		{
			var player = PlayerController.Instance;
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
				gameObject.SetActive(false);
				other.gameObject.SetActive(false);
				Instantiate(deadParticlePrefab, transform.position, Quaternion.identity);
				GameManager.Instance.EnemyDeadCounter++;
			}
		}
	}
}