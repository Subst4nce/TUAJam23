using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static  class PhysicsUtils
{
    public static bool DoesMaskContainsLayer(this LayerMask layermask, int layer)
    {
        return layermask == (layermask | (1 << layer));
    }

}
