using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraBehaviorScripts
{
    public class FollowPlayer : MonoBehaviour
    {
        public Transform player;

        // Update is called once per frame
        void Update()
        {
            if (player != null)
            {
                transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
            }
        }
    }
}


