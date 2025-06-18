using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace UnityEngine.XR.Content.Interaction
{
    public class XRPushButtonRayCompatible : UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable
    {
        [Serializable]
        public class ValueChangeEvent : UnityEvent<float> { }

        [SerializeField]
        Transform m_Button;

        [SerializeField]
        float m_PressDistance = 0.1f;

        [SerializeField]
        bool m_ToggleButton = false;

        [SerializeField]
        UnityEvent m_OnPress;

        [SerializeField]
        UnityEvent m_OnRelease;

        [SerializeField]
        ValueChangeEvent m_OnValueChange;

        bool m_Toggled = false;
        bool m_Pressed = false;

        Vector3 m_InitialLocalPosition;

        protected override void Awake()
        {
            base.Awake();
            if (m_Button != null)
                m_InitialLocalPosition = m_Button.localPosition;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            selectEntered.AddListener(OnSelectEntered);
            selectExited.AddListener(OnSelectExited);
            UpdateButtonVisual();
        }

        protected override void OnDisable()
        {
            selectEntered.RemoveListener(OnSelectEntered);
            selectExited.RemoveListener(OnSelectExited);
            base.OnDisable();
        }

        void OnSelectEntered(SelectEnterEventArgs args)
        {
            if (m_ToggleButton)
            {
                m_Toggled = !m_Toggled;
                if (m_Toggled)
                    m_OnPress.Invoke();
                else
                    m_OnRelease.Invoke();
            }
            else
            {
                m_Pressed = true;
                m_OnPress.Invoke();
            }

            m_OnValueChange.Invoke(m_ToggleButton ? (m_Toggled ? 1f : 0f) : 1f);
            UpdateButtonVisual();
        }

        void OnSelectExited(SelectExitEventArgs args)
        {
            if (!m_ToggleButton && m_Pressed)
            {
                m_Pressed = false;
                m_OnRelease.Invoke();
                m_OnValueChange.Invoke(0f);
                UpdateButtonVisual();
            }
        }

        void UpdateButtonVisual()
        {
            if (m_Button == null) return;

            var offset = (m_ToggleButton ? (m_Toggled ? -m_PressDistance : 0f) :
                         (m_Pressed ? -m_PressDistance : 0f));

            var newPos = m_InitialLocalPosition;
            newPos.y += offset;
            m_Button.localPosition = newPos;
        }
    }
}
