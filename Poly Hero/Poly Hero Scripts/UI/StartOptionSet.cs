using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartOptionSet : MonoBehaviour
{
    [SerializeField] private Option option;     //저장된 옵션 값이 있다면 실행하자마자 옵션 값으로 변경 시켜주기 위해서 Option 스크립트를 받아옴

    // Start is called before the first frame update
    void Start()
    {
        option.OptionSetting();
    }
}
