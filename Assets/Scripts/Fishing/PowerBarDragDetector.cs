using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PowerBarDragDetector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public FishingCastController castController;

    public void OnPointerDown(PointerEventData eventData)
    {
        castController.isDraggingPowerBar = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        castController.isDraggingPowerBar = false;
        castController.OnPowerBarReleased(); // Trigger cast if appropriate
    }
}
