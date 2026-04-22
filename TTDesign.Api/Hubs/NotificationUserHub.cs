using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using TTDesign.API.Constants;
using TTDesign.API.Hubs.Intefaces;

namespace TTDesign.API.Hubs
{
  [Authorize]
  public class NotificationUserHub : Hub
  {
    private readonly IUserConnectionManager _userConnectionManager;

    public NotificationUserHub( IUserConnectionManager userConnectionManager )
    {
      _userConnectionManager = userConnectionManager;
    }

    //public string GetConnectionId()
    //{
    //  var httpContext = this.Context.GetHttpContext();
    //  var userId = httpContext.Request.Query [ "user" ];
    //  _userConnectionManager.KeepUserConnection( userId, Context.ConnectionId );
    //  return Context.ConnectionId;
    //}

    ////Called when a connection with the hub is terminated.
    //public async override Task OnDisconnectedAsync( Exception exception )
    //{
    //  //get the connectionId
    //  var connectionId = Context.ConnectionId;
    //  _userConnectionManager.RemoveUserConnection( connectionId );
    //  var value = await Task.FromResult( 0 );
    //}

    public string GetConnectionId()
    {
      return Context.ConnectionId;
    }

    public async Task BroadcastToGroup( string groupName, string listUsersOnline )
    {
      await Clients.Group( groupName ).SendAsync( "JoinRoom", $"{groupName} online:  {listUsersOnline}" );
    }

    public async Task BroadcastToConnection()
    {
      await Clients.Client( Context.ConnectionId ).SendAsync( "Login", $"{Context.ConnectionId} has logged." );
    }

    [Authorize]
    public override async Task OnConnectedAsync()
    {
      var httpContext = this.Context.GetHttpContext();
      var userLogin = httpContext!.User.Claims.FirstOrDefault( x => x.Type == ClaimTypes.NameIdentifier )!.Value;
      var teamUserLogin = httpContext.User.Claims.FirstOrDefault( x => x.Type == Enums.CLAIM_TYPE_TEAM )!.Value;

      _userConnectionManager.KeepUserConnection( userLogin, Context.ConnectionId );
      await Groups.AddToGroupAsync( Context.ConnectionId, teamUserLogin );
    }
  }
}
