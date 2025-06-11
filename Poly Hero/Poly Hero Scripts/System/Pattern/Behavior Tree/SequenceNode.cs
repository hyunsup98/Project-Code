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

    //SequenceNode�� �ڽ� ��带 ���ʿ��� ���������� �����ϸ鼭 Failure ���°� ���� �� ���� �����Ѵ�.
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
