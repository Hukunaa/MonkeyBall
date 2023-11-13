using UnityEngine;

namespace GeneralScriptableObjects
{
    public abstract class DescriptionBaseSO : SerializableScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string developerDescription = "";
#endif
    }
}