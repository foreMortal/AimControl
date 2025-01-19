public class SwappableCanvas : CanvasManager
{
    private CanvasManager handler;

    protected override void Enable()
    {
        handler = manager.canvas;
        base.Enable();
    }

    private void OnDisable()
    {
        handler.ReEnable();
    }
}
