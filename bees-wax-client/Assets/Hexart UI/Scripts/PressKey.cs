﻿using UnityEngine;
using UnityEngine.Events;

namespace Hexart_UI.Scripts
{
    public class PressKey : MonoBehaviour
    {
        [Header("KEY")]
        [SerializeField]
        public KeyCode hotkey;

        [Header("KEY ACTION")]
        [SerializeField]
        public UnityEvent pressAction;

        void Update()
        {
            if (Input.GetKeyDown(hotkey))
            {
                pressAction.Invoke();
            }
        }
    }
}