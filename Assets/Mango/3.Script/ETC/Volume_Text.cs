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
        value.gameObject.SetActive(false); // 처음에는 비활성화
    }

    private void Update()
    {
        if (value.gameObject.activeSelf) // 활성화된 경우에만 업데이트
        {
            value.text = Mathf.Floor(slider.value * 100f).ToString();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        value.gameObject.SetActive(true); // 핸들을 클릭했을 때 활성화
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        value.gameObject.SetActive(false); // 핸들을 놓았을 때 비활성화
    }
}
