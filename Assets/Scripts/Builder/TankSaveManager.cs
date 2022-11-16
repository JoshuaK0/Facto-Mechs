using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class TankSaveManager : MonoBehaviour
{
    [System.Serializable]
    struct SavedBlock
    {
        public int index;
        public Vector3 position;
        public Vector3 rotation;
    }
    [SerializeField]
    Transform transformToSave;

    [SerializeField]
    List<SavedBlock> savedBlocks;

    [SerializeField]
    GameObject tankGameModePrefab;
    [SerializeField]
    GameObject tankEditModePrefab;

    [SerializeField]
    EditorBlockDatabase database;

    public bool doSave;

    List<BuildableBlock> newLoadedBlocks = new List<BuildableBlock>();

    static TankSaveManager _instance;

    static TankSaveManager Instance { get { return _instance; } }

    public void SaveTransform()
    {
        savedBlocks.Clear();
        savedBlocks = new List<SavedBlock>();
        foreach(Transform t in transformToSave)
        { 
            BuildableBlock block = t.GetComponent<BuildableBlock>();
            if (block != null)
            {
                SavedBlock savedBlock = new SavedBlock();
                savedBlock.index = block.blockIndex;
                savedBlock.position = block.transform.localPosition;
                savedBlock.rotation = block.transform.eulerAngles;
                savedBlocks.Add(savedBlock);
            }
        }
    }

    void Update()
    {
        if (doSave)
        {
            doSave = false;
            SaveTransform();
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void LoadGameModeTank()
    {
        GameObject newTank = Instantiate(tankGameModePrefab);
        transformToSave = newTank.GetComponent<TankManager>().GetBlockParent();
        TankManager tankManager = newTank.GetComponent<TankManager>();
        Transform blockParent = tankManager.GetBlockParent();

        newLoadedBlocks.Clear();
        foreach(SavedBlock block in savedBlocks)
        {
            GameObject newBlock = Instantiate(database.GetAvailableBlocks()[block.index].prefab);
            newBlock.transform.parent = blockParent;
            newBlock.transform.localPosition = block.position;
            newBlock.transform.localEulerAngles = block.rotation;
            newLoadedBlocks.Add(newBlock.GetComponent<BuildableBlock>());
        }
        ConveyorSystemManager.Instance().blockParent = tankManager.GetBlockParent();
    }

    public void LoadEditModeTank()
    {
        GameObject newTank = Instantiate(tankEditModePrefab);
        transformToSave = newTank.GetComponent<TankManager>().GetBlockParent();
        TankManager tankManager = newTank.GetComponent<TankManager>();
        Transform blockParent = tankManager.GetBlockParent();

        newLoadedBlocks.Clear();
        foreach (SavedBlock block in savedBlocks)
        {
            GameObject newBlock = Instantiate(database.GetAvailableBlocks()[block.index].prefab);
            newBlock.transform.parent = blockParent;
            newBlock.transform.localPosition = block.position;
            newBlock.transform.localEulerAngles = block.rotation;
            BuildableBlock newBlockComponent = newBlock.GetComponent<BuildableBlock>();
            newBlockComponent.blockIndex = block.index;
            newLoadedBlocks.Add(newBlockComponent);
        }
        BuildController buildController = FindObjectOfType<BuildController>();
        buildController.SetBlockParent(transformToSave);
    }

    public void TankToGameMode()
    {
        foreach(BuildableBlock b in newLoadedBlocks)
        {
            b.GameReadyMode();
        }
    }

    public void TankToBuiltMode()
    {
        foreach (BuildableBlock b in newLoadedBlocks)
        {
            b.BuiltMode();
        }
    }
}
