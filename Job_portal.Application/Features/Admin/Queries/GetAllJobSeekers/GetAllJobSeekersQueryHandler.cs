using AutoMapper;
using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.Admin.Queries.GetAllJobSeekers
{
    public class GetAllJobSeekersQueryHandler :IRequestHandler<GetAllJobSeekersQuery,GetAllJobSeekersResult>
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public GetAllJobSeekersQueryHandler(
            IUserRepository userRepo,
            IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<GetAllJobSeekersResult> Handle(GetAllJobSeekersQuery request, CancellationToken ct)
        {
            var jobSeekers = await _userRepo.GetAllJobSeekersAsync(ct);
            var jobSeekerDtos = _mapper.Map<IEnumerable<UserDto>>(jobSeekers);

            return new GetAllJobSeekersResult(
                true, "Job seekers fetched.", jobSeekerDtos);
        }
    }
}
