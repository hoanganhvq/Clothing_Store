package cit.backend.repository;

import cit.backend.model.PointDiscountRule;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import java.math.BigDecimal;
import java.util.Optional;

public interface PointDiscountRepository extends JpaRepository<PointDiscountRule, Integer> {

    @Query("SELECT r FROM PointDiscountRule r WHERE :point >= r.minPoints AND :point <= r.maxPoints")
    Optional<PointDiscountRule> findByPointRange(@Param("point") int point);
}

