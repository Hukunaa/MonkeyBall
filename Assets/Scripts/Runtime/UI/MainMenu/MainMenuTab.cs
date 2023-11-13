using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI.MainMenuUI
{
    public class MainMenuTab : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private CanvasGroup _canvasGroup;
        
        public void ShowTab()
        {
            gameObject.SetActive(true);
        }
        
        public void HideTab()
        {
            gameObject.SetActive(false);
        }
    }
}