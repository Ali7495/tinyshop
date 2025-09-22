
public sealed class UserRegisteredEvent : IDomainEvent
{
    public Guid UserId { get; set; }
    public Email Email { get; set; }
    public DateTime OccuredAt { get; } = DateTime.Now;

    public UserRegisteredEvent(Guid userId, Email email)
    {
        UserId = userId;
        Email = email;
    }
}