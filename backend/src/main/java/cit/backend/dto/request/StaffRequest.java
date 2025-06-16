package cit.backend.dto.request;

import cit.backend.Enum.Role;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
public class StaffRequest {

    private String username;

    private String password;

    private Role role;
}
