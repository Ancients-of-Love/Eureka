using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleSwitchScript : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Sprite OnSprite;

    [SerializeField]
    private Sprite OffSprite;

    private Image Image;

    public bool Toggle = true;

    public event Action<ToggleSwitchScript> ToggleOn, ToggleOff;

    private void Awake()
    {
        Image = GetComponent<Image>();

        Image.sprite = OnSprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Toggle = !Toggle;
        if (Toggle)
        {
            Image.sprite = OnSprite;
            ToggleOn?.Invoke(this);
        }
        else
        {
            Image.sprite = OffSprite;
            ToggleOff?.Invoke(this);
        }
    }
}