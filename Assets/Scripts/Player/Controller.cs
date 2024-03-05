using Interacts;
using System.Collections.Generic;
using TilemapScripts;
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
        public GameObject _menuUI;

        [HideInInspector] public Rigidbody2D _rb;
        [HideInInspector] public CapsuleCollider2D _collider;
        [HideInInspector] public Animator _animator;
        [HideInInspector] public PlayerControls playerControls;
        public SpriteRenderer equipedSprite;
        public WeaponPoint weaponPoint;

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
        private bool multiGrid;
        private bool place;
        public bool canFire;
        private float attackTimer = 0f;
        private float timeBetweenShots = .5f;

        public bool isInteract = false;
        public bool isInventoryOpen = false;
        public bool isMenuOpen = false;
        public bool isUIOpen;

        [Header("Stats")]
        public float _speed = 3f;
        private float _currSpeed;
        public float _sprintMultiplier = 1.6f;
        public Vector2 sizeOfIA; // static for all tools for now
        float maxDistance;
        private Vector2 _movementInput;
        private Vector2 _mousePos;
        private Vector2 _moveDirection = Vector2.zero;
        public Character character;
        private float offsetDistance = 1.2f;

        [Header("Data")]
        Vector3Int selectedTilePosition;
        List <Vector3Int> selectedTiles;
        public Data.Item selectedItem;
        public ItemForPickup[] itemsToPickup;

        [SerializeField] ToolActions.Base onTilePickUp;
        [SerializeField] ItemHighlight itemHighlight;

        #endregion

        #region Runtime
        private void Awake()
        {
            controller = this;
            playerControls = new();
            maxDistance = sizeOfIA.x * sizeOfIA.y;
            selectedTiles = new List<Vector3Int>();
            weaponPoint = GetComponentInChildren<WeaponPoint>();

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
            RangedAttackMath();

            HandleUIInteraction();

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

                    if (!isInteract)
                    {
                        UseToolWorld();
                        character.GetTired(1);
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (!isInteract && useGrid)
                {
                    UseToolGrid();
                    character.GetTired(1);
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
        protected void HandleUIInteraction()
        {
            // Menu related
            if (Keyboard.current.iKey.wasReleasedThisFrame)
            {
                OpenInventory();
            }

            if (Keyboard.current.escapeKey.wasReleasedThisFrame)
            {
                if (isInventoryOpen)
                {
                    OpenInventory();
                }
                else
                {
                    OpenPauseMenu();
                }
            }

            isUIOpen = isInventoryOpen || isMenuOpen;
        }

        /// <summary>
        /// Method for opening the inventory UI
        /// </summary>
        protected void OpenInventory()
        {
            if(_inventoryUI == null) { return; }

            bool isOpen = _inventoryUI.activeSelf;

            _inventoryUI.SetActive(!isOpen);

            isInventoryOpen = !isOpen;
        }
        protected void OpenPauseMenu()
        {
            if (_menuUI == null) { return; }

            bool isOpen = _menuUI.activeSelf;

            _menuUI.SetActive(!isOpen);

            isMenuOpen = !isOpen;
        }
        #endregion

        #region Tools
        private bool UseToolWorld()
        {
            Vector2 position = _rb.position;

            if (selectedItem == null)
            {
                PickUpTile();
                return true;
            }

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
                if (selectedItem.onTileMapAction == null) { return; }

                int count = multiGrid ? selectedTiles.Count : 1;

                //Debug.Log(count);

                for (int i = 0; i < count; i++)
                {
                    if (multiGrid)
                    {
                        selectedTilePosition = selectedTiles[i];
                    }

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

                multiGrid = false;
                selectedTiles.Clear();
            }
        }

        private void Interact()
        {
            /// Handles collision for key input instead of mouse
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 position = _rb.position;
                _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
            inventoryManager.AddItem(itemsToPickup[id].item);
        }
        public void PickupItemList()
        {
            // Items player starts out with
            for (int i = 0; i < itemsToPickup.Length; i++)
            {
                for(int j = 0; j < itemsToPickup[i].count; j++)
                {
                    PickupItem(i);
                }
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
            onTilePickUp.OnApply(new Vector2(_mousePos.x, _mousePos.y - .5f));
        }
        private void Marker()
        {
            markerManager.markedCellPosition = selectedTilePosition;
            itemHighlight.cellPosition = selectedTilePosition;

            multiGrid = markerManager.isMultiple;
            place = markerManager.isPlace;

            if(multiGrid) { selectedTiles = markerManager.multiPositions; }
        }
        private void HandleSelection()
        {
            selectedItem = inventoryManager.selectedItem;

            if (selectedItem != null && 
                selectedItem.image != null) { equipedSprite.sprite = selectedItem.image; }
            else
            {
                equipedSprite.sprite = null;
            }

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
            if(selectedItem.isMelee)
            {
                MeleeAttack(damage);
            }
            else if(selectedItem.isRanged) 
            {
                if (canFire)
                {
                    RangedAttack(damage);
                    canFire = false;
                }
            }
        }

        private void MeleeAttack(float damage)
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
                    break;
                }
            }
        }

        // static damage for now got lazy
        private void RangedAttack(float damage)
        {
            if(selectedItem.projectile != null)
            {
                weaponPoint.Fire(selectedItem.projectile, damage);
            }
            else
            {
                Debug.Log("Projectile prefab not set for" + selectedItem.name);
            }
        }

        private void RangedAttackMath()
        {
            if (!canFire)
            {
                attackTimer += Time.deltaTime;
                if(attackTimer > timeBetweenShots)
                {
                    canFire = true;
                    attackTimer = 0;
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

