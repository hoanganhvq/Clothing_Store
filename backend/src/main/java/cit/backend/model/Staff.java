package cit.backend.model;

import cit.backend.Enum.Role;
import jakarta.persistence.*;
import lombok.*;


@Entity
@Table(name = "staffs")
@NoArgsConstructor
@AllArgsConstructor@Data
public class Staff {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;

    @Column(nullable = false, unique = true, name = "username")
    private String username;

    @Column(nullable = false, name = "password")
    private String password;

    @Enumerated(EnumType.STRING)
    @Column(nullable = false, name = "role")
    private Role role;
}