package cit.backend.dto.request;

import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.Pattern;
import jakarta.validation.constraints.Size;
import lombok.Data;

@Data
public class RegisterRequest {
    @NotBlank
    @Size(min = 4, max = 30, message = "Username must be between 4 and 30 characters")
    private String username;

    @NotBlank(message = "Password is required")
    @Size(min = 6, message = "Password must be at least 8 characters long")
    private String password;


    @NotBlank(message = "Phone is requires")
    public String phone;

    @NotBlank(message = "Role is required")
    @Pattern(
            regexp = "^(Admin|Staff)$",
            message = "Role must be one of: Admin, Staff"
    )
    private String role;
}