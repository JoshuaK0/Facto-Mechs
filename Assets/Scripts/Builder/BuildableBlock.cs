using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableBlock : MonoBehaviour
{
    [SerializeField] GameObject blueprintObject;
    [SerializeField] GameObject buildObject;
    [SerializeField] GameObject gameReadyObject;

    public int blockIndex;
    public void BlueprintMode()
    {
        blueprintObject.SetActive(true);
        buildObject.SetActive(false);
        gameObject.layer = LayerMask.NameToLayer("Blueprint");
    }
    public void BuiltMode()
    {
        blueprintObject.SetActive(false);
        buildObject.SetActive(true);
        gameObject.layer = LayerMask.NameToLayer("Buildable");
    }

    public void GameReadyMode()
    {
        gameReadyObject.SetActive(true);
        gameReadyObject.transform.parent = transform.parent;
        Destroy(gameObject);
    }

    public void BlockDestroy()
    {
        Destroy(gameObject);
    }
}
