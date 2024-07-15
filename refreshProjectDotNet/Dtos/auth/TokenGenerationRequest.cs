using System.ComponentModel.DataAnnotations;

namespace refreshProjectDotNet.Dtos.auth;

public record class CustomClaimDto
(
    [Required]string Type,
    [Required]string Value
);

public record class TokenGenerationRequestDTO
(
    [Required]string Email,
    [Required]string Password,
    List<CustomClaimDto> CustomClaims
    
);
