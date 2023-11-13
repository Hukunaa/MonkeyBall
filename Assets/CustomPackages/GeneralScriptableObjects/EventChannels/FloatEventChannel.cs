using UnityEngine;
using UnityEngine.Events;

namespace GeneralScriptableObjects.EventChannels
{
	/// <summary>
	/// This class is used for Events that have one int argument.
	/// Example: An Achievement unlock event, where the int is the Achievement ID.
	/// </summary>
	[CreateAssetMenu(menuName = "ScriptableObjects/Events/Float Event Channel")]
	public class FloatEventChannel : ScriptableObject
	{
		public UnityAction<float> onEventRaised;
	
		public void RaiseEvent(float value)
		{
			onEventRaised?.Invoke(value);
		}
	}
}
