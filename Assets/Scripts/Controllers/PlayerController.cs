using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Photon.Pun;
using Code;
using System.Linq;

namespace Code
{
    public class PlayerController : MyCharacterController
    {
        public static PlayerController Instance { get; private set; }
        public float checkRadius;
        public LayerMask checkLayers;
        [SerializeField] private ScreenTouchController input;
       // [SerializeField] private Shield shield;
        [SerializeField] private ShootController shootController;

        private readonly List<Transform> _enemies = new();
        private bool _isShooting;
        public bool isDead;
        Collider targetEnemy;



        private void Awake()
        {
            Instance = this;
        }
        public PhotonView _view;
        #region Private Members
        // private Animator _animator;
        private CharacterController _characterController;
        private float Gravity = 20.0f;
        private Vector3 _moveDirection = Vector3.zero;
        #endregion
        #region Public Members
        public float Speed = 5.0f;
        public float RotationSpeed = 240.0f;
        public GameObject Hand;
        public float JumpSpeed = 7.0f;
        #endregion

        // Use this for initialization
        void Start()
        {

            //GameObject[] allChidren = this.gameObject.GetComponentsInChildren<GameObject>();
            //foreach (GameObject t in allChidren)
            //{
            //    allChidren[4] = FindObjectOfType<Shield>();
            //}
            if (_view.IsMine)
            {
                FindObjectOfType<Shield>().SetShield(gameObject);
                FindObjectOfType<PlayerFollow>().SetCameraTarget(transform); //player finds the camera
                FindObjectOfType<SkillController>().SetPlayer(this); //Skill finds the player
 
            }
            input = FindObjectOfType<ScreenTouchController>();
           // shield = FindObjectOfType<Shield>();

            // _animator = GetComponent<Animator>();
            _characterController = GetComponent<CharacterController>();

        }
        [SerializeField]
        private bool mIsControlEnabled = true;

        public void EnableControl()
        {
            mIsControlEnabled = true;
        }

        public void DisableControl()
        {
            mIsControlEnabled = false;
        }

        private Vector3 mExternalMovement = Vector3.zero;

        public Vector3 ExternalMovement
        {
            set
            {
                mExternalMovement = value;
            }
        }

        void LateUpdate()
        {
            if (mExternalMovement != Vector3.zero)
            {
                _characterController.Move(mExternalMovement);
            }
        }

        // Update is called once per frame
        void Update()
        {


            if (mIsControlEnabled && _view.IsMine && !isDead)
            {

                // Get Input for axis
                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");
                if (h == 0)
                {
                    h = input.Direction.x;
                }
                if (v == 0)
                {
                    v = input.Direction.y;
                }

                // Calculate the forward vector
                Vector3 camForward_Dir = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
                Vector3 move = v * camForward_Dir + h * Camera.main.transform.right;

                if (move.magnitude > 1f) move.Normalize();


                // Calculate the rotation for the player
                move = transform.InverseTransformDirection(move);

                // Get Euler angles
                float turnAmount = Mathf.Atan2(move.x, move.z);
                if (v != 0 && h != 0)
                {
                    transform.Rotate(0, turnAmount * RotationSpeed * Time.deltaTime, 0);
                }

                if (_characterController.isGrounded || mExternalMovement != Vector3.zero)
                {
                    _moveDirection = transform.forward * move.magnitude;

                    _moveDirection *= Speed;

                    if (Input.GetButton("Jump"))
                    {
                        // _animator.SetBool("is_in_air", true);
                        _moveDirection.y = JumpSpeed;

                    }
                    else
                    {
                        // _animator.SetBool("is_in_air", false);
                        //_animator.SetBool("run", move.magnitude > 0);
                    }
                }
                else
                {
                    Gravity = 20.0f;
                }
                _moveDirection.y -= Gravity * Time.deltaTime;
                _characterController.Move(_moveDirection * Time.deltaTime);


                if (h == 0 && v == 0)
                {

                    Collider[] colliders = Physics.OverlapSphere(transform.position, checkRadius, checkLayers);
                    Array.Sort(colliders, new DistanceComparer(transform));
                    foreach (Collider item in colliders)
                    {
                        Debug.Log(item.name);


                        //targetEnemy = item;
                        // float speed = 100f;
                        // var look = targetEnemy.transform.position - transform.position;
                        // look.y = 0;
                        // var targetrotation = Quaternion.LookRotation(targetEnemy.transform.position);
                        // transform.rotation = Quaternion.Lerp(transform.rotation, targetrotation, Time.deltaTime * speed);
                        transform.LookAt(item.transform.position);

                    }


                }

            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_view.IsMine)
            {
                if (collision.transform.CompareTag($"Enemy"))
                {
                    Dead();
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag($"FinishPoint"))
            {
                OnReachSavePoint();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.transform.CompareTag($"Enemy"))
            {
                if (!_enemies.Contains(other.transform))
                    _enemies.Add(other.transform);
                // HitEnemies();
                AutoShoot();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.transform.CompareTag($"Enemy"))
            {
                _enemies.Remove(other.transform);
            }
        }

        private void AutoShoot()
        {
            IEnumerator Do()
            {
                while (_enemies.Count > 0)
                {
                    var hits = Physics.RaycastAll(transform.position, transform.forward, 20f);

                    foreach (var hit in hits)
                    {
                        if (hit.collider.tag == "Enemy")
                        {
                            var myTransform = transform;
                            var position = myTransform.position + Vector3.up;
                            if (!isDead)
                            {
                                shootController.Shoot(transform.forward, position);

                            }
                        }
                    }
                    //var enemy = _enemies[0];
                    //var myTransform = transform;
                    //var position = myTransform.position + Vector3.up;
                    //var direction = enemy.transform.position - position;
                    //direction.y = 0;
                    //direction = direction.normalized;
                    //if (!isDead)
                    //{
                    //    shootController.Shoot(direction, position);
                    //}
                    _enemies.RemoveAt(0);
                    yield return new WaitForSeconds(shootController.Delay);
                }

                _isShooting = false;
            }

            if (!_isShooting)
            {
                _isShooting = true;
                StartCoroutine(Do());
            }
        }

        private void Dead()
        {
            if (_view.IsMine)
            {
                _view.RPC("SetDead", RpcTarget.All);
            }
        }
        [PunRPC]
        public void SetDead()
        {
            PhotonNetwork.LeaveRoom();
            isDead = true;
            GameManager.Instance.GameOver();
        }
        private void OnReachSavePoint()
        {
            GameManager.Instance.Win();
        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, checkRadius);
        }
    }
}
