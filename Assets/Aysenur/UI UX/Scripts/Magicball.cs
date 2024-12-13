using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Serialization;


public class Magicball : MonoBehaviour
    {
        [SerializeField] private Image uiFillImage;
        [SerializeField] private Text UiText;
        [SerializeField] private float cooldown;

        public int Duration
        {
            get;
            private set;
        }

        private int remainingDuration;
        private void Awake()
        {
            ResetTimer();
        }

        private void ResetTimer()
        {
            UiText.text = "00:00";
            uiFillImage.fillAmount = 0f;
            Duration = remainingDuration = 0;
        }

        public Magicball SetDuration(int seconds)
        {
            Duration = remainingDuration = seconds;
            return this;
        }

        public void Begin()
        {
            StopAllCoroutines();
            StartCoroutine(UpdateTimer());
        }
        private IEnumerator UpdateTimer()
        {
                while (remainingDuration > 0)
                {
                    UpdateUI(remainingDuration);
                    remainingDuration--;
                    yield return new WaitForSeconds(1f);
                }
                End();
        }
        private void UpdateUI(int seconds)
            {
                UiText.text=string.Format("{0:D2}:{1:D2}",seconds/60,seconds%60);
                uiFillImage.fillAmount = Mathf.InverseLerp(0, Duration, seconds);
            }

        private void End()
        {
            ResetTimer();
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
        //private void Start()
       //    {
       //        SkillImage.fillAmount = 2;
       //    }

       //    private void Update()
       //    {
       //        throw new NotImplementedException();
       //    }
    }


