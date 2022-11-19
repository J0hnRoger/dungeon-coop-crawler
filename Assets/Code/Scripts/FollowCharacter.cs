using Unity.Netcode;
using UnityEngine;

namespace DungeonCoop
{
    public class FollowCharacter: MonoBehaviour 
    {
        private Camera _camera;
        [SerializeField] private float _centeredZ = -13;
        private Transform _playerTransform; 
        
        // cached transform of the target
        private Transform _cameraTransform;

        // Cache for camera offset
        Vector3 cameraOffset = Vector3.zero;
        private Vector3 _difference;

        public void Awake()
        {
            _camera = GetComponent<Camera>();
            _cameraTransform = _camera.transform;
        }

        public void SetFollowedPlayer(Transform playerTransform)
        {
            _difference = playerTransform.position + transform.position;
            _playerTransform = playerTransform;
        }

        private void LateUpdate()
        {
            if (_playerTransform != null)
                Follow();
        }

        /// <summary>
        /// Follow the target smoothly
        /// </summary>
        void Follow()
        {
            _cameraTransform.position = new Vector3(_playerTransform.position.x + _difference.x, _difference.y, _playerTransform.position.z + _difference.z);
        }
    }
}