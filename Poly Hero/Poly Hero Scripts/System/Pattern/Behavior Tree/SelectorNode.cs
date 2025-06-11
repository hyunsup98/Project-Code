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

    //Selector는 하나의 액션 노드를 선택하여 실행하기 때문에 Running/Success일 경우 해당 노드를 실행 후 종료, Failure일 경우 다음 자식 노드로 이동
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
