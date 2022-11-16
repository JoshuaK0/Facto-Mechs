using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorInventoryManager : MonoBehaviour
{
    static EditorInventoryManager instance;

    [SerializeField]
    EditorBlockDatabase blockDatabase;

    [SerializeField] Button buttonPrefab;
    [SerializeField] RectTransform inventoryParent;

    [SerializeField] BuildController buildController;

    public static EditorInventoryManager Instance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    public void SelectedBlockButton(EditorBlock block)
    {
        buildController.SetEditingBlock(block);
    }

    void Start()
    {
        foreach(EditorBlock b in blockDatabase.GetBlocks())
        {
            EditorInventoryButton newButton = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity, inventoryParent).GetComponent<EditorInventoryButton>();
            newButton.SetBlock(b);
        }
    }
}
