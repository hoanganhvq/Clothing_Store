package cit.backend.repository;

import cit.backend.model.Customer;
import cit.backend.model.Order;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;

import java.time.LocalDateTime;
import java.util.List;

public interface OrderRepository  extends JpaRepository<Order, Integer> {
    List<Order> findByCustomerIsAndOrderDateBetween(Customer customer, LocalDateTime start, LocalDateTime end);
    Page<Order> findByOrderDateBetween(LocalDateTime start, LocalDateTime end, Pageable pageable);
}
