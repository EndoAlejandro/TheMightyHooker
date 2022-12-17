using Interfaces;
using UnityEngine;

public class HookSocket : MonoBehaviour, IToggleChild
{
    [SerializeField] private bool invertValue;
    [SerializeField] private Color unActiveColor;

    private Color activeColor = Color.white;

    public IToggle Toggle { get; private set; }
    public bool State { get; private set; }

    private new SpriteRenderer renderer;

    private void Awake()
    {
        Toggle = GetComponentInParent<IToggle>();
        renderer = GetComponentInChildren<SpriteRenderer>();
        if (Toggle != null)
            Toggle.OnToggle += OnToggle;
    }

    private void Start()
    {
        if (Toggle == null)
            OnToggle(true);
    }

    public void OnToggle(bool value)
    {
        State = invertValue ? !value : value;
        renderer.color = State ? activeColor : unActiveColor;
    }

    private void OnDestroy()
    {
        if (Toggle == null) return;
        Toggle.OnToggle -= OnToggle;
    }
}