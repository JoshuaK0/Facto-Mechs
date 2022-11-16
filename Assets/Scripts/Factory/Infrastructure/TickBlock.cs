using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TickBlock : MonoBehaviour
{
    enum TickPriority
    {
        Primary,
        Secondary
    }


    [SerializeField]
    TickPriority priority;

    void OnEnable()
    {
        BlockTickManager.InitTick += DoInitTick;
        if(priority == TickPriority.Primary)
        {
            BlockTickManager.PrimaryTick += DoTick;
        }
        else if (priority == TickPriority.Secondary)
        {
            BlockTickManager.SecondaryTick += DoTick;
        }
    }


    void OnDisable()
    {
        BlockTickManager.InitTick -= DoInitTick;
        if (priority == TickPriority.Primary)
        {
            BlockTickManager.PrimaryTick -= DoTick;
        }
        else if (priority == TickPriority.Secondary)
        {
            BlockTickManager.SecondaryTick -= DoTick;
        }
    }

    private void OnDestroy()
    {
        BlockTickManager.InitTick -= DoInitTick;
        if (priority == TickPriority.Primary)
        {
            BlockTickManager.PrimaryTick -= DoTick;
        }
        else if (priority == TickPriority.Secondary)
        {
            BlockTickManager.SecondaryTick -= DoTick;
        }
    }

    public abstract void DoInitTick();

    public abstract void DoTick();
}
