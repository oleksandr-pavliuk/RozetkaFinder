using AutoMapper;
using RozetkaFinder.DTOs;
using RozetkaFinder.Models.User;
using RozetkaFinder.Repository;
using RozetkaFinder.Services.GoodsServices;
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
        Task<List<GoodDTO>> SearchGoods(string name);
        Task<bool> SubscribeGood(string id, string user);
    }
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<User> _repository;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IIdService _configurationId;
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;
        private readonly IGoodsService _goodsService;
        public UserService(IGoodsService goodsService, IConfiguration config,IMapper mapper, IRepository<User> repository, IJwtService jwtConfiguration, IRefreshTokenService refreshTokenConfiguration, IIdService configurationId, IPasswordService passwordService, IValidationService validationService)
        {
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
            if (!await _validationService.ModelValidationAsync(request))
                return null;

            User user = _mapper.Map<User>(request);

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

        //------------------------------------------------------------  LOGIN
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

        //----------------------------------------------------------- UPDATE REFTOKEN
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

        //----------------------------------------------------------  GET ALL USER (ADMIN)
        public async Task<IEnumerable<User>> GetAll()
        {
            return await _repository.GetAllAsync();
        }


        //----------------------------------------------------------  FIND EMAIL
        private async Task<User> FindByEmail(string email)
        {
            return await _repository.ReadAsync(email);
        }


        //--------------------------------------------------------- SEARCHING GOODS BY REQUEST 
        public async Task<List<GoodDTO>> SearchGoods(string name)
        {
            /*IMPORTANT CODE FOR GETING USER EMAIL
             var user = HttpContext.User.Claims.Where(i => i.Value.Contains('@')).FirstOrDefault();
            Console.Write(user.Value);
            */
            return await _goodsService.GetGoodsByRequestAsync(name);
        }

        //--------------------------------------------------------  SUBSCRIBE THE PRODUCT

        public async Task<bool> SubscribeGood(string id, string user)
        {
            return await _goodsService.GetGoodIDAsync(id, user);
        }
        
    }
}
