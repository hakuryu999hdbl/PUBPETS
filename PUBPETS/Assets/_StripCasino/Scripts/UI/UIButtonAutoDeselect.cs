using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class UIButtonAutoDeselect : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(DeselectNextFrame());
    }

    IEnumerator DeselectNextFrame()
    {
        yield return null; // 等一帧，避免UI内部逻辑覆盖
        EventSystem.current.SetSelectedGameObject(null);
    }
}