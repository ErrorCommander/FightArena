using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] private List<ButtonAction> _actionsList;

    private void Awake()
    {
        foreach (var ba in _actionsList)
            ba.Button.onClick.AddListener(ba.OnClickButton.Invoke);
    }

    [System.Serializable]
    private class ButtonAction
    {
        public Button Button;
        public UnityEvent OnClickButton;
    }
}
