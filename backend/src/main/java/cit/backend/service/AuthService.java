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
    private StaffRepository userRepository;

    @Autowired
    private PasswordEncoder passwordEncoder;

    @Autowired
    private JwtUtil jwtUtil;

    public AuthRespone login(LoginRequest loginRequest){
         authManager.authenticate(new UsernamePasswordAuthenticationToken(loginRequest.getUsername(), loginRequest.getPassword()));
        String accessToken = jwtUtil.generateToken(loginRequest.getUsername());
        return new AuthRespone(accessToken);
    }

    public String register(RegisterRequest registerRequest){
        if(userRepository.findByUsername(registerRequest.getUsername()).isPresent()){
            throw new UserAlreadyExit("User already exits");
        }
        Staff staff = new Staff();
        staff.setUsername(registerRequest.getUsername());
        staff.setPassword(passwordEncoder.encode(registerRequest.getPassword()));
        staff.setRole(Role.Staff);

        userRepository.save(staff);
        return jwtUtil.generateToken(staff.getUsername());
    }




}