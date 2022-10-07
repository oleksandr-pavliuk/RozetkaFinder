using AutoMapper;
using RozetkaFinder.Helpers.Constants;
using RozetkaFinder.DTOs;
using RozetkaFinder.Helpers;
using RozetkaFinder.Models.Exceptions;
using RozetkaFinder.Models.User;
using RozetkaFinder.Repository;
using RozetkaFinder.Services.GoodsServices;
using RozetkaFinder.Services.JSONServices;
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
        void Update(User user);
        Task<TokenDTO> Login(UserInDTO request);
        Task<IEnumerable<User>> GetAll();
        Task<string> ChangePassword(string email, string oldPassword, string newPassword);
        Task<string> ChangeNotificationSetting(string email);
        public User GetUser(string userEmail);
    }
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<User> _repository;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IIdHelper _configurationId;
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;
        private readonly IGoodsService _goodsService;
        private readonly IJsonService _jsonService;
        public UserService( IJsonService jsonService, IGoodsService goodsService, IConfiguration config,IMapper mapper, IRepository<User> repository, IJwtService jwtConfiguration, IRefreshTokenService refreshTokenConfiguration, IIdHelper configurationId, IPasswordService passwordService, IValidationService validationService)
        {
            _jsonService = jsonService;
            _goodsService = goodsService;
            _mapper = mapper;
            _configuration = config;
            _repository = repository;
            _jwtService = jwtConfiguration;
            _refreshTokenService = refreshTokenConfiguration;
            _configurationId = configurationId;
            _passwordService = passwordService;
            _validationService = validationService;
        }

        // -------------------------------------------------------------   REGISTRATION
        public async Task<TokenDTO> Create(UserRegisterDTO request)
        {
            _validationService.ModelValidation(request);

            User user = _mapper.Map<User>(request);

            byte[] passwordHash, passwordSalt;
            RefreshToken refToken;
            refToken = _refreshTokenService.GenerateRefreshTokenAsync();
            (passwordHash, passwordSalt) = await _passwordService.CreatePasswordHashAsync(request.Password);

            user.IdHash = _configurationId.ConfigIdHashAsync();
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.RefreshToken = refToken.Token;
            user.TokenExpires = refToken.Expires;
            user.TokenCreated = refToken.Created;

            await _repository.CreateAsync(user);

            return(new TokenDTO()
            {
                JwtToken = _jwtService.GenerateJwtTokenAsync(user, await _jsonService.GetJwtSaltAsync()),
                Refresh = refToken
            });

        }

        // ------------------------------------------------------------  LOGIN
        public async Task<TokenDTO> Login(UserInDTO request)
        {
            User user = this.FindByEmail(request.Email);
            if (user == null)
                throw new UserNotFoundException(Constants.userNotFoundMessage);

            if (!await _passwordService.AuthenticationPasswordHashAsync(request.Password, user.PasswordHash, user.PasswordSalt))
                throw new UserNotFoundException(Constants.passwordOrEmailInvalid);
            
            RefreshToken refToken = _refreshTokenService.GenerateRefreshTokenAsync();
            user.RefreshToken = refToken.Token;
            user.TokenCreated = refToken.Created;
            user.TokenExpires = refToken.Expires;
            await _repository.UpdateAsync(user, u => user.Email == u.Email);
            
            return new TokenDTO()
            {
                JwtToken = _jwtService.GenerateJwtTokenAsync(user, await _jsonService.GetJwtSaltAsync()),
                Refresh = refToken
            };

        }

        //----------------------------------------------------------- UPDATE REFTOKEN
        public async Task<TokenDTO> Update(UserInDTO request)
        {
            User user = _repository.ReadAsync(u => u.Email == request.Email);

            if (user == null)
                throw new UserNotFoundException(Constants.userNotFoundMessage);
            else
            {
                RefreshToken refreshToken = _refreshTokenService.GenerateRefreshTokenAsync();
                user.RefreshToken = refreshToken.Token;
                user.TokenCreated = refreshToken.Created;
                user.TokenExpires = refreshToken.Expires;
                await _repository.UpdateAsync(user, u => u.Email == user.Email);

                return (new TokenDTO()
                {
                    JwtToken = _jwtService.GenerateJwtTokenAsync(user, await _jsonService.GetJwtSaltAsync()),
                    Refresh = refreshToken
                });
            }
        }

        public async void Update(User user)
        {
            await _repository.UpdateAsync(user, u => u.Email == user.Email);
        }
        // --------------------------------------------------------- FIND USER

        public User GetUser(string userEmail)
        {
            return _repository.ReadAsync(u => u.Email == userEmail);
        }
        //----------------------------------------------------------  GET ALL USER (ADMIN)
        public async Task<IEnumerable<User>> GetAll()
        {
            return await _repository.GetAllAsync();
        }


        //----------------------------------------------------------  FIND EMAIL
        private User FindByEmail(string email)
        {
            return _repository.ReadAsync(u => u.Email == email);
        }

        //-------------------------------------------------------- CHANGE PASSWORD
        public async Task<string> ChangePassword(string email, string oldPassword, string newPassword)
        {
            User user = _repository.ReadAsync(u => u.Email == email);
            if (oldPassword == newPassword)
                throw new AuthenticationException(Constants.authMakeNewPassword);

            if (await _passwordService.AuthenticationPasswordHashAsync(oldPassword, user.PasswordHash, user.PasswordSalt))
            {
                (user.PasswordHash, user.PasswordSalt) = await _passwordService.CreatePasswordHashAsync(newPassword);
                await _repository.UpdateAsync(user, u => u.Email == user.Email);
                return Constants.passwordChanged;
            }
            else
                throw new AuthenticationException(Constants.passwordIsNotCorrect);

        }

        //--------------------------------------------------------- CHANGE NOTIFICATION
        public async Task<string> ChangeNotificationSetting(string email)
        {
            User user = _repository.ReadAsync(u => u.Email == email);
            user.Notification = user.Notification == global::Notification.email ? global::Notification.telegram : global::Notification.email;
            await _repository.UpdateAsync(user, u => u.Email == user.Email);
            return Constants.notificationChanged;
        }
    }
}
