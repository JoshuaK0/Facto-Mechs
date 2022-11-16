using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTickManager : MonoBehaviour
{
    [SerializeField]
    float tickRate;
    float lastTick;

    [SerializeField]
    bool autoStart;
    [SerializeField]
    bool isStarted = false;
    [SerializeField]
    bool isStarting = false;

    static BlockTickManager instance;

    public delegate void InitTickAction();
    public delegate void PrimaryTickAction();
    public delegate void SecondaryTickAction();
    public static event InitTickAction InitTick;
    public static event PrimaryTickAction PrimaryTick;
    public static event SecondaryTickAction SecondaryTick;

    public static BlockTickManager Instance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (autoStart)
        {
            isStarting = true;
        }
    }

    void Update()
    {
        
        if (isStarting)
        {
            isStarted = true;
            isStarting = false;

            BuildableBlock[] blocks = FindObjectsOfType<BuildableBlock>();
            foreach(BuildableBlock block in blocks)
            {
                block.GameReadyMode();
            }

            InitTick();
        }

        if(!isStarted)
        {
            return;
        }

        if (Time.time >= lastTick + tickRate)
        {
            lastTick = Time.time;
            if(PrimaryTick != null)
            {
                PrimaryTick();
            }
            if(SecondaryTick != null)
            {
                SecondaryTick();
            }
        }
    }
}
