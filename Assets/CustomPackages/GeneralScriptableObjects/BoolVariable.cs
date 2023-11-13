using UnityEngine;
using UnityEngine.Events;

namespace GeneralScriptableObjects
{
    [CreateAssetMenu(fileName = "BoolVariable", menuName = "ScriptableObjects/Variables/Bool", order = 0)]
    public class BoolVariable : ScriptableObject
    {
        public bool Value;

        public UnityAction onValueChanged;

        public void SetValue(bool value)
        {
            Value = value;
            onValueChanged?.Invoke();
        }
    }
}