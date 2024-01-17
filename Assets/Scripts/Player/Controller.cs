using Inventory;
using System.Collections;
using System.Collections.Generic;
using TilemapScripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
        [HideInInspector] public PlayerControls playerControls;

        [Header("Manager References")]
        [HideInInspector] public GameManager gameManager;
        [HideInInspector] public Inventory.Manager inventoryManager;
        [HideInInspector] public MarkerManager markerManager;
        [HideInInspector] TilemapScripts.Reader tilemapReader;
        [HideInInspector] public static Controller controller;

        private InputAction move;
        private InputAction interact;

        private bool useGrid;
        private bool selectable;
        public bool isInteract = false;

        [Header("Stats")]
        public float _speed = 3f;
        private float _currSpeed;
        public float _sprintMultiplier = 1.6f;
        [SerializeField] float maxDistance = 1.5f;
        private Vector2 _movementInput;
        private Vector2 _moveDirection = Vector2.zero;

        [Header("Data")]
        Vector3Int selectedTilePosition;
        public Data.Item selectedItem;
        public Data.Item[] itemsToPickup;

        [SerializeField] ToolActions.Base onTilePickUp;
        [SerializeField] ItemHighlight itemHighlight;

        #endregion

        #region Runtime
        private void Awake()
        {
            controller = this;
            playerControls = new();

            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<CapsuleCollider2D>();
            _animator = GetComponentInChildren<Animator>();
        }

        // Start is called before the first frame update
        void Start()
        {
            gameManager = GameManager.Instance;

            // Reference each manager in the GameManager
            inventoryManager = gameManager.inventory;
            tilemapReader = gameManager.reader;
            markerManager = gameManager.markerManager;

            PickupItemList();
        }

        // Update is called once per frame
        void Update()
        {
            // Movement related
            _moveDirection = move.ReadValue<Vector2>();

            HandleSelection();

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
            _currSpeed = Keyboard.current.shiftKey.isPressed ? _speed * _sprintMultiplier : _speed;

            _rb.velocity = new Vector2(_moveDirection.x * _currSpeed, _moveDirection.y * _currSpeed);
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

        #region Interactions
        public void PickupItem(int id)
        {
            inventoryManager.AddItem(itemsToPickup[id]);
        }
        public void PickupItemList()
        {
            // Items player starts out with
            for (int i = 0; i < itemsToPickup.Length; i++)
            {
                PickupItem(i);
            }
        }
        private void SelectTile()
        {
            selectedTilePosition = tilemapReader.GetGridPosition(tilemapReader.tilemap,Input.mousePosition, true);
            Marker();
        }
        private void Marker()
        {
            markerManager.markedCellPosition = selectedTilePosition;
            itemHighlight.cellPosition = selectedTilePosition;
        }
        private void HandleSelection()
        {
            selectedItem = inventoryManager.selectedItem;

            // Checks if grid needs to be displayed
            if (selectedItem == null || selectedItem.usesGrid)
            {
                useGrid = true;
                CanSelectCheck();
                SelectTile();
            }
            else
            {
                useGrid = false;
                markerManager.Show(false);
            }
        }
        #endregion


        #region Checks
        // Method checks if it is possible for the user to select the tile 
        // based on its position and the camera's posiiton
        void CanSelectCheck()
        {
            Vector2 characterPosition = transform.position;
            Vector2 cameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectable = Vector2.Distance(characterPosition, cameraPosition) < maxDistance;
            markerManager.Show(selectable);
            itemHighlight.CanSelect = selectable;
        }
        #endregion
    }
}

