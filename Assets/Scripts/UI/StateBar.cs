using UnityEngine;
using UnityEngine.UI;

public class StateBar : MonoBehaviour
{
    public int Value { get; private set; }

    [SerializeField] private Text _textScore;
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _fill;
    [SerializeField] private Gradient _fillColor;

    public void Initialize()
    {
        _slider.normalizedValue = 1f;
        _fill.color = _fillColor.Evaluate(1f);
        _textScore.text = "0";
    }

    public void ChangeHealth(float partHealth)
    {
        _slider.normalizedValue = partHealth;
        _fill.color = _fillColor.Evaluate(partHealth);
    }

    public void ChangeScore(int score)
    {
        _textScore.text = score.ToString();
    }
}
