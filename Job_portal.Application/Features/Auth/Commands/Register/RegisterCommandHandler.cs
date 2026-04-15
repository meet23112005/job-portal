using Job_portal.Application.Common.Interfaces;
using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Domain.Entities;
using Job_portal.Domain.Enums;
using MediatR;

namespace Job_portal.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResult>
    {
        private readonly IUserRepository _userRepo;
        private readonly IEmailService _emailService;

        private readonly IFileService _fileService;

        public RegisterCommandHandler(IUserRepository userRepo, IEmailService emailService, IFileService fileService)
        {
            _userRepo = userRepo;
            _emailService = emailService;
            _fileService = fileService;
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
                Profile = new UserProfile(), // create empty profile
                IsEmailConfirmed = false,
                EmailConfirmationToken = Guid.NewGuid().ToString("N") 

            };

            //Upload profile photo if provided
            if (request.PhotoStream != null && request.PhotoFileName != null)
            {
                var photourl = await _fileService.UploadImageAsync(request.PhotoStream, request.PhotoFileName, "profiles");

                user.Profile.ProfilePhoto = photourl;
            }

            _userRepo.Add(user);
            await _userRepo.SaveChangesAsync(ct);

            //Send Email Confirmation
            var confirmLink = $"https://localhost:44331/api/v1/user/confirm-email?token={user.EmailConfirmationToken}";

            await _emailService.SendConfirmationEmailAsync(
                                user.Email,
                                user.FullName,
                                confirmLink);


            return new RegisterResult(true, "Registration successful. Please check your email to confirm your account.");
        }
    }
}
