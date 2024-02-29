using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteracable
{
    bool CanUse
    {
        get;
    }

    void Use();
}
