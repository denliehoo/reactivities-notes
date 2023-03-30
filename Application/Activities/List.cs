using System.Linq;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

// the query handler
namespace Application.Activities
{
    public class List
    {
        // inherit from IRequest and IRequestHandler from MediatR
        public class Query : IRequest<Result<PagedList<ActivityDto>>>
        {
            public ActivityParams Params { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PagedList<ActivityDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _mapper = mapper;
                _context = context;
            }
            /*
            returns a Task of List<Activity> 
             we pass our Query which forms a request which pass that to our Handler, which returns 
            the data we are looking for specified in the IRequest interface. Eventually, we return
            the list of activities

            CancellationToken basically cancels the request.
            For example, if on our frontend, we call the API when user clicks something,
            but then the user quits the page before our API sends a response, our API
            would still send a respond. The CancellationToken prevents this from happening
            and cancels the request (if we want to implement it)
            */
            public async Task<Result<PagedList<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _context.Activities
                    .Where(d => d.Date >= request.Params.StartDate)
                    .OrderBy(d => d.Date) // in desecnding order
                    .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider,
                        new { currentUsername = _userAccessor.GetUsername() })
                    .AsQueryable(); //Note: AsQueryable is not an async method

                // filtering for if user is going and isnt host (i.e. what is he attending)
                if (request.Params.IsGoing && !request.Params.IsHost)
                {
                    query = query.Where(x => x.Attendees.Any(a => a.Username == _userAccessor.GetUsername()));
                }
                // filtering for if user ishost (i.e. what is he hosting)
                if (request.Params.IsHost && !request.Params.IsGoing)
                {
                    query = query.Where(x => x.HostUsername == _userAccessor.GetUsername());
                }

                return Result<PagedList<ActivityDto>>.Success(
                    // where query is our source
                    await PagedList<ActivityDto>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize)
                );
            }
        }
    }
}