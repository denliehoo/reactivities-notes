using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Comments;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;
        public ChatHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task SendComment(Create.Command command)
        {
            var comment = await _mediator.Send(command);

            // send the comment to the clients connected to the hub
            // we can specify which clients by using Clients.XXX
            await Clients.Group(command.ActivityId.ToString())
                .SendAsync("ReceiveComment", comment.Value);
        }

        // here we are overriding the OnConenctedAsync method from SignalR
        // so that we can control what happens when a client is connected
        /* whenever a client connects, we join them to the group with the activityId
        then we send them a list of comments we get from our DB
        anytime a comment is sent to the group, they will receive comment based on the
        "ReceiveComment" method in the SendComment method above */
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var activityId = httpContext.Request.Query["activityId"];
            await Groups.AddToGroupAsync(Context.ConnectionId, activityId);
            var result = await _mediator.Send(new List.Query { ActivityId = Guid.Parse(activityId) });
            await Clients.Caller.SendAsync("LoadComments", result.Value);
        }
    }
}