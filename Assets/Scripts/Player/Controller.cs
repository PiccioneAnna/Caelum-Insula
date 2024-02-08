using Interacts;
using Inventory;
using System.Collections;
using System.Collections.Generic;
using TilemapScripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using UnityEngine.Tilemaps;
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

        [Header("Conditions")]
        private bool useGrid;
        private bool selectable;
        public bool isInteract = false;
        public bool isUIOpen = false;

        [Header("Stats")]
        public float _speed = 3f;
        private float _currSpeed;
        public float _sprintMultiplier = 1.6f;
        public Vector2 sizeOfIA; // static for all tools for now
        float maxDistance = 1.5f;
        private Vector2 _movementInput;
        private Vector2 _moveDirection = Vector2.zero;
        public Character character;
        private float offsetDistance = 1.2f;

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

            HandleSelection();
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

            if(Input.GetMouseButtonDown(0))
            {
                WeaponAction();
                character.GetTired(1);
            }

            if (Input.GetMouseButtonDown(0))
            {             
                if (!isInteract && !isUIOpen)
                {
                    Interact();

                    if(!isInteract && useGrid)
                    {
                        UseToolGrid();
                        character.GetTired(1);
                    }
                    else if (!isInteract)
                    {
                        UseToolWorld();
                        character.GetTired(1);
                    }

                }
            }
        }

        private void FixedUpdate()
        {
            ApplyMovement();

            // Passive Regen
            character.Rest(.10f);
            character.Heal(.05f);
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

            isUIOpen = !isOpen;
        }
        #endregion

        #region Tools
        private bool UseToolWorld()
        {
            Vector2 position = _rb.position;

            if (selectedItem == null) { return false; }
            if (selectedItem.onAction == null) { return false; }

            bool complete = selectedItem.onAction.OnApply(position);

            // Checks if item used can be removed from inventory
            if (complete)
            {
                if (selectedItem.onItemUsed != null)
                {
                    selectedItem.onItemUsed.OnItemUsed(selectedItem, inventoryManager);
                }
            }

            return complete;
        }

        public void UseToolGrid()
        {
            if (selectable)
            {
                if (selectedItem == null)
                {
                    PickUpTile();
                    return;
                }
                if (selectedItem.onTileMapAction == null) { return; }

                bool complete = selectedItem.onTileMapAction.OnApplyToTileMap(selectedTilePosition, tilemapReader, selectedItem);

                // Checks if item used can be removed from inventory
                if (complete)
                {
                    if (selectedItem.onItemUsed != null)
                    {
                        selectedItem.onItemUsed.OnItemUsed(selectedItem, inventoryManager);
                    }
                }
            }
        }

        private void Interact()
        {
            /// Handles collision for key input instead of mouse
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 position = _rb.position;
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfIA.x);

                foreach (Collider2D c in colliders)
                {
                    Interactable obj = c.GetComponent<Interactable>();
                    if (obj != null)
                    {
                        obj.Interact(controller);
                    }
                }
            }
        }

        private void WeaponAction()
        {
            //Player can only attack withitems marked as weapons
            if (selectedItem != null && selectedItem.isWeapon)
            {
                Attack(selectedItem.damage);
            }
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
        private void PickUpTile()
        {
            if (onTilePickUp == null) { return; }
            onTilePickUp.OnApplyToTileMap(selectedTilePosition, tilemapReader, null);
        }
        private void Marker()
        {
            markerManager.markedCellPosition = selectedTilePosition;
            itemHighlight.cellPosition = selectedTilePosition;
        }
        private void HandleSelection()
        {
            selectedItem = inventoryManager.selectedItem;

            if (selectedItem == null || !selectedItem.usesGrid)
            {
                useGrid = false;
                markerManager.Show(false);
                return;
            }

            // Checks if grid needs to be displayed
            useGrid = true;
            CanSelectCheck();
            SelectTile();
        }
        public void Attack(float damage)
        {
            Vector2 position = _rb.position + _moveDirection * offsetDistance;

            Collider2D[] targets = Physics2D.OverlapBoxAll(position, sizeOfIA, 0f);

            foreach (Collider2D c in targets)
            {
                if (c.TryGetComponent<Damageable>(out var damageable))
                {
                    damageable.TakeDamage(damage);
                    break;
                }

                Damageable damageable1 = c.gameObject.GetComponentInParent<Damageable>();

                if (damageable1 != null)
                {
                    damageable1.TakeDamage(damage);
                }
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

