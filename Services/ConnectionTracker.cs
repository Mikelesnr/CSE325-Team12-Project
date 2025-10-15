using Microsoft.AspNetCore.Components.Server.Circuits;

namespace CSE325_Team12_Project.Services
{
    public class ConnectionTracker : CircuitHandler
    {
        public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            Console.WriteLine("⚠️ Blazor circuit disconnected.");
            return Task.CompletedTask;
        }

        public override Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            Console.WriteLine("✅ Blazor circuit reconnected.");
            return Task.CompletedTask;
        }
    }
}
