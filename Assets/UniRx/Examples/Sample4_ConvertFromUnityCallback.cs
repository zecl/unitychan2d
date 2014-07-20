﻿using UnityEngine;

namespace UniRx.Examples
{
    public class Sample4_ConvertFromUnityCallback : TypedMonoBehaviour
    {
        private class LogCallback
        {
            public string Condition;
            public string StackTrace;
            public UnityEngine.LogType LogType;
        }

        private static class LogHelper
        {
            static Subject<LogCallback> subject;

            public static IObservable<LogCallback> LogCallbackAsObservable()
            {
                if (subject == null)
                {
                    subject = new Subject<LogCallback>();

                    // Publish to Subject in callback
                    UnityEngine.Application.RegisterLogCallback((condition, stackTrace, type) =>
                    {
                        subject.OnNext(new LogCallback { Condition = condition, StackTrace = stackTrace, LogType = type });
                    });
                }

                return subject.AsObservable();
            }
        }

        public override void Awake()
        {
            // method is separatable and composable
            LogHelper.LogCallbackAsObservable()
                .Where(x => x.LogType == LogType.Warning)
                .Subscribe(x => Debug.Log(x));

            LogHelper.LogCallbackAsObservable()
                .Where(x => x.LogType == LogType.Error)
                .Subscribe(x => Debug.Log(x));
        }
    }
}