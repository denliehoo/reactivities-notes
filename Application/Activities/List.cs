using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

// the query handler
namespace Application.Activities
{
    public class List
    {
        // inherit from IRequest and IRequestHandler from MediatR
        public class Query : IRequest<Result<List<ActivityDto>>> { }

        public class Handler : IRequestHandler<Query, Result<List<ActivityDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
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
            public async Task<Result<List<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activities = await _context.Activities
                .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

                return Result<List<ActivityDto>>.Success(activities);
            }
        }
    }
}