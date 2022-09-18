using AutoMapper;
using Microsoft.VisualBasic;
using RozetkaFinder.DTOs;
using RozetkaFinder.Models.User;
using RozetkaFinder.Repository;
using RozetkaFinder.Services.IdServices;
using RozetkaFinder.Services.PasswordServices;
using RozetkaFinder.Services.Security.JwtToken;
using RozetkaFinder.Services.Security.RefreshToken;
using RozetkaFinder.Services.ValidationServices;

namespace RozetkaFinder.Services.UserServices
{
    public interface IUserService
    {
        Task<TokenDTO> Create(UserRegisterDTO request);
        Task<TokenDTO> Update(UserInDTO request);
        Task<TokenDTO> Login(UserInDTO request);
        Task<IEnumerable<User>> GetAll();

    }
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _repository;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IIdService _configurationId;
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;
        public UserService(IConfiguration config,IMapper mapper, IUserRepository repository, IJwtService jwtConfiguration, IRefreshTokenService refreshTokenConfiguration, IIdService configurationId, IPasswordService passwordService, IValidationService validationService)
        {
            _mapper = mapper;
            _configuration = config;
            _repository = repository;
            _jwtService = jwtConfiguration;
            _refreshTokenService = refreshTokenConfiguration;
            _configurationId = configurationId;
            _passwordService = passwordService;
            _validationService = validationService;
        }
        public async Task<TokenDTO> Create(UserRegisterDTO request)
        {
            User user = _mapper.Map<User>(request);
            if (!await _validationService.EmailValidationAsync(request.Email))
                return null;

            byte[] passwordHash, passwordSalt;
            RefreshToken refToken;
            refToken = await _refreshTokenService.GenerateRefreshTokenAsync();
            (passwordHash, passwordSalt) = await _passwordService.CreatePasswordHashAsync(request.Password);

            user.IdHash = await _configurationId.ConfigIdHashAsync();
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.RefreshToken = refToken.Token;
            user.TokenExpires = refToken.Expires;
            user.TokenCreated = refToken.Created;
            user.Notification = "telegram";

            _repository.CreateAsync(user);

            return(new TokenDTO()
            {
                JwtToken = await _jwtService.GenerateJwtTokenAsync(user, _configuration.GetSection("AppSettings:Token").Value),
                Refresh = refToken
            });

        }
        public async Task<TokenDTO> Login(UserInDTO request)
        {
            User user = await this.FindByEmail(request.Email);
            if(user == null)
                return null;

            if (await _passwordService.AuthenticationPasswordHashAsync(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                RefreshToken refToken = await _refreshTokenService.GenerateRefreshTokenAsync();
                user.RefreshToken = refToken.Token;
                user.TokenCreated = refToken.Created;
                user.TokenExpires = refToken.Expires;
                await _repository.UpdateAsync(user);
                return new TokenDTO()
                {
                    JwtToken = await _jwtService.GenerateJwtTokenAsync(user, _configuration.GetSection("AppSettings:Token").Value),
                    Refresh = refToken
                };
            }
            else return null;
            
        }
        public async Task<TokenDTO> Update(UserInDTO request)
        {
            User user = await _repository.ReadAsync(request.Email);

            if (user == null)
                return null;
            else
            {
                RefreshToken refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync();
                user.RefreshToken = refreshToken.Token;
                user.TokenCreated = refreshToken.Created;
                user.TokenExpires = refreshToken.Expires;
                await _repository.UpdateAsync(user);

                return (new TokenDTO()
                {
                    JwtToken = await _jwtService.GenerateJwtTokenAsync(user, _configuration.GetSection("AppSettings:Token").Value),
                    Refresh = refreshToken
                });
            }
        }
        public async Task<IEnumerable<User>> GetAll()
        {
            return await _repository.GetAllAsync();
        }

        private async Task<User> FindByEmail(string email)
        {
            return await _repository.ReadAsync(email);
        }
    }
}
