using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Volume_Text : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Text value;
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        value = GetComponentsInChildren<Text>()[1];
        value.gameObject.SetActive(false); // ó������ ��Ȱ��ȭ
    }

    private void Update()
    {
        if (value.gameObject.activeSelf) // Ȱ��ȭ�� ��쿡�� ������Ʈ
        {
            value.text = Mathf.Floor(slider.value * 100f).ToString();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        value.gameObject.SetActive(true); // �ڵ��� Ŭ������ �� Ȱ��ȭ
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        value.gameObject.SetActive(false); // �ڵ��� ������ �� ��Ȱ��ȭ
    }
}
