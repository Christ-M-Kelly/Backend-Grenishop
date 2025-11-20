using BackendGrenishop.DTOs.Request;
using BackendGrenishop.DTOs.Response;
using BackendGrenishop.Models;
using BackendGrenishop.Services.Interfaces;
using BackendGrenishop.Common.Helpers;
using Microsoft.AspNetCore.Identity;

namespace BackendGrenishop.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly JwtHelper _jwtHelper;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        JwtHelper jwtHelper,
        ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtHelper = jwtHelper;
        _logger = logger;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            // Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Erreur d'inscription",
                    Errors = new List<string> { "Cette adresse email est déjà utilisée" }
                };
            }

            // Create new user
            var user = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                Nom = registerDto.Nom,
                Prenom = registerDto.Prenom,
                DateInscription = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, registerDto.MotDePasse);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                _logger.LogWarning("Registration failed for {Email}: {Errors}", registerDto.Email, string.Join(", ", errors));
                
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Erreur d'inscription",
                    Errors = errors
                };
            }

            _logger.LogInformation("User {Email} registered successfully", registerDto.Email);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Inscription réussie"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for {Email}", registerDto.Email);
            return new AuthResponseDto
            {
                Success = false,
                Message = "Une erreur est survenue lors de l'inscription",
                Errors = new List<string> { "Veuillez réessayer plus tard" }
            };
        }
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                _logger.LogWarning("Login attempt failed: user not found for {Email}", loginDto.Email);
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Email ou mot de passe incorrect"
                };
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.MotDePasse, lockoutOnFailure: false);
            
            if (!result.Succeeded)
            {
                _logger.LogWarning("Login attempt failed: invalid password for {Email}", loginDto.Email);
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Email ou mot de passe incorrect"
                };
            }

            // Generate JWT token
            var token = _jwtHelper.GenerateToken(user);

            _logger.LogInformation("User {Email} logged in successfully", loginDto.Email);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Connexion réussie",
                Token = token,
                User = new UserProfileDto
                {
                    Id = user.Id,
                    Nom = user.Nom,
                    Prenom = user.Prenom,
                    Email = user.Email ?? string.Empty,
                    DateInscription = user.DateInscription
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for {Email}", loginDto.Email);
            return new AuthResponseDto
            {
                Success = false,
                Message = "Une erreur est survenue lors de la connexion"
            };
        }
    }

    public async Task<UserProfileDto?> GetUserProfileAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User profile not found for ID: {UserId}", userId);
                return null;
            }

            return new UserProfileDto
            {
                Id = user.Id,
                Nom = user.Nom,
                Prenom = user.Prenom,
                Email = user.Email ?? string.Empty,
                DateInscription = user.DateInscription
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user profile for {UserId}", userId);
            return null;
        }
    }
}
