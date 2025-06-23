package cit.backend.repository;

import cit.backend.model.Order;
import org.springframework.data.jpa.repository.JpaRepository;

import java.time.LocalDateTime;
import java.util.List;

public interface OrderRepository  extends JpaRepository<Order, Integer> {
    List  <Order> findByCustomerIdAndCreatedAtBetween(int customerId, LocalDateTime startDate, LocalDateTime endDate);
}
