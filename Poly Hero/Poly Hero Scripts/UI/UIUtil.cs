using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUtil : MonoBehaviour
{
    private void OnEnable()
    {
        UIManager.Instance.siblingObject = gameObject;
        GameManager.Instance.SetGameState(gameObject, true);
    }

    private void OnDisable()
    {
        if(UIManager.Instance.siblingObject == gameObject)
        {
            UIManager.Instance.siblingObject = null;
        }
        GameManager.Instance.SetGameState(gameObject, false);
    }
}
