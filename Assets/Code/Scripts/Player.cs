using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DungeonCoop
{
    public class Player : NetworkBehaviour
    {
        [SerializeField] private bool _isOnline = true;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private float _speed = 5;
        [SerializeField] private float _rotationSpeed = 10;
        [SerializeField] private float _mouseSmoothTime = 0.03f;
        [SerializeField] private Camera _camera;

        private PlayerControls _playerControls;
        private InputAction _moveAction;
        private InputAction _lookAction;
        private Vector2 currentMouseDelta = Vector2.zero;
        private Vector2 currentMouseDeltaVelocity = Vector2.zero;
        private float _cameraPitch = 0.0f;

        private void Awake()
        {
            _playerControls = new PlayerControls();
        }

        private void OnEnable()
        {
            _playerControls.Enable();
            _moveAction = _playerControls.FindAction("move");
            _lookAction = _playerControls.FindAction("look");
        }

        private void OnDisable()
        {
            _playerControls.Disable();
        }

        private void FixedUpdate()
        {
            if (_isOnline && !IsOwner)
                return;

            Move();
            RotateTowardCamera();
        }
        
        /// <summary>
        /// Retourne les coordonn�es du point d'intersection d'un point de l'�cran sur le sol 
        /// </summary>
        /// <param name="screenPoint"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Vector3 GetWorldPointFromScreenPoint(Vector3 screenPoint, float height)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPoint);
            Plane plane = new Plane(Vector3.up, new Vector3(0, height, 0));
            float distance;
            if (plane.Raycast(ray, out distance))
                return ray.GetPoint(distance);
            return Vector3.zero;
        }
        
        private void RotateTowardCamera()
        {
            var mousePosition = _lookAction.ReadValue<Vector2>();
            // On tourne le personnage vers la souris - on affecte la hauteur du personnage comme point de rep�re  
            Vector3 mouseDirection = GetWorldPointFromScreenPoint(mousePosition, _rb.position.y);
            // On soustrait la position actuelle du joueur  
            if (mouseDirection != Vector3.zero)
            {
                Vector3 playerDirection = mouseDirection - _rb.transform.position;
                FaceDirection(playerDirection);
            }
        }

        /// <summary>
        /// Applique une rotation sur le transforme pour que le personnage tourne vers la souris
        /// </summary>
        public void FaceDirection(Vector3 dir)
        {
            if (dir == Vector3.zero && transform != null)
                return;

            Quaternion desiredRotation = Quaternion.LookRotation(dir);
            _rb.rotation = Quaternion.Slerp(_rb.rotation, desiredRotation, _rotationSpeed * Time.deltaTime);
        }

        private void Look()
        {
            // check mouse position 
            Vector2 lookInput = _lookAction.ReadValue<Vector2>();

            currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, lookInput, ref currentMouseDeltaVelocity,
                _mouseSmoothTime);

            _cameraPitch = Mathf.Clamp(_cameraPitch, -90.0f, 90.0f);

            transform.Rotate(Vector3.up * currentMouseDelta.x * 3.5f);

            Vector3 lookDirection = new Vector3(lookInput.x, 0, lookInput.y);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(lookDirection);
            Debug.DrawLine(Camera.main.transform.position, worldPosition);
            // rotate caracter 
            _rb.transform.LookAt(worldPosition);
        }

        private void Move()
        {
            Vector2 input = _moveAction.ReadValue<Vector2>();
            Vector3 direction = new Vector3(input.x, 0, input.y);
            _rb.MovePosition(transform.position + direction * _speed * Time.deltaTime);
        }

        private void SimpleMove()
        {
            Vector3 moveDir = new Vector3(0, 0, 0);
            if (Input.GetKey(KeyCode.Z)) moveDir.z = +1f;
            if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
            if (Input.GetKey(KeyCode.Q)) moveDir.x = -1f;
            if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;

            float moveSpeed = 6f;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
    }
}