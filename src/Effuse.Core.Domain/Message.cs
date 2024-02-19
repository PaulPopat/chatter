namespace Effuse.Core.Domain;

public class Message
{
  public Message(string text, DateTime when, Guid userId)
  {
    Text = text;
    When = when;
    UserId = userId;
  }

  public string Text { get; }

  public DateTime When { get; }

  public Guid UserId { get; }
}
