using UnityEngine;
using UnityEngine.Events;

public class WarpDetector : MonoBehaviour
{
    [SerializeField][Tooltip("Any position change magnitude superior to this threshold will be considered a wrap")] 
    private float _warpThreshold = 5;

    [SerializeField] 
    private UnityEvent<Vector3> _onWarpDetected;
    
    private Vector3 _previousPos;

    private bool _targetSet;
    
    public void TargetChanged()
    {
        _previousPos = transform.position;
        _targetSet = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (_targetSet == false) return;
        
        var positionDelta = transform.position - _previousPos;
        var positionChangeMagnitude = positionDelta.sqrMagnitude;

        if (positionChangeMagnitude > _warpThreshold * _warpThreshold)
        {
            Debug.Log("Warp Detected!");
            _onWarpDetected?.Invoke(positionDelta);
        }

        _previousPos = transform.position;
    }
}
