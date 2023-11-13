using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Rewards;
using UnityEngine;
using DG.Tweening;

namespace GameplayManagers
{
    public class TargetManager : MonoBehaviour
    {
        private GameObject[] _multiplierTargets;

        private GameObject[] _targetPointers;

        private GameObject[] _bounceRings;
        private GameObject[] _bouncePads;

        public static TargetManager Instance;

        private Dictionary<ETargetType, GameObject[]> _targets = new Dictionary<ETargetType, GameObject[]>();

        public enum ETargetType
        {
            BouncePad,
            BounceRing,
            Multiplier
        }

        private void Awake()
        {
            Instance = this;
        }

        public void UpdateTargets()
        {
            _targets[ETargetType.Multiplier] = GameObject.FindGameObjectsWithTag("Target");
            _targets[ETargetType.BounceRing] = GameObject.FindGameObjectsWithTag("BounceRing");
            _targets[ETargetType.BouncePad] = GameObject.FindGameObjectsWithTag("BouncePad");
            
            _targetPointers = GameObject.FindGameObjectsWithTag("Target")
                .Where(t => t.GetComponent<TargetPointer>() != null)
                .ToArray();

            FindLayoutTargetPointers();
        }

        private void FindLayoutTargetPointers()
        {
            foreach (GameObject obj in _targetPointers)
            {
                var Pointer = obj.GetComponent<TargetPointer>();
                if (Pointer._showPointer)
                {
                    Pointer.FindPlayer();
                }
            }
        }
        
        

        public void DisplayTargetPointers(bool show)
        {
            if (!show)
            {
                foreach (GameObject obj in _targetPointers)
                {
                    obj.GetComponent<TargetPointer>()._isShown = false;
                    obj.GetComponent<TargetPointer>()._pointerCanvasGroup.DOFade(0.0f, 0.5f);
                }
            }
            else
            {
                foreach (GameObject obj in _targetPointers)
                {
                    obj.GetComponent<TargetPointer>()._isShown = true;
                }
            }
        }

        public Dictionary<ETargetType, GameObject[]> Targets => _targets;

        public GameObject[] MultiplierTargets => _multiplierTargets;
        public GameObject[] BounceRings => _bounceRings;
        public GameObject[] BouncePads => _bouncePads;
    }
}
