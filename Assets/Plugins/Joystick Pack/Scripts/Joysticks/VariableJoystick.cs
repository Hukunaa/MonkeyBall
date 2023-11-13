using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class VariableJoystick : Joystick
{
    public float MoveThreshold { get { return moveThreshold; } set { moveThreshold = Mathf.Abs(value); } }

    [SerializeField] private float moveThreshold = 1;
    [SerializeField] private JoystickType joystickType = JoystickType.Fixed;
    
    private Vector2 fixedPosition = Vector2.zero;
    
    [SerializeField] 
    private Vector2 _defaultPos;

    [SerializeField] 
    private Color _usedColor;
    
    [SerializeField] 
    private Color _unusedColor;

    [SerializeField] 
    private float _fadeDuration;

    public UnityAction OnJoystickDown;
    public UnityAction OnJoystickUp;
    
    public void SetMode(JoystickType joystickType)
    {
        this.joystickType = joystickType;
        if(joystickType == JoystickType.Floating)
        {
            background.anchoredPosition = _defaultPos;
            backgroundImage.color = _unusedColor;
            handleImage.color = _unusedColor;
        }
    }

    protected override void Start()
    {
        base.Start();
        fixedPosition = background.anchoredPosition;
        SetMode(joystickType);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(joystickType == JoystickType.Floating)
        {
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            backgroundImage.DOColor(_usedColor, _fadeDuration);
            handleImage.DOColor(_usedColor, _fadeDuration);
            OnJoystickDown?.Invoke();
        }
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (joystickType == JoystickType.Floating)
        {
            background.anchoredPosition = _defaultPos;
            backgroundImage.DOColor(_unusedColor, _fadeDuration);
            handleImage.DOColor(_unusedColor, _fadeDuration);
            OnJoystickUp?.Invoke();
        }

        base.OnPointerUp(eventData);
    }

    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (joystickType == JoystickType.Dynamic && magnitude > moveThreshold)
        {
            Vector2 difference = normalised * (magnitude - moveThreshold) * radius;
            background.anchoredPosition += difference;
        }
        base.HandleInput(magnitude, normalised, radius, cam);
    }
}

public enum JoystickType { Fixed, Floating, Dynamic }