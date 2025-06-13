package cit.backend.service;

import cit.backend.Enum.Role;
import cit.backend.dto.respone.AuthRespone;
import cit.backend.dto.request.LoginRequest;
import cit.backend.dto.request.RegisterRequest;
import cit.backend.exception.UserAlreadyExit;
import cit.backend.model.Staff;
import cit.backend.repository.StaffRepository;
import cit.backend.security.JwtUtil;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;

@Service
public class AuthService {
    @Autowired
    private AuthenticationManager authManager;

    @Autowired
    private StaffRepository staffRepository;

    @Autowired
    private PasswordEncoder passwordEncoder;

    @Autowired
    private JwtUtil jwtUtil;

    public AuthRespone login(LoginRequest request) {
        // tìm người dùng từ DB (ví dụ là bảng staffs)
        Staff user = staffRepository.findByUsername(request.getUsername())
                .orElseThrow(() -> new RuntimeException("User not found"));

        if (!passwordEncoder.matches(request.getPassword(), user.getPassword())) {
            throw new RuntimeException("Invalid credentials");
        }

        //  tạo token với role
        String token = jwtUtil.generateToken(user.getUsername(), user.getRole());

        return new AuthRespone(token);
    }


    public String register(RegisterRequest registerRequest) {
        if (staffRepository.findByUsername(registerRequest.getUsername()).isPresent()) {
            throw new UserAlreadyExit("User already exists");
        }

        if (registerRequest.getPassword() == null || registerRequest.getPassword().isBlank()) {
            throw new IllegalArgumentException("Password cannot be null or blank");
        }

        Staff staff = new Staff();
        staff.setUsername(registerRequest.getUsername());
        staff.setPassword(passwordEncoder.encode(registerRequest.getPassword()));
        staff.setRole(Role.Staff);

        staffRepository.save(staff);
        return jwtUtil.generateToken(staff.getUsername(), staff.getRole());
    }





}