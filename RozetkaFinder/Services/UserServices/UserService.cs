using AutoMapper;
using RozetkaFinder.DTOs;
using RozetkaFinder.Models.User;
using RozetkaFinder.Repository;
using RozetkaFinder.Services.IdServices;
using RozetkaFinder.Services.PasswordServices;
using RozetkaFinder.Services.Security.JwtToken;
using RozetkaFinder.Services.Security.RefreshToken;

namespace RozetkaFinder.Services.UserServices
{
    public interface IUserService
    {
        Task<TokenDTO> Create(UserRegisterDTO user);
        Task<TokenDTO> Update(UserInDTO user);
    }
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _repository;
        private readonly IJwtService _jwtConfiguration;
        private readonly IRefreshTokenService _refreshTokenConfiguration;
        private readonly IConfigurationId _configurationId;
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;
        public UserService(IConfiguration config,IMapper mapper, IUserRepository repository, IJwtConfiguration jwtConfiguration, IRefreshTokenConfiguration refreshTokenConfiguration, IConfigurationId configurationId, IPasswordService passwordService)
        {
            _mapper = mapper;
            _configuration = config;
            _repository = repository;
            _jwtConfiguration = jwtConfiguration;
            _refreshTokenConfiguration = refreshTokenConfiguration;
            _configurationId = configurationId;
            _passwordService = passwordService;
        }
        public async Task<TokenDTO> Create(UserRegisterDTO request)
        {
            User user = _mapper.Map<User>(request);

            byte[] passwordHash, passwordSalt;
            RefreshToken refToken;
            refToken = await _refreshTokenConfiguration.GenerateRefreshToken();
            (passwordHash, passwordSalt) = await _passwordService.CreatePasswordHash(request.Password);

            user.IdHash = _configurationId.ConfigIdHash();
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.RefreshToken = refToken.Token;
            user.TokenExpires = refToken.Expires;
            user.TokenCreated = refToken.Created;
            user.Notification = "telegram";

            _repository.Create(user);

            return(new TokenDTO()
            {
                JwtToken = await _jwtConfiguration.GenerateJwtToken(user, _configuration.GetSection("AppSettings:Token").Value),
                Refresh = refToken
            });

        }
        public async Task<TokenDTO> Update(UserInDTO request)
        {
            User user = await _repository.Read(request.Email);

            if (user == null)
                return null;
            else
            {
                RefreshToken refreshToken = await _refreshTokenConfiguration.GenerateRefreshToken();
                user.RefreshToken = refreshToken.Token;
                user.TokenCreated = refreshToken.Created;
                user.TokenExpires = refreshToken.Expires;
                await _repository.Update(user);

                return (new TokenDTO()
                {
                    JwtToken = await _jwtConfiguration.GenerateJwtToken(user, _configuration.GetSection("AppSettings:Token").Value),
                    Refresh = refreshToken
                });
            }
                

        }
    }
}
