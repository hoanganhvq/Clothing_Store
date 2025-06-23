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
        System.out.println("login");
        // Bắt buộc Spring xác thực người dùng chính thức
        authManager.authenticate(
                new UsernamePasswordAuthenticationToken(request.getUsername(), request.getPassword())
        );

        // Tìm user để lấy role (phục vụ cho JWT token)
        Staff user = staffRepository.findByUsername(request.getUsername())
                .orElseThrow(() -> new RuntimeException("User not found"));

        // Tạo token với role
        String token = jwtUtil.generateToken(user.getUsername(), user.getRole());



        return new AuthRespone(token, user.getRole(), user.getId());
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