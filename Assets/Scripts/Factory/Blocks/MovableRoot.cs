using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableRoot : MonoBehaviour
{
    bool hasMoved;
    [SerializeField] List<MovablePart> movableParts = new List<MovablePart>();

    public BulletMod bulletMod;
    public void DoMove(Vector3 moveDir)
    {
        if(hasMoved)
        {
            return;
        }
        bool allBlocksCanMove = true;
        foreach(MovablePart part in movableParts)
        {
            if(part == null)
            {
                continue;
            }
            if (part.CanMove(moveDir))
            {
                continue;
            }
            allBlocksCanMove = false; 
            break;
        }
        if (allBlocksCanMove)
        {
            UpdatePartPrevCoords();
            transform.localPosition += moveDir;
            UpdatePartCoords();
            hasMoved = true;
            foreach(MovablePart part in movableParts )
            {
                if (part == null)
                {
                    continue;
                }
                part.MoveSuccess();
            }
        }
    }

    public void InitRoot()
    {
        hasMoved = false;
    }

    public void AddBlock(MovablePart part)
    {
        part.transform.SetParent(transform);
        movableParts.Add(part);
        if(part.bulletMod != null)
        {
            bulletMod = ModCombiner.CombineTwoMods(bulletMod, part.bulletMod);
        }
        

        part.SetRoot(this);
    }

    public void SetRootMod(BulletMod mod)
    {
        bulletMod = mod;
    }

    public void UpdatePartCoords()
    {
        foreach(MovablePart part in movableParts)
        {
            part.UpdateCoord(transform.localPosition);
        }
    }

    public void UpdatePartPrevCoords()
    {
        foreach (MovablePart part in movableParts)
        {
            part.UpdatePreviousPos(transform.localPosition);
        }
    }

    public void AddPartCoords()
    {
        foreach (MovablePart part in movableParts)
        {
            part.AddCoords(transform.localPosition);
        }
    }

    public List<MovablePart> GetBlocks()
    {
        return movableParts;
    }

    public void DestroyBlock()
    {
        foreach(MovablePart part in movableParts)
        {
            part.DestroyPart();
            ConveyorSystemManager.Instance().UpdateInteractables();
            Destroy(gameObject);
        }
    }
}
