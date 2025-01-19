public class PopUpCanvas : StandartCanvas
{
    private CanvasManager handler;

    protected override void OnEnable()
    {
        base.OnEnable();
        handler = manager.canvas;
    }

    protected override void Enable()
    {
        base.Enable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        handler.ReEnable();
    }
}
