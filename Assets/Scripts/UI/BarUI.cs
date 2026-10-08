
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarUI : MonoBehaviour
{
  [SerializeField] private Color _backgroundColor;
  [SerializeField] private Color _fillColor;
  [SerializeField] private Color _fillColor2;
  [SerializeField] private Image _background;
  [SerializeField] private Image _progress;

  [SerializeField]
  private TMP_Text _valueText;

  private Color _currentColor;

  private void Awake()
  {
    if (_background != null)
      _background.color = _backgroundColor;

    if (_progress != null)
      _progress.color = _fillColor;
  }

  public void SetProgress(float value, float maxValue)
  {
    if (_valueText != null)
      _valueText.text = $"{value} / {maxValue}";

    if (_progress == null)
      return;

    float progress = maxValue > 0f ? value / maxValue : 0f;

    _progress.fillAmount = progress;

    Color targetColor = progress < 0.25f ? _fillColor2 : _fillColor;

    if (_currentColor != targetColor)
    {
      _progress.color = targetColor;
      _currentColor = targetColor;
    }
  }

}