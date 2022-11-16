using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankManager : MonoBehaviour
{
    [SerializeField]
    Transform blockParent;

    public Transform GetBlockParent()
    {
        return blockParent; 
    }
}
