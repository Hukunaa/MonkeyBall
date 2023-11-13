using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.UI.MainMenuUI
{
    [RequireComponent(typeof(UIDocument))]
    public abstract class UIToolkitMonoBehavior : MonoBehaviour
    {
        protected UIDocument UIDocument;
        protected VisualElement Root;

        protected virtual void Awake()
        {
            UIDocument = GetComponent<UIDocument>();
            Root = UIDocument.rootVisualElement;

            FindVisualElements();
            BindButtons();
        }

        protected abstract void FindVisualElements();

        protected abstract void BindButtons();
        
        public void DisplayUI()
        {
            Root.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
        }

        public void HideUI()
        {
            Root.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
        }
    }
}