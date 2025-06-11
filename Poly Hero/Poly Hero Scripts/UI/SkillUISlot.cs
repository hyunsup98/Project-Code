using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SkillUISlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Skill skill;
    [SerializeField] private SlotType slotType;
    [SerializeField] private Image skillImg;

    [SerializeField] private GameObject sInfoUI;    //스킬 정보 UI
    [SerializeField] private TMP_Text sName;        //스킬 이름
    [SerializeField] private TMP_Text sInfo;        //스킬 설명
    [SerializeField] private TMP_Text sLevel;       //스킬 요구 레벨

    private bool isUnlock = false;
    private RectTransform rect;

    private void Start()
    {
        CheckPlayerLevel();
        rect = sInfoUI.GetComponent<RectTransform>();
        GameManager.Instance.player.LevelUpAction += CheckPlayerLevel;
    }

    //플레이어 레벨이 스킬의 요구레벨보다 높을 경우 해금
    private void CheckPlayerLevel()
    {
        isUnlock = GameManager.Instance.player.Level >= skill.sLevel ? false : true;
        skillImg.color = isUnlock ? new Color32(70, 70, 70, 255) : Color.white;

        if(isUnlock)
        {
            skillImg.color = new Color32(70, 70, 70, 255);
        }
        else
        {
            skillImg.color = Color.white;
            GameManager.Instance.player.LevelUpAction -= CheckPlayerLevel;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && !isUnlock)
        {
            DragSkillSlot.Instance.dragslot = this;
            DragSkillSlot.Instance.SetItemData(skill.sIcon);
            DragSkillSlot.Instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (skill != null)
        {
            DragSkillSlot.Instance.transform.position = eventData.position;
        }
    }
    

    public void OnEndDrag(PointerEventData eventData)
    {
        DragSkillSlot.Instance.Setcolor(0);
        DragSkillSlot.Instance.dragslot = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(skill != null)
        {
            sInfoUI.SetActive(true);
            sName.text = skill.sName;
            sInfo.text = skill.sInfo;
            sLevel.text = $"요구 레벨: {skill.sLevel}";
            sLevel.color = isUnlock ? Color.red : Color.green;

            SetRectPos(sInfoUI.transform);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(sInfoUI.activeSelf)
        {
            sInfoUI.SetActive(false);
            sName.text = sInfo.text = sLevel.text = string.Empty;
        }
    }

    void SetRectPos(Transform obj)
    {
        float standard = Screen.width - (rect.sizeDelta.x + 50);

        if (transform.position.x < standard)
            SetAnchorPivot(1, 0, obj);
        else
            SetAnchorPivot(0, 1, obj);
    }

    //아이템 정보창 앵커, 피봇 포지션 잡아주기
    void SetAnchorPivot(float anchor, float pivot, Transform obj)
    {
        obj.SetParent(transform);

        rect.anchorMin = new Vector2(anchor, 1);
        rect.anchorMax = new Vector2(anchor, 1);
        rect.pivot = new Vector2(pivot, 1);
        rect.anchoredPosition = Vector2.zero;

        rect.SetParent(UIManager.Instance.transform);
        rect.SetAsLastSibling();
    }
}
