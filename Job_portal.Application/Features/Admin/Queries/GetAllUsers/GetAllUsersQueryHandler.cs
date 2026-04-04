using AutoMapper;
using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.Admin.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUserQuery,GetAllUserResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<GetAllUserResult> Handle(GetAllUserQuery request, CancellationToken ct)
        {
            var user = await _userRepository.GetAllAsync(ct);

            var userDtos = _mapper.Map<IEnumerable<UserDto>>(user);
            return new GetAllUserResult(true, "All Users Fetched Succesfully.", userDtos);
        }
    }
}
