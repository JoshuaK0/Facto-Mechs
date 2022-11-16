using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorInventoryButton : MonoBehaviour
{
    [SerializeField] EditorBlock editorBlock;
    [SerializeField] Text text;
    public void EditorInventoryButtonClicked()
    {
        EditorInventoryManager.Instance().SelectedBlockButton(editorBlock);
    }

    public void SetBlock(EditorBlock block)
    {
        editorBlock = block;
        text.text = block.name;
    }
}
