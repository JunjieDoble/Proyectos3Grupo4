using UnityEngine;
using UnityEngine.InputSystem;

namespace _Code.Scripts.Character
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Player))]
    [DisallowMultipleComponent]
    public class MovementController : MonoBehaviour, IController
    {
        private PlayerParameters _playerParameters;
        private CharacterController _characterController;
        private Vector2 _movementInput;
        private float _verticalVelocity;
        private bool _canRun = true;
        private bool _running;
        private bool _crouchButtonHeld;
        private bool _crouching;
        private bool _grounded;
        private float _coyoteCounter;
        
        public bool IsRunning => _running;
        public bool IsCrouching => _crouching;
        public bool IsGrounded => _grounded;

        public bool IsEnabled { get; set; }
        
        void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            if (!_playerParameters)
            {
                _playerParameters = ScriptableObject.CreateInstance<PlayerParameters>();
            }
            IsEnabled = false;
            GetComponent<Player>()?.AddController(this);
        }

        public void LoadPlayerParameters(PlayerParameters playerParameters) => _playerParameters = playerParameters;

        void Update()
        {
            if (!IsEnabled) return;
            float deltaTime = Time.deltaTime;
            
            _coyoteCounter -= deltaTime;
            
            Vector3 relativeDirection = new Vector3(_movementInput.x, 0, _movementInput.y);
            Vector3 worldDirection = transform.TransformDirection(relativeDirection);

            float speed = CalculateCharacterSpeed();

            float gravity = Physics.gravity.y;
            
            if (_verticalVelocity < 0)
            {
                gravity *= _playerParameters.fallMultiplier;
            }
            
            _verticalVelocity += gravity * deltaTime;

            Vector3 movement = new Vector3(
                worldDirection.x * speed * deltaTime,
                _verticalVelocity * deltaTime,
                worldDirection.z * speed * deltaTime
            );

            CollisionFlags collisionFlags = _characterController.Move(movement);
            CheckCollisionFlags(collisionFlags);
        }

        void LateUpdate()
        {
            if (!IsEnabled) return;
            SetCharacterHeight();
        }

        private void SetCharacterHeight()
        {
            _characterController.height = _crouching ? _playerParameters.crouchHeight : _playerParameters.walkHeight;
        }

        private float CalculateCharacterSpeed()
        {
            float speed;
            if (_crouching)
            {
                speed = _playerParameters.crouchSpeed;
            }
            else if (_running)
            {
                speed = _playerParameters.runSpeed;                
            }
            else
            {
                speed = _playerParameters.walkSpeed;                
            }
            
            if (!_grounded)
            {
                speed *= _playerParameters.airControl;
            }
            
            return speed;
        }

        private void CheckCollisionFlags(CollisionFlags flags)
        {
            if ((flags & CollisionFlags.Below) != 0)
            {
                _grounded = true;
                _coyoteCounter = _playerParameters.coyoteTime;
                if (_verticalVelocity < -2)
                    _verticalVelocity = -2;
                Crouch(_crouchButtonHeld);
            }
            else
            {
                _grounded = false;
            }
        }
        
        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            _movementInput = input.sqrMagnitude > 1f ? input.normalized : input;
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.started) _running = _canRun;
            if (context.canceled) _running = false;
        }
        
        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started) Jump();
            if (context.canceled) StopJump();
        }
        
        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (context.started) _crouchButtonHeld = true;
            if (context.canceled) _crouchButtonHeld = false;
            Crouch(_crouchButtonHeld);
        }



        private void StopJump()
        {
            if (_verticalVelocity > 0)
            {
                _verticalVelocity *= _playerParameters.jumpStopMultiplier;
            }
        }

        private void Jump()
        {
            if (_grounded || _coyoteCounter > 0)
            {
                _verticalVelocity = _playerParameters.jumpSpeed;
                _grounded = false;
                _coyoteCounter = 0;
                _crouching = false;
            }
        }

        private void Crouch(bool crouching)
        {
            if (_grounded && _crouching != crouching)
            {
                if (crouching)
                {
                    _crouching = true;
                    return;
                }
                Vector3 headPosition = transform.position + Vector3.up * (_crouching ? _playerParameters.crouchHeight/2 : _playerParameters.walkHeight/2);
                Ray headCheckRay = new Ray(headPosition, Vector3.up);
                if (Physics.Raycast(headCheckRay, _playerParameters.walkHeight - _playerParameters.crouchHeight))
                {
                    _crouching = true;
                    return;
                }
            }
            _crouching = crouching;                
        }

        public void Teleport(Vector3 spawnPointPosition)
        {
            _characterController.enabled = false;
            transform.position = spawnPointPosition;
            _characterController.enabled = true;
        }
    }
}

