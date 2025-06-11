using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : INode
{
    List<INode> childs;

    public SequenceNode(List<INode> childs)
    {
        this.childs = childs;
    }

    //SequenceNode는 자식 노드를 왼쪽에서 오른쪽으로 진행하면서 Failure 상태가 나올 때 까지 실행한다.
    public INode.ENodeState Evaluate()
    {
        if(childs == null || childs.Count == 0)
            return INode.ENodeState.ENS_Failure;

        foreach(var child in childs)
        {
            switch(child.Evaluate())
            {
                case INode.ENodeState.ENS_Running:
                    return INode.ENodeState.ENS_Running;
                case INode.ENodeState.ENS_Success:
                    continue;
                case INode.ENodeState.ENS_Failure:
                    return INode.ENodeState.ENS_Failure;
            }
        }

        return INode.ENodeState.ENS_Success;
    }

}
