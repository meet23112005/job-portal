using Job_portal.Application.Common.Interfaces.Repositories;
using MediatR;

namespace Job_portal.Application.Features.SavedJobs.Queries.GetSaveStatus
{
    public class GetSaveStatusQueryHandler : IRequestHandler<GetSaveStatusQuery,GetSaveStatusResult>
    {
        private readonly ISavedJobRepository _savedJobRepository;

        public GetSaveStatusQueryHandler(ISavedJobRepository savedJobRepository)
        {
            _savedJobRepository = savedJobRepository;
        }

        public async Task<GetSaveStatusResult> Handle(GetSaveStatusQuery request, CancellationToken ct)
        {
            var isSaved = await _savedJobRepository.AlreadySavedAsync(request.UserId, request.JobId, ct);
            return new GetSaveStatusResult(true, "Save status fetched.", isSaved);
        }
    }
}
