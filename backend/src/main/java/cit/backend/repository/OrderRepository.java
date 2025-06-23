package cit.backend.repository;

import cit.backend.Enum.OrderStatus;
import cit.backend.model.Order;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.time.LocalDateTime;

@Repository
public interface OrderRepository extends JpaRepository<Order, Long> {
    Page<Order> findByOrderDateBetweenAndStatus(LocalDateTime startDate, LocalDateTime endDate, OrderStatus status, Pageable pageable);
}