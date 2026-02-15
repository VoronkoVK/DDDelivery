using MediatR;

namespace Ddd;

public abstract class DomainEvent : INotification
{
    public Guid EventId { get; protected set; }
    public DateTime OccurredAt { get; protected set; }

    protected DomainEvent()
    {
        EventId = Guid.NewGuid();
        OccurredAt = DateTime.UtcNow;
    }
}