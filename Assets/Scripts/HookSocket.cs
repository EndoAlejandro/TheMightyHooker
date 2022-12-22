using Interfaces;
using Pooling;
using UnityEngine;

public class HookSocket : MonoBehaviour, IToggleChild
{
    [SerializeField] private bool invertValue;
    [SerializeField] private Color unActiveColor;
    [SerializeField] private PoolAfterSeconds toggleFx;

    private readonly Color activeColor = Color.white;
    private new SpriteRenderer renderer;

    public IToggle Toggle { get; private set; }
    public PoolAfterSeconds ToggleFx => toggleFx;
    public bool State { get; private set; }

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
        var t = transform;
        ToggleFx.Get<PoolAfterSeconds>(t.position, t.rotation);
        State = invertValue ? !value : value;
        renderer.color = State ? activeColor : unActiveColor;
    }

    private void OnDestroy()
    {
        if (Toggle == null) return;
        Toggle.OnToggle -= OnToggle;
    }
}