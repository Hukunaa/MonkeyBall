using UnityEngine;

namespace ScriptableObjects.DataContainer
{
    [CreateAssetMenu(fileName = "StringListVariable", menuName = "ScriptableObjects/Variables/StringListVariable", order = 0)]
    public class StringListVariable : ScriptableObject
    {
        [SerializeField] private string[] _value;

        public string[] Value => _value;
    }
}