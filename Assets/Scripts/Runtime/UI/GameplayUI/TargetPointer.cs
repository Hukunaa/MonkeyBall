using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Character;
using TMPro;
using UnityEngine.UI;

public class TargetPointer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _distanceText;

    [SerializeField]
    public RectTransform _distancePointer;

    [SerializeField]
    public CanvasGroup _pointerCanvasGroup;

    [SerializeField]
    private float _maxDistance = 300f;

    [SerializeField]
    private float _minDistance = 25f;

    [SerializeField]
    private float _minAlpha = 0.5f;

    public bool _showPointer;

    private GameObject _player;

    private Vector3 _lookAtTarget;

    private Vector3 _scale;

    private float _alpha;

    public bool _isShown = false;

    private void Start()
    {
        Mathf.Clamp(_minAlpha, 0f, 1f);
    }

    public void FindPlayer()
    {
        if (_player == null)
        {
            var players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject obj in players)
            {
                var Player = obj.GetComponent<Player>();
                if (Player != null && Player.IsPlayer == true)
                {
                    _player = Player.gameObject;
                    break;
                }
            }
        }
    }

    void Update()
    {
        if (_player != null && _isShown)
        {
            Vector3 distanceVector = this.transform.position - _player.transform.position;
            float distance = distanceVector.magnitude;

            _distanceText.text = distance.ToString("F0") + "m";

            _lookAtTarget = _player.transform.position;
            _lookAtTarget.y = this.transform.position.y;
            _distancePointer.transform.LookAt(_lookAtTarget);

            distance = Mathf.Clamp(distance, _minDistance, _maxDistance);
            float _currentScale = (distance / _maxDistance);
            _scale = new Vector3(_currentScale, _currentScale, _currentScale);
            _alpha = _currentScale;

            _distancePointer.localScale = _scale;
            _pointerCanvasGroup.alpha = Mathf.Clamp(_alpha, _minAlpha, 1f);
        }

    }
}
