package cit.backend.dto.respone;

import cit.backend.Enum.Role;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@NoArgsConstructor
@AllArgsConstructor
public class StaffResponse {

    private int id;

    private String username;

    private Role role;
}
