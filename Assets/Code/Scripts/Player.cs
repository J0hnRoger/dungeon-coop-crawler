using Unity.Netcode;
using UnityEngine;

namespace dcc
{
    public class Player : NetworkBehaviour
    {
        private NetworkVariable<int> shoot = new NetworkVariable<int>(1,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);
        private void Update()
        {
            if (!IsOwner)
                return;
            
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
