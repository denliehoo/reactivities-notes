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
        public class Query: IRequest<List<Activity>>{}

        public class Handler : IRequestHandler<Query, List<Activity>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
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
            public async Task<List<Activity>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Activities.ToListAsync();
            }
        }
    }
} 