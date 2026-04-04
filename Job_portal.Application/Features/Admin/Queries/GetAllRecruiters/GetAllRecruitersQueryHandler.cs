using AutoMapper;
using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.Admin.Queries.GetAllRecruiters
{
    public class GetAllRecruitersQueryHandler : IRequestHandler<GetAllRecruitersQuery,GetAllRecruitersResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetAllRecruitersQueryHandler(IUserRepository userRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<GetAllRecruitersResult> Handle(GetAllRecruitersQuery request, CancellationToken ct)
        {
            var recruiters = await _userRepository.GetAllRecruitersAsync(ct);


            var recruitersDtos = _mapper.Map<IEnumerable<UserDto>>(recruiters);
            return new GetAllRecruitersResult(true, "Recruiters fetched.", recruitersDtos);
        }
    }
}
