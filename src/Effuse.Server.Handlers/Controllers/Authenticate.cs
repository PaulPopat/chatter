﻿using Effuse.Core.Handlers;
using Effuse.Core.Handlers.Contracts;
using Effuse.Server.Services;

namespace Effuse.Server.Handlers.Controllers;

[Route(Method.Get, "/api/v1/auth/token")]
public class Authenticate : IHandler
{
  private struct Response
  {
    public string LocalToken { get; set; }
  }

  private readonly Auth auth;

  public Authenticate(Auth auth)
  {
    this.auth = auth;
  }

  public async Task<HandlerResponse> Handle(HandlerProps props)
  {
    var ssoToken = props.QueryParameters["token"];
    var password = props.QueryParameters["password"];

    var result = await this.auth.Authenticate(ssoToken, password);

    return new(200, new Response
    {
      LocalToken = result
    });
  }
}
