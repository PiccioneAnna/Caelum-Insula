using System.Collections.Generic;
using UnityEngine;
public enum ResourceType
{
    Undefined,
    Tree,
    Mineral,
}

namespace ToolActions
{

    // Base class for any tools that 'hit'
    public class ToolHit : MonoBehaviour
    {
        public virtual void Hit() { }

        public virtual bool CanBeHit(List<ResourceType> canBeHit)
        {
            return true;
        }
    }

    [CreateAssetMenu(menuName = "Data/Tool Action/Enviro Hit")]
    public class EnviroHit : Base
    {
        [SerializeField] float sizeOfInteractableArea = 1;
        [SerializeField] List<ResourceType> canHitNodesOfType;
        public override bool OnApply(Vector2 worldPoint)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(worldPoint, sizeOfInteractableArea);

            Debug.Log("Using on apply");

            foreach (Collider2D c in colliders)
            {
                if(c.TryGetComponent<ToolHit>(out var hit))
                {
                    if (hit.CanBeHit(canHitNodesOfType))
                    {
                        hit.Hit();
                        return true;
                    }
                }
            }

            return false;
        }
    }
}


