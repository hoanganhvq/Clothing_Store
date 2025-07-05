package cit.backend.dto.request;

import cit.backend.Enum.Role;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.Pattern;
import jakarta.validation.constraints.Size;
import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class StaffUpdateDTO {
    private String username;

    private String password;

    private String phone;

    private Role role;
}
