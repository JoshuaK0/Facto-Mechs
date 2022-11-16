using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour
{
    EditorBlock editorBlock;
    Transform blockBeingPlaced;
    [SerializeField] LayerMask buildMask;
    [SerializeField] Transform blockParent;
    [SerializeField] EditorBlockDatabase database;

    [SerializeField]
    Vector3 blockRot;

    Vector3 placePos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.mouseScrollDelta.y != 0)
        {
            blockRot += Vector3.up * Input.mouseScrollDelta.y * 90f;
        }

        if(blockBeingPlaced == null)
        {
            return;
        }
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, buildMask))
        {
            placePos = hit.transform.position + hit.normal;
            blockBeingPlaced.gameObject.SetActive(true);
            blockBeingPlaced.position = placePos;
            blockBeingPlaced.localEulerAngles = blockRot;
            if(Input.GetMouseButtonDown(1))
            {
                PlaceObject(blockBeingPlaced.gameObject);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                BuildableBlock hitBlock = hit.transform.GetComponent<BuildableBlock>();
                if (hitBlock != null)
                {
                    hitBlock.BlockDestroy();
                }
            }
        }
        else
        {
            blockBeingPlaced.gameObject.SetActive(false);
        }
    }

    void PlaceObject(GameObject block)
    {
        GameObject newBlock = Instantiate(block, placePos, Quaternion.Euler(blockRot), blockParent);
        newBlock.GetComponent<BuildableBlock>().blockIndex = database.GetAvailableBlocks().IndexOf(editorBlock);
        newBlock.GetComponent<BuildableBlock>().BuiltMode();
    }

    public void SetEditingBlock(EditorBlock block)
    {
        editorBlock = block;
        if(blockBeingPlaced != null)
        {
            Destroy(blockBeingPlaced.gameObject);
        }
        
        blockBeingPlaced = Instantiate(block.prefab, Vector3.zero, Quaternion.Euler(blockRot)).transform;
        blockBeingPlaced.GetComponent<BuildableBlock>().BlueprintMode();
    }

    public void SetBlockParent(Transform newParent)
    {
        blockParent = newParent;
    }
}
