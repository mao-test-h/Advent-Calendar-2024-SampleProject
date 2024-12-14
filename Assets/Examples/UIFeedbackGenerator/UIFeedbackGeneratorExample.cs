using Examples.UIFeedbackGenerator.Plugins.iOS;
using LightScrollSnap;
using Misc;
using UnityEngine;
using UnityEngine.UI;

namespace Examples.UIFeedbackGenerator
{
    internal sealed class UIFeedbackGeneratorExample : MonoBehaviour
    {
        [Header("Notification")]
        [SerializeField] private Button notificationPrepareButton;
        [SerializeField] private Button notificationSuccessButton;
        [SerializeField] private Button notificationWarningButton;
        [SerializeField] private Button notificationErrorButton;

        [Header("Notification - Example")]
        [SerializeField] private SimpleNotification simpleNotification;
        [SerializeField] private Button showSuccessButton;
        [SerializeField] private Button showWarningButton;
        [SerializeField] private Button showErrorButton;

        [Header("Selection")]
        [SerializeField] private Button selectionPrepareButton;
        [SerializeField] private Button selectionButton;

        [Header("Selection - Example")]
        [SerializeField] private ScrollSnap selectionScrollSnap;
        [SerializeField] private RectTransform selectionScrollNode;
        [SerializeField] private GameObject selectionScrollPrefab;
        [SerializeField] private int selectionScrollCount = 32;

        [Header("Impact")]
        [SerializeField] private Button impactPrepareButton;
        [SerializeField] private Button impactOccurredButton;
        [SerializeField] private Toggle impactLightToggle;
        [SerializeField] private Toggle impactMediumToggle;
        [SerializeField] private Toggle impactHeavyToggle;
        [SerializeField] private Toggle impactSoftToggle;
        [SerializeField] private Toggle impactRigidToggle;
        [SerializeField] private SliderAndLabel impactIntensitySlider;

        [Header("Impact - Example")]
        [SerializeField] private Button impactSoftSAmple;
        [SerializeField] private Button impactRigidSample;

        private UINotificationFeedbackGenerator _notificationFeedbackGenerator;
        private UISelectionFeedbackGenerator _selectionFeedbackGenerator;
        private UIImpactFeedbackGenerator _impactFeedbackGenerator;

        private void Start()
        {
            Application.targetFrameRate = 60;

#if UNITY_IOS && !UNITY_EDITOR
            _selectionFeedbackGenerator = new UISelectionFeedbackGenerator();
            _notificationFeedbackGenerator = new UINotificationFeedbackGenerator();
            _impactFeedbackGenerator = new UIImpactFeedbackGenerator(UIImpactFeedbackGenerator.FeedbackStyle.Light);
#endif

            SetEventNotificationFeedbackGenerator();
            SetEventSelectionFeedbackGenerator();
            SetEventImpactFeedbackGenerator();
        }

        private void OnDestroy()
        {
            _selectionFeedbackGenerator?.Dispose();
            _notificationFeedbackGenerator?.Dispose();
            _impactFeedbackGenerator?.Dispose();
        }

        // UINotificationFeedbackGenerator
        private void SetEventNotificationFeedbackGenerator()
        {
            notificationPrepareButton.onClick.AddListener(() =>
            {
                _notificationFeedbackGenerator?.Prepare();
            });

            notificationSuccessButton.onClick.AddListener(() =>
            {
                var type = UINotificationFeedbackGenerator.FeedbackType.Success;
                _notificationFeedbackGenerator?.NotificationOccurred(type);
            });

            notificationWarningButton.onClick.AddListener(() =>
            {
                var type = UINotificationFeedbackGenerator.FeedbackType.Warning;
                _notificationFeedbackGenerator?.NotificationOccurred(type);
            });

            notificationErrorButton.onClick.AddListener(() =>
            {
                var type = UINotificationFeedbackGenerator.FeedbackType.Error;
                _notificationFeedbackGenerator?.NotificationOccurred(type);
            });

            // Notification Example
            showSuccessButton.onClick.AddListener(() =>
            {
                simpleNotification.Play(SimpleNotification.NotificationType.Success, () =>
                {
                    var type = UINotificationFeedbackGenerator.FeedbackType.Success;
                    _notificationFeedbackGenerator?.NotificationOccurred(type);
                });
            });

            showWarningButton.onClick.AddListener(() =>
            {
                simpleNotification.Play(SimpleNotification.NotificationType.Warning, () =>
                {
                    var type = UINotificationFeedbackGenerator.FeedbackType.Warning;
                    _notificationFeedbackGenerator?.NotificationOccurred(type);
                });
            });

            showErrorButton.onClick.AddListener(() =>
            {
                simpleNotification.Play(SimpleNotification.NotificationType.Error, () =>
                {
                    var type = UINotificationFeedbackGenerator.FeedbackType.Error;
                    _notificationFeedbackGenerator?.NotificationOccurred(type);
                });
            });
        }

