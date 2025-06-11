using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//이 인터페이스를 상속받으면 대미지를 입는 오브젝트임
public interface IDamageable
{
    public void Damage(float damage, Entity attacker, DamageType type);
}

//이 인터페이스를 상속받으면 사용 가능한 아이템임(ex. 포션 등등)
public interface IUseable
{
    public void Use(Entity entity, Slot slot);
}

//이 인터페이스를 상속받으면 강화 가능한 아이템임
public interface IForce
{
    public void Force();
}