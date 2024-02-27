using Effuse.Core.Integration.Contracts;
using Unity;

namespace Effuse.Core.AWS.Integration;

public static class Deps
{
  public static void Register(UnityContainer container)
  {
    container.RegisterType<IParameters, ParameterStore>();
    container.RegisterType<IStatic, S3Statics>();
  }
}
