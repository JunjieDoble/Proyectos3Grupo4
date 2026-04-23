using UnityEngine;
using UnityEngine.InputSystem;

namespace _Code.Scripts.Character
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Player))]
    public class CameraController : MonoBehaviour, IController
    {
        [SerializeField] private PlayerParameters parameters;
        [SerializeField] private Transform pitchController;

        private float _yaw;
        private float _pitch;

        public bool IsEnabled { get; set; }

        private void Awake()
        {
            _yaw = transform.eulerAngles.y;
            _pitch = pitchController.localEulerAngles.x;
            Cursor.lockState = CursorLockMode.Locked;
            if (!parameters)
                parameters = ScriptableObject.CreateInstance<PlayerParameters>();
            GetComponent<Player>()?.AddController(this);
        }

        void Update()
        {
            if (!IsEnabled) return;
            transform.rotation = Quaternion.Euler(0f, _yaw, 0f);
            pitchController.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
        }

        public void OnLook(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed) return;

            AddLookDelta(ctx.ReadValue<Vector2>());
        }

        public void AddLookDelta(Vector2 delta)
        {
            _yaw += delta.x * parameters.mouseSensitivity;
            _pitch -= delta.y * parameters.mouseSensitivity;
            _pitch = Mathf.Clamp(_pitch, parameters.minPitch, parameters.maxPitch);
        }
        
    }
}
