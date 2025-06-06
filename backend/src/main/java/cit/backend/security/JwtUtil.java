package cit.backend.security;


import io.jsonwebtoken.JwtException;
import io.jsonwebtoken.Jwts;
import io.jsonwebtoken.SignatureAlgorithm;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Component;

import java.time.Duration;
import java.util.Date;

@Component
public class JwtUtil{
    @Value("${spring.security.jwt.secret}")
    private String secret;

    @Value("${spring.security.jwt.expiration}")
    private Duration expiration;

    public String generateToken(String username ) {
        return Jwts.builder()
                .setSubject(username)
                .setIssuedAt(new Date())
                .setExpiration(new Date(System.currentTimeMillis() + expiration.toMillis()))
                .signWith(SignatureAlgorithm.HS256, secret)
                .compact();
    }


    public String extractUsername(String token){
        return Jwts.parserBuilder().setSigningKey(secret).build().parseClaimsJws(token).getBody().getSubject();
    }


    public boolean validateToken(String token){
        try{
           Jwts.parserBuilder().setSigningKey(token).build().parseClaimsJws(token);
           return true;
        }catch(JwtException | IllegalArgumentException e){
            return false;
        }
    }






}
