using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActionNode : INode
{
    Func<INode.ENodeState> onUpdate = null;

    public ActionNode(Func<INode.ENodeState> onUpdate)
    {
        this.onUpdate = onUpdate;
    }

    public INode.ENodeState Evaluate() => onUpdate?.Invoke() ?? INode.ENodeState.ENS_Failure;
}
