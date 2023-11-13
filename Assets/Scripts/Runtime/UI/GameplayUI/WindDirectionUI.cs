using Gameplay;
using TMPro;
using UnityEngine;

namespace UI.GameplayUI
{
    public class WindDirectionUI : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text _windSpeedText;

        [SerializeField] 
        private WindController _windController;

        private void Awake()
        {
            _windController._onWindChanged += OnWindChanged;
        }

        private void OnWindChanged()
        {
            float windSpeed = Mathf.RoundToInt(_windController.WindForce);
            _windSpeedText.text = windSpeed.ToString()+"km/h";
        }
    }
}
