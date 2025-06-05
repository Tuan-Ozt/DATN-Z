using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
// đưa dữ liệu nhân vật ra để hiển thị
public class EquipmentSlotUI : MonoBehaviour, IPointerClickHandler
{
    public string itemId;
    public Image iconImage;

    public string itemType;

    public void SetItem(string id, Sprite icon, string type = null)
    {
        itemId = id;
        itemType = type;
        if (iconImage != null)
        {
            iconImage.sprite = icon;
            iconImage.color = Color.white;
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(itemId))
        {
            ItemDetailsPanel.Instance.Show(itemId, iconImage.sprite, itemType);
        }
    }

}
