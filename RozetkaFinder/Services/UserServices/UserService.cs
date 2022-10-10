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
        Task<TokenDTO> Registration(UserRegisterDTO request);
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

        // Method for user registration and adding to data base.
        public async Task<TokenDTO> Registration(UserRegisterDTO request)
        {
            _validationService.ModelValidation(request);

            User user = _mapper.Map<User>(request);

            byte[] passwordHash, passwordSalt;
           
            (passwordHash, passwordSalt) = await _passwordService.CreatePasswordHashAsync(request.Password);

            user.IdHash = _configurationId.ConfigIdHashAsync();
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            SetUserRefreshToken(ref user);

            await _repository.CreateAsync(user);

            return(new TokenDTO()
            {
                JwtToken = _jwtService.GenerateJwtTokenAsync(user, await _jsonService.GetJwtSaltAsync()),
                Refresh = user.RefreshToken
            });

        }

        // Method for login user in the system.
        public async Task<TokenDTO> Login(UserInDTO request)
        {
            User user = this.FindByEmail(request.Email);
            if (user == null)
                throw new UserNotFoundException(Constants.userNotFoundMessage);

            if (!await _passwordService.AuthenticationPasswordHashAsync(request.Password, user.PasswordHash, user.PasswordSalt))
                throw new UserNotFoundException(Constants.passwordOrEmailInvalid);
            
            SetUserRefreshToken(ref user);
            await _repository.UpdateAsync(user, u => user.Email == u.Email);
            
            return new TokenDTO()
            {
                JwtToken = _jwtService.GenerateJwtTokenAsync(user, await _jsonService.GetJwtSaltAsync()),
                Refresh = user.RefreshToken
            };

        }

        // Method for updating refresh token in the data base.
        public async Task<TokenDTO> Update(UserInDTO request)
        {
            User user = _repository.ReadAsync(u => u.Email == request.Email);

            if (user == null)
                throw new UserNotFoundException(Constants.userNotFoundMessage);
            else
            {
               SetUserRefreshToken(ref user);

                return (new TokenDTO()
                {
                    JwtToken = _jwtService.GenerateJwtTokenAsync(user, await _jsonService.GetJwtSaltAsync()),
                    Refresh = user.RefreshToken
                });
            }
        }
        

        // Method for setting user's reftoken  
        private void SetUserRefreshToken(ref User user)
        {
            RefreshToken refreshToken = _refreshTokenService.GenerateRefreshTokenAsync();
            user.RefreshToken = refreshToken.Token;
            user.TokenCreated = refreshToken.Created;
            user.TokenExpires = refreshToken.Expires;
        }

        //Method for updating user in database.
        public async void Update(User user)
        {
            await _repository.UpdateAsync(user, u => u.Email == user.Email);
        }

        // Method for user search in data base.
        public User GetUser(string userEmail)
        {
            return _repository.ReadAsync(u => u.Email == userEmail);
        }
        
        //Method for getting all users from data base (only for admins).
        public async Task<IEnumerable<User>> GetAll()
        {
            return await _repository.GetAllAsync();
        }

        // Method for user search by user's email.
        private User FindByEmail(string email)
        {
            return _repository.ReadAsync(u => u.Email == email);
        }

        // Method for changing user's password.
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

        //Method for changing user's notification.
        public async Task<string> ChangeNotificationSetting(string email)
        {
            User user = _repository.ReadAsync(u => u.Email == email);
            user.Notification = user.Notification == global::Notification.email ? global::Notification.telegram : global::Notification.email;
            await _repository.UpdateAsync(user, u => u.Email == user.Email);
            return Constants.notificationChanged;
        }
    }
}
