using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Minimap : NetworkBehaviour {

    public Transform player;

    private void LateUpdate()
    {
        if(localPlayerAuthority)
        {
            
            Vector3 newPosition = player.position;

            newPosition.y = transform.position.y;
            transform.position = newPosition;
        }
        
    }

}
