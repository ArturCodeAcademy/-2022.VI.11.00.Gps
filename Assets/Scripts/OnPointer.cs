using UnityEngine;
using UnityEngine.EventSystems;

public class OnPointer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        InputManager.Instance.SetCanConfirm(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InputManager.Instance.SetCanConfirm(true);
    }
}
