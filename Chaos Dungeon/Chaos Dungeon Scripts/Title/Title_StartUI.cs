using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class AbilityInfos
{
    public AbilityInfo left;
    public AbilityInfo right;
}
[System.Serializable]
public class AbilityInfo
{
    public Sprite img;
    public PassiveSkill ps;
    public string name;
    public string lore;
}

public class Title_StartUI : MonoBehaviour
{
    [SerializeField] private Title_Player player;
    [SerializeField] private List<AbilityInfos> abilities;
    [SerializeField] private Title_AbllityText ability_Left;
    [SerializeField] private Title_AbllityText ability_Right;

    [SerializeField] private Canvas UI;

    private void Start()
    {
        Button_PlayerSetting(0);
    }

    AbilityInfos info;
    string strSkin = string.Empty;
    public void Button_PlayerSetting(int i)
    {
        player.mySkin += i;

        if ((int)player.mySkin > 3)
            player.mySkin = 0;
        if ((int)player.mySkin < 0)
            player.mySkin = PlayerSkin.magic;

        
        if((int)player.mySkin == 1)
             strSkin = PlayerSkin.sword.ToString();
        else
            strSkin = player.mySkin.ToString();

        Skeleton sk = player.skeleton;
        sk.Skin = sk.Data.FindSkin(strSkin);
        sk.SetSlotsToSetupPose();

        info = abilities[(int)player.mySkin];
        ability_Left.RepInfo(info.left);
        ability_Right.RepInfo(info.right);
    }


    public void Button_Start()
    {
        if (GameManager.GetPlayer() == null)
        {
            GameManager.Instance.player = Instantiate(GameManager.Instance.playerPrefab);
            GameManager.Instance.player.name = "Player";
        }
        else
        {
            GameManager.GetPlayer().Reset();
        }
        GameManager.GetPlayer().ChangeSkin(strSkin);

        PassiveSkill pSkill;
        if (ability_Left.GetComponent<Toggle>().isOn)
            pSkill = info.left.ps;
        else
            pSkill = info.right.ps;
        GameManager.GetPlayer().playerPassive = pSkill;
        GameManager.GetPlayer().PassiveType = pSkill.type;

        UI.gameObject.SetActive(false);
        GameManager.SetUI(true);
        SceneManager.LoadScene(1);
    }
}
