using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class CollectiblesManager : MonoBehaviour
    {
        private readonly HashSet<Collectible> _collectibles = new HashSet<Collectible>();
        
        public void GetCollectibles()
        {
            _collectibles.Clear();
            var collectiblesGo = GameObject.FindGameObjectsWithTag("Collectible");
            foreach (var collectibleGo in collectiblesGo)
            {
                _collectibles.Add(collectibleGo.GetComponent<Collectible>());
            }
        }

        public void ResetCollectibles()
        {
            foreach (var collectible in _collectibles)
            {
                collectible.ResetPickup();
            }
        }
    }
}