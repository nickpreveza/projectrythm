using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class KinectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] bool overrideGlobalSettings;
    [SerializeField] float rotationSpeed;
    [SerializeField] RectTransform targetGraphic;
    [SerializeField] bool isHighlighted;
    Animator animator;
    [SerializeField] bool shouldPulsate;
    Button button;
    TextMeshProUGUI buttonText;
    Color startingTextColor;
    Color startingImageColor;
    private void Start()
    {
        animator = GetComponent<Animator>();
        button = GetComponent<Button>();
        buttonText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        startingTextColor = buttonText.color;
        startingImageColor = targetGraphic.GetComponent<Image>().color;

        if (!overrideGlobalSettings)
        rotationSpeed = GameManager.Instance.visuals.buttonRotationSpeed;
    }

    void ChangeInteractivity(bool newState)
    {
        button.interactable = newState;

        if (button.interactable)
        {
            buttonText.color = startingTextColor;
            targetGraphic.GetComponent<Image>().color= startingImageColor;
        }
        else
        {
            buttonText.color = button.colors.disabledColor;
            targetGraphic.GetComponent<Image>().color = button.colors.disabledColor;
        }
    }
    void Update()
    {
        if (isHighlighted)
        {
            targetGraphic.RotateAround(targetGraphic.position, Vector3.forward, 1 * rotationSpeed);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        SelectWithKinect();
    }

    public void SelectWithKinect()
    {
        isHighlighted = true;

        if (shouldPulsate)
        {
            animator.SetBool("isHighlighted", true);
        }
    }

    public void DeselectWithKinect()
    {

        isHighlighted = false;

        if (shouldPulsate)
        {
            animator.SetBool("isHighlighted", false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DeselectWithKinect();
    }
}
