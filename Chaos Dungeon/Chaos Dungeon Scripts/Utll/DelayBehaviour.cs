using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


//시간 지연을 보다 쉽게 사용
//Delay 쓰고 싶으면 대신 이걸 상속받아 사용

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


    //일정시간 후 발동 (예시형식)
    // Delay(()=>{
    //   코드;
    //   코드;
    // },시간)
    public void Delay(UnityAction action, float time)
    {
        actions.Add(action, time);
    }
}
