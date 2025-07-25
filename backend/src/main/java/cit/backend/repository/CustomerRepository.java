package cit.backend.repository;

import cit.backend.dto.respone.CustomerResponse;
import cit.backend.model.Customer;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.Optional;

public interface CustomerRepository extends JpaRepository <Customer, Integer> {
    Page<Customer> findByPhoneContainingIgnoreCase(String name, Pageable pageable);
    Optional<Customer> findByEmail(String email);
    Optional<Customer> findByPhone(String phone);

    Page<Customer> findByNameContainingIgnoreCaseOrPhoneContainingIgnoreCase(String name, String phone, Pageable pageable);
}
