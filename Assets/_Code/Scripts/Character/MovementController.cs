using UnityEngine;
using UnityEngine.InputSystem;

namespace _Code.Scripts.Character
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]
    [DisallowMultipleComponent]
    public class MovementController : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private PlayerParameters parameters;

        private CharacterController _characterController;
        private Vector2 _movementInput;
        private float _verticalVelocity;
        private bool _canRun = true;
        private bool _running;
        private bool _crouchButtonHeld;
        private bool _crouching;
        private bool _grounded;
        
        public bool IsRunning => _running;
        public bool IsCrouching => _crouching;
        public bool IsGrounded => _grounded;

        void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            if (!parameters)
            {
                parameters = ScriptableObject.CreateInstance<PlayerParameters>();
            }
        }

        void Update()
        {
            float deltaTime = Time.deltaTime;
            
            Vector3 relativeDirection = new Vector3(_movementInput.x, 0, _movementInput.y);
            Vector3 worldDirection = transform.TransformDirection(relativeDirection);

            float speed = CalculateCharacterSpeed();

            float gravity = Physics.gravity.y;
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
            SetCharacterHeight();
        }

        private void SetCharacterHeight()
        {
            _characterController.height = _crouching ? parameters.crouchHeight : parameters.walkHeight;
        }

        private float CalculateCharacterSpeed()
        {
            float speed;
            if (_crouching)
            {
                speed = parameters.crouchSpeed;
            }
            else if (_running)
            {
                speed = parameters.runSpeed;                
            }
            else
            {
                speed = parameters.walkSpeed;                
            }
            
            if (!_grounded)
            {
                speed *= parameters.airControl;
            }
            
            return speed;
        }

        private void CheckCollisionFlags(CollisionFlags flags)
        {
            if ((flags & CollisionFlags.Below) != 0)
            {
                _grounded = true;
                if (_verticalVelocity < -2)
                    _verticalVelocity = -2; // Keep player grounded
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
                _verticalVelocity *= parameters.jumpStopMultiplier;
            }
        }

        private void Jump()
        {
            if (_grounded)
            {
                _verticalVelocity = parameters.jumpSpeed;
                _grounded = false;
                _crouching = false;
            }
        }

        private void Crouch(bool crouching)
        {
            if (_grounded)
            {
                _crouching = crouching;
                return;
            }
            _crouching = false;
        }
    }
}

