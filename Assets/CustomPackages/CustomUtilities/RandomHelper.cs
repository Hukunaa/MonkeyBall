using UnityEngine;

namespace CustomUtilities
{
    public struct RandomHelper
    {
        public static int GetRandomWeightedIndex(int[] _weights)
        {
            // Get the total sum of all the weights.
            int weightSum = 0;
           
            for (int i = 0; i < _weights.Length; ++i)
            {
                weightSum += _weights[i];
            }
 
            // Step through all the possibilities, one by one, checking to see if each one is selected.
            int index = 0;
            int lastIndex =  _weights.Length - 1;
            while (index < lastIndex)
            {
                // Do a probability check with a likelihood of weights[index] / weightSum.
                if (Random.Range(0, weightSum) < _weights[index])
                {
                    return index;
                }
 
                // Remove the last item from the sum of total untested weights and try again.
                weightSum -= _weights[index++];
            }
 
            // No other item was selected, so return very last index.
            return index;
        }
    }
}