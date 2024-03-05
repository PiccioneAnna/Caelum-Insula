using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Visuals
{
    public class TriggerTransparency : MonoBehaviour
    {
        public List<SpriteRenderer> renderers;

        void OnTriggerEnter2D(Collider2D other)
        {
            //Debug.Log("Player has entered the trigger");

            if (other.gameObject.tag == "Player")
            {
                foreach (SpriteRenderer renderer in renderers)
                {
                    renderer.color = new Color(1f, 1f, 1f, 0.5f);
                }
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            //Debug.Log("Player has exited the trigger");

            if (other.gameObject.tag == "Player")
            {
                foreach (SpriteRenderer renderer in renderers)
                {
                    renderer.color = new Color(1f, 1f, 1f, 1f);
                }
            }
        }
    }
}


