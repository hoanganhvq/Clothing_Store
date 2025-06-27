package cit.backend.repository;

import cit.backend.dto.respone.CustomerResponse;
import cit.backend.model.Customer;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;

public interface CustomerRepository extends JpaRepository <Customer, Integer> {
    Page<Customer> findByNameContainingIgnoreCase(String name, Pageable pageable);
}
