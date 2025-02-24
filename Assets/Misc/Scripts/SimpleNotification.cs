using System;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Misc
{
    public sealed class SimpleNotification : MonoBehaviour
    {
        public enum NotificationType
        {
            Success,
            Warning,
            Error
        }

        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Image image;
        [SerializeField] private Text text;

        [SerializeField] private float toPositionY;
        [SerializeField] private Ease inEase;
        [SerializeField] private float inDuration;

        [SerializeField] private Ease outEase;
        [SerializeField] private float outDuration;
        [SerializeField] private float outDelay;

        private readonly CompositeMotionHandle _motionHandles = new();

        public void Play(NotificationType type, Action onPlayHaptics)
        {
            _motionHandles.Cancel();

            SetNotification(type);

            var height = rectTransform.sizeDelta.y;
            LMotion.Create(height, toPositionY, inDuration)
                .WithEase(inEase)
                .WithOnComplete(() =>
                {
                    onPlayHaptics();
                    LMotion.Create(toPositionY, height, inDuration)
                        .WithEase(outEase)
                        .WithDelay(outDelay)
                        .BindToAnchoredPositionY(rectTransform)
                        .AddTo(_motionHandles);
                })
                .BindToAnchoredPositionY(rectTransform)
                .AddTo(_motionHandles);
        }

        private void SetNotification(NotificationType type)
        {
            switch (type)
            {
                case NotificationType.Success:
                    image.color = new Color32(64, 255, 64, 255);
                    text.text = "成功通知";
                    break;
                case NotificationType.Warning:
                    image.color = new Color32(255, 255, 64, 255);
                    text.text = "警告通知";
                    break;
                case NotificationType.Error:
                    image.color = new Color32(255, 64, 64, 255);
                    text.text = "エラー通知";
                    break;
            }
        }
    }
}
