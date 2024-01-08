using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Class handles movement, actions, and main references to other systems
namespace Player
{
    public class Controller : MonoBehaviour
    {
        #region Fields

        [Header("UI References")]
        public GameObject _inventoryUI;

        [HideInInspector] public Rigidbody2D _rb;
        [HideInInspector] public CapsuleCollider2D _collider;
        [HideInInspector] public Animator _animator;
        public PlayerControls playerControls;

        private InputAction move;
        private InputAction interact;

        public float _speed = 3f;
        private Vector2 _movementInput;
        private Vector2 _moveDirection = Vector2.zero;

        #endregion

        #region Runtime
        private void Awake()
        {
            playerControls = new();

            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<CapsuleCollider2D>();
            _animator = GetComponentInChildren<Animator>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            // Movement related
            _moveDirection = move.ReadValue<Vector2>();

            // Menu related
            if (Keyboard.current.iKey.wasReleasedThisFrame) 
            { 
                OpenInventory();
            }
        }

        private void FixedUpdate()
        {
            ApplyMovement();
        }

        private void OnEnable()
        {
            move = playerControls.Player.Move;
            move?.Enable();
        }
        private void OnDisable()
        {
            move?.Disable();
        }
        #endregion

        #region Movement
        protected void ApplyMovement()
        {
            _rb.velocity = new Vector2(_moveDirection.x * _speed, _moveDirection.y * _speed);
        }
        #endregion

        #region UI
        /// <summary>
        /// Method for opening the inventory UI
        /// </summary>
        protected void OpenInventory()
        {
            if(_inventoryUI == null) { return; }

            bool isOpen = _inventoryUI.activeSelf;

            _inventoryUI.SetActive(!isOpen);
        }
        #endregion
    }
}

