using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _arrowIcon;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _arrowIcon.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _arrowIcon.SetActive(false);
    }
}
