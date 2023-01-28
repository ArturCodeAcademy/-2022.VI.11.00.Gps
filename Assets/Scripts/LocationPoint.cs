using TMPro;
using UnityEngine;

public class LocationPoint : MonoBehaviour
{
    public string Text { set => _text.text = value; }

    [SerializeField] private TMP_Text _text;

    void Start()
    {
        UpdateForward();
    }

    public void UpdateForward()
    {
        transform.forward = transform.position.normalized;
    }
}
