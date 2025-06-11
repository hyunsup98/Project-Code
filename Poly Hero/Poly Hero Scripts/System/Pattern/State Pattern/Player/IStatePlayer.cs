using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    void Handle(Animator playerAnim);
}

//플레이어가 맨손인 상태
public class PlayerHand : IPlayerState
{
    public void Handle(Animator playerAnim)
    {
        playerAnim.SetTrigger("handatk");
    }
}

//플레이어가 도끼를 든 상태
public class PlayerLogging : IPlayerState
{
    public void Handle(Animator playerAnim)
    {
        playerAnim.SetTrigger("logging");
    }
}

//플레이어가 곡괭이를 든 상태
public class PlayerMining : IPlayerState
{
    public void Handle(Animator playerAnim)
    {
        playerAnim.SetTrigger("mining");
    }
}

//플레이어가 무기를 든 상태
public class PlayerWeaponAttack : IPlayerState
{
    public void Handle(Animator playerAnim)
    {
        playerAnim.SetTrigger("weaponatk");
    }
}