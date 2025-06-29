package cit.backend.controller;

import cit.backend.dto.respone.AuthRespone;
import cit.backend.dto.request.LoginRequest;
import cit.backend.dto.request.RegisterRequest;
import cit.backend.service.AuthService;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("auth")
@Validated
public class AuthController {
    @Autowired
    private AuthService authService;

    @PostMapping("/login")
    public AuthRespone login(@RequestBody LoginRequest loginRequest){
        return authService.login(loginRequest);
    }

    @PostMapping("/register")
    public AuthRespone register(@Valid @RequestBody RegisterRequest registerRequest){
         return authService.register(registerRequest);
    }

}//Test API ok