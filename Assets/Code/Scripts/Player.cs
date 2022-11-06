using Unity.Netcode;
using UnityEngine;

namespace dcc
{
    public class Player : NetworkBehaviour
    {
        [SerializeField] private bool _isOnline = true;
        [SerializeField] private Rigidbody _rb;

        [SerializeField] private InputAction _playerInputs; 
        private void Update()
        {
            if (_isOnline && !IsOwner)
                return;
            
            GatherInput();
            // SimpleMove();
        }

        private void FixedUpdate()
        {
            
        }

        private void GatherInput()
        {
            // read values  
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
