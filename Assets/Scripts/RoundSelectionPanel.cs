using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoundSelectionPanel : MonoBehaviour
{
    public const string COUNT_KEY = "RC";

    [SerializeField] private int _gameSceneIndex;

    [Header("UI")]
    [SerializeField] private Slider _countSlider;
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _closeButton;

    public void OnSliderValueChanged(float value)
    {
        int v = (int)value;
        _countText.text = v.ToString();
        SceneDataContainer.Instance.SetData(COUNT_KEY, v);
    }

    public void OnStartClick()
    {
        SceneManager.LoadScene(_gameSceneIndex);
    }

    public void SetPanelActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
