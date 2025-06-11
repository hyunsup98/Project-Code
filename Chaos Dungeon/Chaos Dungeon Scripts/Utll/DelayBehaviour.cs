using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


//�ð� ������ ���� ���� ���
//Delay ���� ������ ��� �̰� ��ӹ޾� ���

public class DelayBehaviour : MonoBehaviour
{
    protected Dictionary<UnityAction, float> actions = new Dictionary<UnityAction, float>();
    private List<UnityAction> removes = new List<UnityAction>();

    void Update()
    {
        foreach (UnityAction action in actions.Keys.ToList<UnityAction>())
        {
            actions[action] -= GameManager.deltaTime;
            if (actions[action] <= 0)
            {
                removes.Add(action);
            }
        }
        foreach (var action in removes)
        {
            action.Invoke();
            actions.Remove(action);
        }
        if (removes.Count > 0)
        {
            removes.Clear();
        }
        Updates();
    }

    protected virtual void Updates() { }


    //�����ð� �� �ߵ� (��������)
    // Delay(()=>{
    //   �ڵ�;
    //   �ڵ�;
    // },�ð�)
    public void Delay(UnityAction action, float time)
    {
        actions.Add(action, time);
    }
}
