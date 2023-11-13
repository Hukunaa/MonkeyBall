using UnityEngine;
using UnityEngine.Events;

namespace GeneralScriptableObjects.EventChannels
{
	[CreateAssetMenu(menuName = "ScriptableObjects/Events/FadeEventChannel")]
	public class FadeChannel : ScriptableObject
	{
		public UnityAction<bool, float> onEventRaised;

		/// <summary>
		/// Fade helper function to simplify usage. Fades the screen in to gameplay.
		/// </summary>
		/// <param name="duration">How long it takes to the image to fade in.</param>
		/// <param name="color">Target color for the image to reach.</param>
		public void FadeIn(float duration)
		{
			Fade(true, duration);
		}

		/// <summary>
		/// Fade helper function to simplify usage. Fades the screen out to black.
		/// </summary>
		/// <param name="duration">How long it takes to the image to fade out.</param>
		public void FadeOut(float duration)
		{
			Fade(false, duration);
		}

		/// <summary>
		/// Generic fade function. Communicates with <seealso cref="FadeController.cs"/>.
		/// </summary>
		/// <param name="fadeIn">If true, the rectangle fades in. If false, the rectangle fades out.</param>
		/// <param name="duration">How long it takes to the image to fade in/out.</param>
		/// <param name="color">Target color for the image to reach. Disregarded when fading out.</param>
		private void Fade(bool fadeIn, float duration)
		{
			if (onEventRaised != null)
				onEventRaised.Invoke(fadeIn, duration);
		}
	}
}
