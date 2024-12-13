using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DecraseOpasity : MonoBehaviour
{
    [SerializeField] private Image timeimage;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text _counttext;
    private int count = 2;
    private int timer = 10;
    private bool opacityDecreased = false;

    private Color originalColor;
    private Color targetColor = new Color(1f, 1f, 1f, 0.1f);
    private float opacityDecreaseDuration = 0.5f;
    private float opacityIncreaseDelay = 10f;

    private void Start()
    {
        originalColor = timeimage.color;
        timeText.text = "";
        _counttext.text = count.ToString();
    }

    private void Update()
    {
        if (count > 0)
        {
            if (Input.GetMouseButtonDown(1))
            {
                count--;
                _counttext.text = count.ToString();

                if (count == 0 && !opacityDecreased)
                {
                    StartCoroutine(DecreaseOpacityRoutine());
                    StartCoroutine(CountdownRoutine()); 
                    
                }
                
            }
        }
    }

    private IEnumerator DecreaseOpacityRoutine()
    {
        opacityDecreased = true;

        float elapsed = 0f;
        while (elapsed < opacityDecreaseDuration)
        {
            float t = elapsed / opacityDecreaseDuration;
            timeimage.color = Color.Lerp(originalColor, targetColor, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(opacityIncreaseDelay);
        

        elapsed = 0f;
        while (elapsed < opacityDecreaseDuration)
        {
            float t = elapsed / opacityDecreaseDuration;
            timeimage.color = Color.Lerp(targetColor, originalColor, t);
            elapsed += Time.deltaTime;
            yield return null;
            
        }

        timeimage.color = originalColor; // Opaklık %100'e geri dön
        opacityDecreased = false;

        count = 2; // Count değerini tekrar 2'ye ayarla
        timer = 10;
        _counttext.text = count.ToString();
        
    
    }

    private IEnumerator CountdownRoutine()
    {
        while (timer > 0)
        {
            timeText.text = timer.ToString();
            yield return new WaitForSeconds(1f);
            timer--;
        }

        timeText.text = "";
    }
    
}
    

