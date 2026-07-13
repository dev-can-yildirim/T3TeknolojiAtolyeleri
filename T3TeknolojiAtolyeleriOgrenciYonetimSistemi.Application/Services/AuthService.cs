using T3TeknolojiAtolyeleri.Domain.Entities;
using T3TeknolojiAtolyeleri.Domain.Enums;
using T3TeknolojiAtolyeleriApplication.DTOs.AuthDtos;
using T3TeknolojiAtolyeleriApplication.Interfaces.Auth;
using T3TeknolojiAtolyeleriApplication.Interfaces.Repositories;
using T3TeknolojiAtolyeleriApplication.Interfaces.Security;
using T3TeknolojiAtolyeleriApplication.Interfaces.Services;
using T3TeknolojiAtolyeleriApplication.Responses;

namespace T3TeknolojiAtolyeleriApplication.Services
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<AppUser> _userRepository;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(
            IGenericRepository<AppUser> userRepository,
            IPasswordHasherService passwordHasherService,
            IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _passwordHasherService = passwordHasherService;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<ServiceResponse<LoginResponseDto>> LoginAsync(LoginDto dto)
        {
            var email = dto.Email.Trim().ToLower();

            var users = await _userRepository.GetAllWithIncludesAsync(x => x.Teacher);

            var user = users.FirstOrDefault(x => x.Email.ToLower() == email);

            if (user == null)
            {
                return ServiceResponse<LoginResponseDto>.Fail("Email veya şifre hatalı.");
            }

            if (!user.IsActive)
            {
                return ServiceResponse<LoginResponseDto>.Fail("Bu kullanıcı hesabı aktif değil.");
            }

            var passwordIsValid = _passwordHasherService.VerifyPassword(
                dto.Password,
                user.PasswordHash
            );

            if (!passwordIsValid)
            {
                return ServiceResponse<LoginResponseDto>.Fail("Email veya şifre hatalı.");
            }

            int? teacherId = null;

            if (user.Role == UserRole.Teacher)
            {
                teacherId = user.Teacher?.Id;

                if (!teacherId.HasValue)
                {
                    return ServiceResponse<LoginResponseDto>.Fail("Öğretmen hesabı bulunamadı.");
                }
            }

            var tokenResult = _jwtTokenService.GenerateToken(user, teacherId);

            var response = new LoginResponseDto
            {
                UserId = user.Id,
                TeacherId = teacherId,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString(),
                Token = tokenResult.Token,
                ExpiresAt = tokenResult.ExpiresAt
            };

            return ServiceResponse<LoginResponseDto>.Ok(response, "Giriş başarılı.");
        }
    }
}
