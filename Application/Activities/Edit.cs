using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Edit
    {
        public class Command : IRequest
        {
            public Activity Activity { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.Activity.Id);
                // abc = xxx ?? yyy means if xxx is null, set it to yyy
                // i.e. if request.Activity.Title is empty meaning request was sent without an activity title,
                // just set it to the current activity.Title
                // activity.Title = request.Activity.Title ?? activity.Title;
                // with automapper, instead of doing the above for each property, we can just do a one liner
                // below
                _mapper.Map(request.Activity, activity);
                
                await _context.SaveChangesAsync();
                return Unit.Value;
            }
        }
    }
}
