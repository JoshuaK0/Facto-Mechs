using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelderBlock : TickBlock
{
    InteractableBlock interactable;

    void Update()
    {
        if(interactable == null)
        {
            return;
        }
        InteractableBlock block1 = interactable.GetInteractable(transform.forward);
        if(block1 != null)
        {
            MovablePart part1 = block1.GetComponent<MovablePart>();
            if (part1 != null)
            {
                InteractableBlock block2 = interactable.GetInteractable(transform.right + transform.forward);
                if (block2 != null)
                {
                    MovablePart part2 = block2.GetComponent<MovablePart>();
                    if (part2 != null)
                    {
                        if(part1 != part2 && !part1.GetRoot().GetBlocks().Contains(part2))
                        {
                            GameObject oldRoot = part2.GetRoot().gameObject;
                            part1.GetRoot().AddBlock(part2);
                            DestroyImmediate(oldRoot);
                        }
                    }
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color= Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - transform.up);
        Gizmos.DrawLine(transform.position - transform.forward, transform.position -transform.up - transform.forward);
        Gizmos.DrawLine(transform.position - transform.up, transform.position-transform.forward - transform.up);
    }

    public override void DoInitTick()
    {
        interactable = GetComponent<InteractableBlock>();
    }

    public override void DoTick()
    {
        return;
    }
}
