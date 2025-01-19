public interface IInteractable
{
    public void Interact() { }

    public void Interact(bool state) { }

    public void ShowInteractionUI(bool state, bool gamepad = false) { }
}
