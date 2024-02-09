namespace Effuse.Core.Integration.Contracts;

public interface IParameters
{
  Task<string> GetParameter(string name);
}