using UnityEngine;

public class StandartCanvas : CanvasManager
{
    [SerializeField] private IButtonManager startButton;
    [SerializeField] private bool needToReset = true;
    private IButtonManager startHandler;
    private IButtonManager[][] buttons;
    private bool startTaken;
    public int rowIndex = 0, colIndex = 0;

    public IButtonManager StartButton { get { return startButton; } set { if (startTaken) { startButton.Highlight(false); } startButton = value; startButton.Highlight(true); } }

    protected override void Enable()
    {
        base.Enable();
        manager.show += Show;
        manager.hide += Hide;

        ResetButton();
    }

    public void ResetButton()
    {
        if (needToReset)
        {
            startButton.Highlight(false);
            startButton = startHandler;
        }
        if (deviceType == 0)
            startButton.Highlight(true);
        else
            startButton.Highlight(false);
    }

    protected virtual void OnDisable()
    {
        startButton.Highlight(false);
    }

    protected override void Awake()
    {
        arUp = MoveUp;
        arLft = MoveLeft;
        arDwn = MoveDown;
        arRgt = MoveRight;
        crss = Activate;

        startHandler = startButton;
        startTaken = true;
        setuped = true;
    }

    public void Hide(int t)
    {
        startButton.Highlight(false);
    }
    public void Show(int t)
    {
        startButton.Highlight(true);
    }

    public void MoveUp(int t)
    {
        startButton.Highlight(false);
        startButton = startButton.Move(1);
        startButton.Highlight(true);
    }
    public void MoveLeft(int t)
    {
        startButton.Highlight(false);
        startButton = startButton.Move(2);
        startButton.Highlight(true);
    }
    public void MoveRight(int t)
    {
        startButton.Highlight(false);
        startButton = startButton.Move(3);
        startButton.Highlight(true);
    }
    public void MoveDown(int t)
    {
        startButton.Highlight(false);
        startButton = startButton.Move(4);
        startButton.Highlight(true);
    }
    public void Activate(int t)
    {
        startButton.Activate();
    }
}
