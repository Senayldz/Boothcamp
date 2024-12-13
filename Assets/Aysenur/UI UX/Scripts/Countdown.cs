using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    [SerializeField] private Image timeimage;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private float duration, currentTime;
    [SerializeField] private TMP_Text _counttext;
    private int count = 2;

    void Start()
    {
        currentTime = duration;
        timeText.text = currentTime.ToString();
        StartCoroutine(TimeIEn());
    }

    private void Update()
    {
        if (count > 0)
        {
            if (Input.GetMouseButtonDown(1))
            {
                count--;
                _counttext.text = count.ToString();
            }
        }
    }

    IEnumerator TimeIEn()
    {
        while (currentTime>=0)
        {
            timeimage.fillAmount = Mathf.InverseLerp(0, duration, currentTime);
            timeText.text = currentTime.ToString();
            yield return new WaitForSeconds(1f);
            currentTime--;
            
        }
        OpenPanel();
    }

    void OpenPanel()
    {
        timeText.text = "";
    }
}
