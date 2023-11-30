using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _arrowIcon;

    private void OnEnable()
    {
        _arrowIcon.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _arrowIcon.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _arrowIcon.SetActive(false);
    }
}
