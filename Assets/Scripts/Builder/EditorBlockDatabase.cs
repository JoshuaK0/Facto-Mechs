using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockDatabase", menuName = "ScriptableObjects/TankEditor", order = 1)]
public class EditorBlockDatabase : ScriptableObject
{
    [SerializeField] List<EditorBlock> availableBlocks;

    public List<EditorBlock> GetBlocks()
    {
        return availableBlocks;
    }

    public List<EditorBlock> GetAvailableBlocks()
    {
        return availableBlocks;
    }
}
