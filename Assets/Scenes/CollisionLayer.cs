using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollisionLayer 
{
    Default = 1 <<0,
    Wall = 1 <<6,
    Enemy = 1 << 7
}
