using UnityEngine;
using UnityEngine.InputSystem;

namespace _Code.Scripts.Character
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Player))]
    public class CameraController : MonoBehaviour, IController
    { 
        
        [SerializeField] private Transform pitchController;
        
        private PlayerParameters _playerParameters;
        
        private float _yaw;
        private float _pitch;
        private Vector2 _mouseLookInput;
        private Vector2 _gamepadLookInput;

        public bool IsEnabled { get; set; }
        
        public void LoadPlayerParameters(PlayerParameters playerParameters) => _playerParameters = playerParameters;
        public void OnPlayerRespawn(Vector3 _) { }

        private void Awake()
        {
            _yaw = transform.eulerAngles.y;
            float initialPitch = pitchController.localEulerAngles.x;
            if (initialPitch > 180f) initialPitch -= 360f;
            _pitch = initialPitch;
            Cursor.lockState = CursorLockMode.Locked;
            if (!_playerParameters)
                _playerParameters = ScriptableObject.CreateInstance<PlayerParameters>();
            IsEnabled = false;
            GetComponent<Player>()?.AddController(this);
        }

        void Update()
        {
            if (!IsEnabled) return;

            // Apply mouse input (already a delta, doesn't use deltaTime)
            if (_mouseLookInput.sqrMagnitude > 0.001f)
            {
                _yaw += _mouseLookInput.x * _playerParameters.mouseSensitivity;
                _pitch -= _mouseLookInput.y * _playerParameters.mouseSensitivity;
                _mouseLookInput = Vector2.zero; // Reset mouse input after applying
            }

            // Apply gamepad/joystick input (deflection, needs deltaTime and controllerSensitivity)
            if (_gamepadLookInput.sqrMagnitude > 0.001f)
            {
                _yaw += _gamepadLookInput.x * _playerParameters.controllerSensitivity * Time.deltaTime;
                _pitch -= _gamepadLookInput.y * _playerParameters.controllerSensitivity * Time.deltaTime;
            }

            _pitch = Mathf.Clamp(_pitch, _playerParameters.minPitch, _playerParameters.maxPitch);

            transform.rotation = Quaternion.Euler(0f, _yaw, 0f);
            pitchController.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
        }

        public void OnLook(InputAction.CallbackContext ctx)
        {
            Vector2 input = ctx.ReadValue<Vector2>();
            if (ctx.control.device is Pointer)
            {
                _mouseLookInput += input;
            }
            else
            {
                _gamepadLookInput = input;
            }
        }

        public void AddLookDelta(Vector2 delta)
        {
            _yaw += delta.x * _playerParameters.mouseSensitivity;
            _pitch -= delta.y * _playerParameters.mouseSensitivity;
            _pitch = Mathf.Clamp(_pitch, _playerParameters.minPitch, _playerParameters.maxPitch);
        }
        
    }
}
