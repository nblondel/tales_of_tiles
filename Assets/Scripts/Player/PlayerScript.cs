using System.Collections;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
    private Rigidbody2D _rb;
    private Vector3 _velocity = Vector3.zero;
    private bool _jumpAsked;
    private bool _stopJumpAsked;
    private bool _landing;
    private Vector2 _movement;
    private float _oldVelocityX;
    private PlayerSounds _playerSounds;

    public Animator animator;
    public LayerMask collisionLayer;
    public GameObject characterHolder;

    [Header("Steps")] 
    public GameObject steps;

    [Header("Physics")] 
    public float maxSpeed = 7f;
    public float linearDrag = 7f;
    public float gravity = 1f;
    public float fallMultiplier = 5f;
    
    [Header("Movements")] 
    public float horizontalSpeed = 6f;
    public float verticalSpeed = 3f;
    private bool _facingRight = true;
    private static bool _canMove;
    private static bool _stopMovement = true;
    private float _gravityScale;

    [Header("Jump")] 
    public float jumpSpeed = 6f;
    public float jumpDelay = 0.25f;
    private float _jumpTimer;

    [Header("Collisions")]
    public float groundLength = 0.72f;
    public Vector3 colliderOffset;
    private bool _onGround = true;
    private bool _wasOnGround = true;
    
    [Header("Camera target")]
    public Transform cameraTarget;
    public float cameraOffset;
    public float cameraSpeed;
    
    [HideInInspector] public bool isClimbing;
    
    // ---------------
    // UNITY FUNCTIONS
    // ---------------

    private void OnEnable() {
        InputsEventManager.OnSpacePressed += JumpInputPressed;
        InputsEventManager.OnSpaceReleased += JumpInputReleased;
        InputsEventManager.OnMovementKeyPressed += MoveInputPressed;
        DialogManager.OnPlayerDialog += PlayerDialogTriggered;
    }

    private void OnDisable() {
        InputsEventManager.OnSpacePressed -= JumpInputPressed;
        InputsEventManager.OnSpaceReleased -= JumpInputReleased;
        InputsEventManager.OnMovementKeyPressed -= MoveInputPressed;
        DialogManager.OnPlayerDialog -= PlayerDialogTriggered;
    }

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
        _playerSounds = GameObject.FindObjectOfType<PlayerSounds>();
    }

    private void Update() {
        var position = transform.position;
        _wasOnGround = _onGround;
        _onGround =
            Physics2D.Raycast(position + colliderOffset, Vector2.down, groundLength, collisionLayer) ||
            Physics2D.Raycast(position - colliderOffset, Vector2.down, groundLength, collisionLayer) ||
            isClimbing;
        
        if (!_wasOnGround && _onGround) {
            _landing = true;
            //StartCoroutine(JumpSqueeze(1.25f, 0.8f, 0.05f));
        } else {
            _landing = false;
        }

        if (_jumpAsked) {
            _jumpTimer = Time.fixedTime + jumpDelay;
            _jumpAsked = false;
        }

        animator.SetBool("onGround", _onGround);
        animator.SetBool("climb", isClimbing);
        _playerSounds.ProcessMovements(_canMove, _onGround, _landing, Mathf.Abs(_rb.velocity.x), isClimbing);
    }

    private void FixedUpdate() {
        if (_canMove) {
            Move(_movement.x, _movement.y);
            if (_jumpTimer > Time.fixedTime && (_onGround || isClimbing)) {
                Jump();
            } else if (_stopMovement == false) {
                Stop();
                _stopMovement = true;
            }
        }

        ModifyPhysics();
    }
    
    // ------------------
    // MOVEMENT FUNCTIONS
    // ------------------

    private void Move(float horizontalMovement, float verticalMovement) {
        var velocity = _rb.velocity;
        if (isClimbing) {
            Vector3 targetVelocity = new Vector2(0, verticalMovement * verticalSpeed);
            _rb.velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref _velocity, .05f);
        } else {
            Vector3 targetVelocity = new Vector2(horizontalMovement * horizontalSpeed, velocity.y);
            _oldVelocityX = _rb.velocity.x; 
            _rb.velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref _velocity, .05f);

            if (_oldVelocityX == 0f) {
                if(_rb.velocity.x > 0f || _rb.velocity.x < 0f) {
                    StartSteps();
                }
            }
            
            if ((horizontalMovement > 0 && !_facingRight) || (horizontalMovement < 0 && _facingRight)) {
                Flip();
            }
        }
        MoveCameraTarget(horizontalMovement);

        if (Mathf.Abs(_rb.velocity.x) > maxSpeed) {
            _rb.velocity = new Vector2(Mathf.Sign(_rb.velocity.x) * maxSpeed, _rb.velocity.y);
        }
        animator.SetFloat("speed", Mathf.Abs(_rb.velocity.x));
    }
    
    private void Jump() {
        StopSteps();
        animator.SetTrigger("jumpInitiated");
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        _jumpTimer = 0;
        //StartCoroutine(JumpSqueeze(0.8f, 1.1f, 0.1f));
    }

    private void ModifyPhysics() {
        var velocity = _rb.velocity;
        var changingDirections = (_movement.x > 0 && velocity.x < 0) || (_movement.x < 0 && velocity.x > 0);

        if (isClimbing) { // Climbing
            _rb.gravityScale = 0;
        } else if(_onGround) { // On ground
            if (Mathf.Abs(_movement.x) < 0.4f || changingDirections) {
                _rb.drag = linearDrag;
            } else {
                _rb.drag = 0f;
            }
            _rb.gravityScale = 0;
        } else { // In the air
            _rb.gravityScale = gravity;
            _rb.drag = linearDrag * 0.15f;
            if(_rb.velocity.y < 0) { // Falling
                animator.SetTrigger("jumpLanding");
                _rb.gravityScale = gravity * fallMultiplier;
            } else if(_rb.velocity.y > 0 && _stopJumpAsked) { // Hang time
                _rb.gravityScale = gravity * (fallMultiplier / 2);
            }
        }
    }

    public void Stop() {
        StopSteps();
        _movement = Vector2.zero;
        _rb.velocity = Vector3.zero;
        _rb.Sleep();
    }

    private void MoveCameraTarget(float horizontalMovement) {
        // Move the camera target
        var localPosition = cameraTarget.localPosition;
        float nextLocalPositionX;
        if (horizontalMovement == 0f) {
            // Go back to player position
            nextLocalPositionX = Mathf.Lerp(localPosition.x, 0f, cameraSpeed * 2f * Time.deltaTime);
        } else {
            // Go ahead the player position
            nextLocalPositionX = Mathf.Lerp(localPosition.x, cameraOffset * Mathf.Abs(horizontalMovement), cameraSpeed * Time.deltaTime);
        }
        cameraTarget.localPosition = new Vector3(nextLocalPositionX, localPosition.y, localPosition.z);
    }
    
    // ---------------
    // STEPS FUNCTIONS
    // ---------------

    private void StartSteps() {
        steps.SetActive(true);
    }

    public void StopSteps() {
        steps.SetActive(false);
    }
    
    // ----------------------------
    // ASSET MODIFICATION FUNCTIONS
    // ----------------------------

    private void Flip() {
        _facingRight = !_facingRight;
        transform.rotation = Quaternion.Euler(0, _facingRight ? 0 : 180, 0); // Flip the sprite X
    }

    private IEnumerator JumpSqueeze(float xSqueeze, float ySqueeze, float seconds) {
        var originalSize = Vector3.one;
        var newSize = new Vector3(xSqueeze, ySqueeze, originalSize.z);
        var t = 0f;
        while (t <= 1.0) {
            t += Time.deltaTime / seconds;
            characterHolder.transform.localScale = Vector3.Lerp(originalSize, newSize, t);
            yield return null;
        }

        t = 0f;
        while (t <= 1.0) {
            t += Time.deltaTime / seconds;
            characterHolder.transform.localScale = Vector3.Lerp(newSize, originalSize, t);
            yield return null;
        }
    }
    
    // ----------------
    // INPUTS FUNCTIONS
    // ----------------
    
    private void JumpInputPressed() {
        _jumpAsked = true;
        _stopJumpAsked = false;
    }

    private void JumpInputReleased() {
        _stopJumpAsked = true;
    }

    private void MoveInputPressed(float horizontalAxeValue, float verticalAxeValue) {
        _movement = new Vector2(horizontalAxeValue, verticalAxeValue);
    }

    // Triggerd by DialogManager when a dialog pops up
    private void PlayerDialogTriggered(bool playerCanMove) {
        SetCanMove(playerCanMove);
    }

    public void SetCanMove(bool newCanMove) {
        _stopMovement = newCanMove;
        _canMove = newCanMove;
    }
    
    // ---------------
    // DEBUG FUNCTIONS
    // ---------------
    
    private void OnDrawGizmos() {
        var position = transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(position + colliderOffset, position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(position - colliderOffset, position - colliderOffset + Vector3.down * groundLength);
    }
}