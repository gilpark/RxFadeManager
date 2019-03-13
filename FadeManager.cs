using System;
using DevToolKit.Utilities;
using UniRx;
using UnityEngine;

namespace DevToolKit.FadeManager
{
    public static class FadeManager
    {
        public static void EnableCanvasGroup(this CanvasGroup c, bool setoOne, bool interactable = true, bool blocksRaycasts = true)
        {
            c.gameObject.SetActive(true);
            c.interactable = interactable;
            c.blocksRaycasts = blocksRaycasts;
//            if (setoOne) c.alpha = 1f;
            c.alpha = setoOne? 1f : 0f;
        }

        public static void DisableCanvasGroup(this CanvasGroup c, bool setoZero, bool objActive = false, bool interactable = false, bool blocksRaycasts = false)
        {
            c.gameObject.SetActive(objActive);
            c.interactable = interactable;
            c.blocksRaycasts = blocksRaycasts;
            //if (setoZero) c.alpha = 0f;
            c.alpha = setoZero? 0f : 1f;
        }

        public static IObservable<float> FadeIn(this CanvasGroup c, float duration, bool interactable = true, bool keepOn = true)
        {
            var timer = 0f;
            c.gameObject.SetActive(true);
            c.alpha = 0;
            var o = Observable.EveryUpdate()
                .Select(_ => timer += Time.deltaTime)
                .TakeWhile(x => x < duration)
                .Select(x => x.MapValue(0, duration, 0, 1))
                .Do(x => c.alpha = x)
                .DoOnCompleted(() =>
                {
                    c.alpha = 1;
                    c.interactable = interactable;
                    c.blocksRaycasts = interactable;
                    c.gameObject.SetActive(keepOn);
                });
            return o;
        }

        public static IObservable<float> FadeOut(this CanvasGroup c, float duration, bool interactable = false, bool keepOn = false)
        {
            var timer = 0f;
            c.gameObject.SetActive(true);
            c.alpha = 1;
            var o = Observable.EveryUpdate()
                .Select(_ => timer += Time.deltaTime)
                .TakeWhile(x => x < duration)
                .Select(x => x.MapValue(0, duration, 1, 0))
                .Do(x => c.alpha = x)
                .DoOnCompleted(() =>
                {
                    c.alpha = 0;
                    c.interactable = interactable;
                    c.blocksRaycasts = interactable;
                    c.gameObject.SetActive(keepOn);
                });
            return o;
        }

        public static IObservable<float> UnitTimer(float duration, bool increment)
        {
            var start = 0;
            var end = 1;
            if (!increment)
            {
                
            }
            var timer = 0f;
            var o = Observable.EveryUpdate()
                .Select(_ => timer += Time.deltaTime)
                .TakeWhile(x => x < duration)
                .Select(x => x.MapValue(0, duration, 1, 0));
            return o;
        }
    }
}
