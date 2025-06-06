package cit.backend.repository;

import cit.backend.Enum.Role;
import cit.backend.model.Staff;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.Optional;

public interface UserRepository extends JpaRepository<Staff, Integer> {
    Optional<Staff> findByUsername(String username);
    Optional<Staff> findAllByRole(Role role);
}