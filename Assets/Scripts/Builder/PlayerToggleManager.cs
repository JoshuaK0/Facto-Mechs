using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToggleManager : MonoBehaviour
{
    public List<Behaviour> editingComponents;

    public RectTransform UIElement;

    bool isEditing = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isEditing = !isEditing;
            foreach(var comp in editingComponents)
            {
                comp.enabled = isEditing;
                UIElement.gameObject.SetActive(!isEditing);
            }
        }
    }
}
