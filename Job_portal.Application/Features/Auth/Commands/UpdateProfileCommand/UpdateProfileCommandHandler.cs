using AutoMapper;
using Job_portal.Application.Common.Interfaces;
using Job_portal.Application.Common.Interfaces.Repositories;
using Job_portal.Application.DTOs;
using Job_portal.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Job_portal.Application.Features.Auth.Commands.UpdateProfileCommand
{
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, UpdateProfileResult>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IFileService _fileService;
        private readonly ILogger<UpdateProfileCommandHandler> _logger;

        public UpdateProfileCommandHandler(IMapper mapper,IUserRepository userRepository, IFileService fileService, ILogger<UpdateProfileCommandHandler> logger)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _fileService = fileService;
            _logger = logger;
        }
        public async Task<UpdateProfileResult> Handle(UpdateProfileCommand request, CancellationToken ct)
        {
            var user = await _userRepository.GetByIdWithProfileAsync(request.UserId,ct);
            if (user == null)
            {
                _logger.LogInformation("Profile update failed — user not found {UserId}",request.UserId);
                return new UpdateProfileResult(false, "User not Found");
            }

            // 2. ensure profile exists
            user.Profile ??= new UserProfile { UserId = request.UserId};

            // 3. update basic info
            if (!string.IsNullOrWhiteSpace(request.FullName))
                user.FullName = request.FullName;
            if (!string.IsNullOrWhiteSpace(request.Email) && request.Email != user.Email)
            {
                var emailExists = await _userRepository.ExistsByEmailAsync(request.Email,ct);
                if (emailExists)
                    return new UpdateProfileResult(false, "Email is already in use.");
                user.Email = request.Email;
            }
                
            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
                user.PhoneNumber = request.PhoneNumber;
            if (!string.IsNullOrWhiteSpace(request.Bio))
                user.Profile.Bio = request.Bio;

            // replaces entire skills list
            if (request.Skills != null && request.Skills.Any())
                user.Profile.Skills = request.Skills;

            // handle photo upload
            if (request.PhotoStream != null && request.PhotoFileName != null)
            {
                // delete old photo first
                if (!string.IsNullOrWhiteSpace(user.Profile.ProfilePhoto))
                    await _fileService.DeleteFileAsync(user.Profile.ProfilePhoto);

                user.Profile.ProfilePhoto = await _fileService.UploadImageAsync(request.PhotoStream,request.PhotoFileName,"profiles");
                _logger.LogInformation("Profile photo uploaded for user {UserId}", request.UserId);
            }

            //handle resume upload
            if (request.ResumeStream != null && request.ResumeFileName != null && request.ResumeOriginalName != null)
            {
                // delete old resume first
                if (!string.IsNullOrWhiteSpace(user.Profile.ResumePath))
                    await _fileService.DeleteFileAsync(user.Profile.ResumePath);

                user.Profile.ResumePath = await _fileService.UploadResumeAsync(request.ResumeStream,request.ResumeFileName,request.ResumeOriginalName);
                user.Profile.ResumeOriginalName = request.ResumeOriginalName;
                _logger.LogInformation("Resume uploaded for user {UserId}",request.UserId);
            }

            await _userRepository.SaveChangesAsync(ct);
            var userDto = _mapper.Map<UserDto>(user);
            return new UpdateProfileResult(true, "UserProfile Succesfully added.", userDto);
        }
    }
}
