using AutoMapper;
using Job_portal.Application.Common.Interfaces;
using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Application.DTOs;
using MediatR;

namespace Job_portal.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
    {
        private readonly IUserRepository _userRepo;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public LoginCommandHandler(IUserRepository userRepo,IJwtService jwtService,IMapper mapper)
        {
            _userRepo = userRepo;
            _jwtService = jwtService;
            _mapper = mapper;
        }
        public async Task<LoginResult> Handle(LoginCommand request, CancellationToken ct)
        {
            var user = await _userRepo.GetByEmailWithProfileAsync(request.Email,ct);
            if (user == null)
                return new LoginResult(false,"Invalid Email or Password.");

            var isValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if(!isValid)
                return new LoginResult(false, "Invalid Email or Password.");

            // Check if user is banned
            if (user.IsRemoved) 
                return new LoginResult(false, "your account has been removed.");    


            var userDto = _mapper.Map<UserDto>(user);
            var token = _jwtService.GenerateToken(user);
            return new LoginResult(true,"Login Succesfull!",token,userDto);
        }
    }
}
