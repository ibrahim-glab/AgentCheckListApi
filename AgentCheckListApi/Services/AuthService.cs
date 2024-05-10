

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AgentCheckListApi.Helper;
using AgentCheckListApi.Interfaces;
using AgentCheckListApi.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace AgentCheckListApi.Services{
    public class AuthService : IAuthService
    {
   private readonly MongoDbService<Organization> _mongoDbService;
        private readonly IMongoCollection<User> _users;
        private readonly IMongoQueryable<User> _usersquery;
        private readonly IMongoQueryable<Permission> _Permissionquery;
        private readonly IMongoCollection<Organization> _organizations;
        private readonly IOptions<JWT> options;
        // add mongo collection Permissions
        private readonly IMongoCollection<Permission> _permissions;
        public AuthService(MongoDbService<Organization> mongoDbService , IOptions<JWT> options)
        {
            _mongoDbService = mongoDbService;
            _users = _mongoDbService.GetCollection<User>("users");
            _usersquery = _users.AsQueryable();
            _organizations = _mongoDbService.GetCollection<Organization>("organizations");
            _permissions = _mongoDbService.GetCollection<Permission>("permissions");
            _Permissionquery = _permissions.AsQueryable();
            this.options = options;
        }        
        public ServiceResult Authnticate(string userName , string password)
        {
            var user = _usersquery.FirstOrDefault(x => x.Username == userName && x.UserPassword == password);
            if(user is null)
                return new ServiceResult{Success = false , Message = "User Not Found"};
            var token = CreateTokenAsync(user);
            return new ServiceResult{Success = true , Message = "User Found" , Data = token};
        }

     private string  CreateTokenAsync(User user)
        {
          
            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, user.UserMobileNumber));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, new Guid().ToString()));
            var role = _Permissionquery.FirstOrDefault(x=>x.UserMobileNumber == user.UserMobileNumber);
            if (role is null)
                claims.Add(new Claim(ClaimTypes.Role, "NA"));
            else{claims.Add(new Claim(ClaimTypes.Role, role.Role.ToString()));}

           

            var symKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Key));
            var signingCredentials = new SigningCredentials(symKey, SecurityAlgorithms.HmacSha256Signature);
            var SecurityToken = new JwtSecurityToken(
                issuer: options.Value.Issuer,
                audience: options.Value.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(options.Value.DurationInHours),
                signingCredentials: signingCredentials
                );
            var Token = new JwtSecurityTokenHandler().WriteToken(SecurityToken);
            return Token ;
        }
    }
}