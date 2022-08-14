using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using IBUAPI.Models;
using IBUAPI.Models.Dto;
using System.IdentityModel.Tokens.Jwt;

namespace IBUAPI.Services;

public class UserRegistrationService : IUserRegistration
{
    private readonly DataContext _context;
    private readonly IConfiguration _configuration;

    public UserRegistrationService(DataContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<User> RegisterUser(UserRegistrationDto userToReg)
    {
        if (_context.Users.Any(x => x.Email == userToReg.Email))
        {
            throw new ArgumentException("User with this email already exists");
        }

        CreatePasswordHash(userToReg.Password,
                           out byte[] passwordHash,
                           out byte[] passwordSalt);

        User user = new User()
        {
            Id = _context.Users.Count() + 1,
            UserName = userToReg.Name,
            Email = userToReg.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        // Generate random string for verification token. Length is 6.
        user.VerificationToken = GenerateRandomString(6);
        //Add an verification with an EmailSender.
        user.VerifiedAt = DateTime.Now.ToUniversalTime();

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public string Login(UserLoginDto user)
    {
        if (!_context.Users.Any(u => u.Email == user.Email))
        {
            throw new ArgumentException("User with this email does not exist");
        }

        User userToLogin = _context.Users.First(u => u.Email == user.Email);

        if (!VerifyPasswordHash(user.Password, userToLogin.PasswordHash, userToLogin.PasswordSalt))
        {
            throw new ArgumentException("Wrong password");
        }

        string token = CreateToken(userToLogin);
        return token;
    }

    private string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim> {
            new Claim(ClaimTypes.Name, user.UserName),
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("JwtKey").Value));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(claims: claims,
                                         expires: DateTime.Now.AddDays(10),
                                         signingCredentials: cred);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }

    private string? GenerateRandomString(int v)
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, v)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
