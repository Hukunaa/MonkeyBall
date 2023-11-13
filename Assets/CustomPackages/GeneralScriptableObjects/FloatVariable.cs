using UnityEngine;

namespace GeneralScriptableObjects
{
    [CreateAssetMenu(fileName = "Float Variable", menuName = "ScriptableObjects/Variables/Float")]
    public class FloatVariable : DescriptionBaseSO
    {
        public float Value;

        public void SetValue(float value)
        {
            Value = value;
        }

        public void SetValue(FloatVariable value)
        {
            Value = value.Value;
        }

        public void ApplyChange(float amount)
        {
            Value += amount;
        }

        public void ApplyChange(FloatVariable amount)
        {
            Value += amount.Value;
        }
    }
}