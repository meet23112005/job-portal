using AutoMapper;
using Job_portal.Application.Common.Interfaces;
using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Application.DTOs;
using Job_portal.Domain.Entities;
using Job_portal.Domain.Enums;
using MediatR;

namespace Job_portal.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResult>
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IJwtService _jwtService;

        public RegisterCommandHandler(IUserRepository userRepo, IMapper mapper, IFileService fileService, IJwtService jwtService)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _fileService = fileService;
            _jwtService = jwtService;
        }
        public async Task<RegisterResult> Handle(RegisterCommand request, CancellationToken ct)
        {
            //check user Exist or not
            var emailExist = await _userRepo.ExistsByEmailAsync(request.Email, ct);
            if (emailExist)
                return new RegisterResult(false, "Email is Already Registreted.");


            //Hashed Password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var userRole = request.Role.ToLower() == "recruiter"
                                        ? UserRole.Recruiter
                                        : UserRole.Student;

            //create user.
            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = passwordHash,
                Role = userRole,
                Profile = new UserProfile() // create empty profile
            };

            //Upload profile photo if provided
            if (request.PhotoStream != null && request.PhotoFileName != null)
            {
                var photourl = await _fileService.UploadImageAsync(request.PhotoStream, request.PhotoFileName, "profiles");

                user.Profile.ProfilePhoto = photourl;
            }

            _userRepo.Add(user);
            await _userRepo.SaveChangesAsync(ct);

            //generate token
            var token = _jwtService.GenerateToken(user);

            // Map to DTO and return
            var userDto = _mapper.Map<UserDto>(user);

            return new RegisterResult(true, "Regsitration Successful.", token, userDto);
        }
    }
}
