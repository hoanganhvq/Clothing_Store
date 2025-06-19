package cit.backend.dto.respone;

import cit.backend.Enum.Role;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
public class AuthRespone {

        private String Access_token;
        private Role Role;
        private int Staff_id;
        }