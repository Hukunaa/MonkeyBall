using System;
using System.Collections;
using Gameplay.Character;
using Gameplay.Rewards;
using TMPro;
using UnityEngine;

namespace UI.GameplayUI
{
    [RequireComponent(typeof(FollowTransform))][RequireComponent(typeof(FaceMainCamera))]
    public class PointsEarnUIFeedback : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _pointsEarned_tmp;

        [SerializeField] 
        private TMP_Text _rewardTypeFeedback_tmp;
    
        [SerializeField] 
        private float _displayTime;
    
        private FollowTransform _followTransformComponent;
        private FaceMainCamera _faceMainCamera;
    
        private Player _player;
        private Canvas _canvas;

        private void Awake()
        {
            _followTransformComponent = GetComponent<FollowTransform>();
            _faceMainCamera = GetComponent<FaceMainCamera>();
            _canvas = GetComponent<Canvas>();
        }

        private void Start()
        {
            Hide();
        }
    
        public void SetPlayerInfo(Player _player)
        {
            this._player = _player;
            _followTransformComponent.SetTarget(_player.transform);
            //this._player.Score.onScoreIncreased += DisplayPointsEarned;
        }

        private void DisplayPointsEarned(int _pointsEarned, EScoreType _scoreType)
        {
            StartCoroutine(DisplayPointsEarnedCoroutine(_pointsEarned, _scoreType));
        }

        private IEnumerator DisplayPointsEarnedCoroutine(int _pointsEarned, EScoreType _scoreType)
        {
            _pointsEarned_tmp.text = $"+ {_pointsEarned} Points";
            switch (_scoreType)
            {
                case EScoreType.Coins:
                    _rewardTypeFeedback_tmp.enabled = false;
                    break;
                case EScoreType.Target:
                    _rewardTypeFeedback_tmp.enabled = false;
                    break;
                case EScoreType.SpeedBonus:
                    _rewardTypeFeedback_tmp.text = "Speed bonus";
                    _rewardTypeFeedback_tmp.enabled = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_scoreType), _scoreType, null);
            }
            Show();
            yield return new WaitForSeconds(_displayTime);
            Hide();
        }

        public void Show()
        {
            _followTransformComponent.enabled = true;
            _faceMainCamera.enabled = true;
            _canvas.enabled = true;
        }

        public void Hide()
        {
            _canvas.enabled = false;
            _followTransformComponent.enabled = false;
            _faceMainCamera.enabled = false;
        }
    }
}
