using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBlock : MonoBehaviour
{
    void Update()
    {
        transform.localPosition = RoundVector(transform.localPosition);
    }
    public InteractableBlock GetInteractable(Vector3 position)
    {
        try
        {
            foreach (InteractableBlock block in ConveyorSystemManager.Instance().InteractableBlocks())
            {
                if (block != null && block != this)
                {
                    if (Vector3.Distance(block.transform.position, transform.position + position) < 0.25f)
                    {
                        return block;
                    }
                }
            }
        }
        catch (System.Exception)
        {

            return null;
        }
        
        return null;
    }
    public Vector3Int RoundVector(Vector3 input)
    {
        return new Vector3Int
            (
            Mathf.RoundToInt(input.x),
            Mathf.RoundToInt(input.y),
            Mathf.RoundToInt(input.z)
            );
    }
}
