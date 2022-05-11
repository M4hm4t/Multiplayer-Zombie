using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Photon.Pun;
using Code;

namespace Code
{
    public class PlayerController : MyCharacterController
    {
        public static PlayerController Instance { get; private set; }

        [SerializeField] private ScreenTouchController input;
        [SerializeField] private ShootController shootController;
   
        private readonly List<Transform> _enemies = new();
        private bool _isShooting;
       

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
            if (_view.IsMine)
            {
                FindObjectOfType<PlayerFollow>().SetCameraTarget(transform); //player finds the camera
            }
            input = FindObjectOfType<ScreenTouchController>();
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
            //if (_enemies.Count > 0)
              //  transform.LookAt(_enemies[0]);

            if (mIsControlEnabled && _view.IsMine)
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

                transform.Rotate(0, turnAmount * RotationSpeed * Time.deltaTime, 0);

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
            }
        }

        private void FixedUpdate()
        {
            var direction = new Vector3(input.Direction.x, 0, input.Direction.y);
            Move(direction);
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag($"Enemy"))
            {
                Dead();
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
                    var enemy = _enemies[0];
                    var myTransform = transform;
                    var position = myTransform.position+Vector3.up;
                    var direction = enemy.transform.position - position;
                    direction.y = 0;
                    direction = direction.normalized;
                    shootController.Shoot(direction, position);
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
            GameManager.Instance.GameOver();
        }

        private void OnReachSavePoint()
        {
            GameManager.Instance.Win();
        }



    }
}
