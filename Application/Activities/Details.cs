using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Details
    {
        // get an activity based on the Id
        public class Query: IRequest<Activity>
        {
            public Guid Id {get; set; }
        }
        public class Handler : IRequestHandler<Query, Activity>
        {
            // DataContext is the database
        private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;    
            }
            public async Task<Activity> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Activities.FindAsync(request.Id);
            }
        }
    }
}