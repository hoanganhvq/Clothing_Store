package cit.backend.repository;

import cit.backend.Enum.Role;
import cit.backend.model.Staff;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.Optional;

public interface StaffRepository extends JpaRepository<Staff, Integer> {
    Page<Staff> findStaffByUsername(String username, Pageable pageable);
    Optional<Staff> findByUsername(String username);
}