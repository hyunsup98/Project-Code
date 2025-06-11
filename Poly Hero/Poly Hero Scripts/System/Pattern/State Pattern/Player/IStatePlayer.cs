using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    void Handle(Animator playerAnim);
}

//�÷��̾ �Ǽ��� ����
public class PlayerHand : IPlayerState
{
    public void Handle(Animator playerAnim)
    {
        playerAnim.SetTrigger("handatk");
    }
}

//�÷��̾ ������ �� ����
public class PlayerLogging : IPlayerState
{
    public void Handle(Animator playerAnim)
    {
        playerAnim.SetTrigger("logging");
    }
}

//�÷��̾ ��̸� �� ����
public class PlayerMining : IPlayerState
{
    public void Handle(Animator playerAnim)
    {
        playerAnim.SetTrigger("mining");
    }
}

//�÷��̾ ���⸦ �� ����
public class PlayerWeaponAttack : IPlayerState
{
    public void Handle(Animator playerAnim)
    {
        playerAnim.SetTrigger("weaponatk");
    }
}