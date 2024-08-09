using UnityEngine;
using UnityEngine.UI;

public class ColorLerpBehaviour : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool _graphicDefaultColor = default;
    [Space]
    [SerializeField] private Color _startingColor = default;
    [SerializeField] private Color _targetColor = default;
    [Space]
    [SerializeField] private float _transitionTime = default;

    [Header("References")]
    [SerializeField] private Graphic _graphic = default;

    private float _elapsedTime = default;
    private bool _reverse = default;

    private void OnEnable()
    {
        _graphic.color = _graphicDefaultColor ? _graphic.color : _startingColor;
    }
    private void OnDisable()
    {
        _elapsedTime = 0f;
    }
    private void Update()
    {
        if (!_reverse)
        {
            _elapsedTime += Time.deltaTime;
            LerpColor(_elapsedTime);

            if (_elapsedTime >= _transitionTime)
                _reverse = true;

            return;
        }

        if (_reverse)
        {
            _elapsedTime -= Time.deltaTime;
            LerpColor(_elapsedTime);

            if (_elapsedTime <= 0f)
                _reverse = false;

            return;
        }
    }

    public void ChangeColorSetting(bool useGraphicDefaultColor, Color startingColor, Color targetColor)
    {
        _graphicDefaultColor = useGraphicDefaultColor;
        _startingColor = useGraphicDefaultColor ? _graphic.color : startingColor;
        _targetColor = useGraphicDefaultColor ? _graphic.color : targetColor;
        _graphic.color = _startingColor;
    }
    public void LerpColor(float elapsedTime)
    {
        _graphic.color = Color.Lerp(_startingColor, _targetColor, elapsedTime / _transitionTime);
    }
}