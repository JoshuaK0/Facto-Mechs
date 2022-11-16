using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBlock : MonoBehaviour
{
    Vector3Int previousPos;
    void Update()
    {
        transform.localPosition = RoundVector(transform.localPosition);
    }
    public InteractableBlock GetInteractable(Vector3 position)
    {
        try
        {
            foreach (InteractableBlock block in FindObjectOfType<ConveyorSystemManager>().InteractableBlocks())
            {
                if (block != null)
                {
                    if (transform != null)
                    {
                        if (Vector3.Distance(block.transform.position, transform.position + position) < 0.25f)
                        {
                            return block;
                        }
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

    public void UpdateCoord(Vector3 offset)
    {
        if(FindObjectOfType<ConveyorSystemManager>().occupiedSpaces.Contains(previousPos))
        {
            FindObjectOfType<ConveyorSystemManager>().occupiedSpaces.Remove(previousPos);
            AddCoords(offset);
        }
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

    public void AddCoords(Vector3 offset)
    {
        FindObjectOfType<ConveyorSystemManager>().occupiedSpaces.Add(RoundVector(transform.localPosition + offset));
    }

    public void UpdatePreviousPos(Vector3 offset)
    {
        previousPos = RoundVector(transform.localPosition + offset);
    }
}
