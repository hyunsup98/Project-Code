using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartOptionSet : MonoBehaviour
{
    [SerializeField] private Option option;     //����� �ɼ� ���� �ִٸ� �������ڸ��� �ɼ� ������ ���� �����ֱ� ���ؼ� Option ��ũ��Ʈ�� �޾ƿ�

    // Start is called before the first frame update
    void Start()
    {
        option.OptionSetting();
    }
}
