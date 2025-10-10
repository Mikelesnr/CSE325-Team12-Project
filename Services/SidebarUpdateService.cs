public class SidebarUpdateService
{
    public event Action? OnTroupeJoined;

    public void NotifyTroupeJoined()
    {
        OnTroupeJoined?.Invoke();
    }
}
