using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : INode
{
    List<INode> childs;

    public SelectorNode(List<INode> childs)
    {
        this.childs = childs;
    }

    //Selector�� �ϳ��� �׼� ��带 �����Ͽ� �����ϱ� ������ Running/Success�� ��� �ش� ��带 ���� �� ����, Failure�� ��� ���� �ڽ� ���� �̵�
    public INode.ENodeState Evaluate()
    {
        if (childs == null)
            return INode.ENodeState.ENS_Failure;

        foreach(var child in childs)
        {
            switch(child.Evaluate())
            {
                case INode.ENodeState.ENS_Running:
                    return INode.ENodeState.ENS_Running;
                case INode.ENodeState.ENS_Success:
                    return INode.ENodeState.ENS_Success;
            }
        }

        return INode.ENodeState.ENS_Failure;
    }
}
