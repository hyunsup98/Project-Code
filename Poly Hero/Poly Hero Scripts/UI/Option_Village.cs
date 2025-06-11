using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option_Village : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.Instance.SetGameState(gameObject, true);
    }

    private void OnDisable()
    {
        GameManager.Instance.SetGameState(gameObject, false);
    }
}