        // UISelectionFeedbackGenerator
        private void SetEventSelectionFeedbackGenerator()
        {
            selectionPrepareButton.onClick.AddListener(() =>
            {
                _selectionFeedbackGenerator?.Prepare();
            });

            selectionButton.onClick.AddListener(() =>
            {
                _selectionFeedbackGenerator?.SelectionChanged();
            });

            // ScrollRect Example
            for (int i = 0; i < selectionScrollCount; i++)
            {
                var go = Instantiate(selectionScrollPrefab, selectionScrollNode);
                go.SetActive(true);
                var text = go.GetComponentInChildren<Text>();
                text.text = $"{i + 1}";
            }

            selectionScrollSnap.OnItemSelected.AddListener((_, _) =>
            {
                _selectionFeedbackGenerator?.SelectionChanged();
            });
        }

        // UIImpactFeedbackGenerator
        private void SetEventImpactFeedbackGenerator()
        {
            impactPrepareButton.onClick.AddListener(() =>
            {
                _impactFeedbackGenerator?.Prepare();
            });

            impactOccurredButton.onClick.AddListener(() =>
            {
                _impactFeedbackGenerator?.ImpactOccurred(impactIntensitySlider.Value);
            });

            void CreateImpactGenerator(UIImpactFeedbackGenerator.FeedbackStyle feedbackStyle, bool isOn)
            {
                if (isOn == false)
                {
                    return;
                }

                _impactFeedbackGenerator?.Dispose();
#if UNITY_IOS && !UNITY_EDITOR
                _impactFeedbackGenerator = new UIImpactFeedbackGenerator(feedbackStyle);
#endif
            }

            impactLightToggle.onValueChanged.AddListener(isOn =>
            {
                CreateImpactGenerator(UIImpactFeedbackGenerator.FeedbackStyle.Light, isOn);
            });

            impactMediumToggle.onValueChanged.AddListener(isOn =>
            {
                CreateImpactGenerator(UIImpactFeedbackGenerator.FeedbackStyle.Medium, isOn);
            });

            impactHeavyToggle.onValueChanged.AddListener(isOn =>
            {
                CreateImpactGenerator(UIImpactFeedbackGenerator.FeedbackStyle.Heavy, isOn);
            });

            impactSoftToggle.onValueChanged.AddListener(isOn =>
            {
                CreateImpactGenerator(UIImpactFeedbackGenerator.FeedbackStyle.Soft, isOn);
            });

            impactRigidToggle.onValueChanged.AddListener(isOn =>
            {
                CreateImpactGenerator(UIImpactFeedbackGenerator.FeedbackStyle.Rigid, isOn);
            });

            // Impact Example
            impactSoftSAmple.onClick.AddListener(() =>
            {
#if UNITY_EDITOR || !UNITY_IOS
                return;
#else
                using var generator = new UIImpactFeedbackGenerator(UIImpactFeedbackGenerator.FeedbackStyle.Soft);
                generator.ImpactOccurred();
#endif
            });

            impactRigidSample.onClick.AddListener(() =>
            {
#if UNITY_EDITOR || !UNITY_IOS
                return;
#else
                using var generator = new UIImpactFeedbackGenerator(UIImpactFeedbackGenerator.FeedbackStyle.Rigid);
                generator.ImpactOccurred();
#endif
            });
        }
    }
}
