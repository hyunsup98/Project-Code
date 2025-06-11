using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PassiveSkill : MonoBehaviour
{
    public ItemType type;

    public abstract void PlayerPassive();
}
