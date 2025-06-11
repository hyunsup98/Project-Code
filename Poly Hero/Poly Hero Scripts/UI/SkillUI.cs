using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private GameObject skillInfo;

    private void OnEnable()
    {
        GameManager.Instance.SetGameState(gameObject, true);
    }
    private void OnDisable()
    {
        GameManager.Instance.SetGameState(gameObject, false);

        if(skillInfo.activeSelf)
        {
            skillInfo.SetActive(false);
        }
    }
}
