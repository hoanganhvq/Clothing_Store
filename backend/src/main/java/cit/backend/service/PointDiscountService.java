package cit.backend.service;

import cit.backend.exception.PointDiscountRuleNotFound;
import cit.backend.model.PointDiscountRule;
import cit.backend.repository.PointDiscountRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;

@Service
public class PointDiscountService {
    @Autowired
    private PointDiscountRepository pointDiscountRepository;

    public PointDiscountRule getPointRange(int point){
        PointDiscountRule rule = pointDiscountRepository.findByPointRange(point)
                .orElseThrow(()->new PointDiscountRuleNotFound("No discount rule found for point"));
        return rule;
    }

    public BigDecimal calculateDiscount(BigDecimal orderTotal, int customerPoint) {
        PointDiscountRule rule = getPointRange(customerPoint);
        BigDecimal discountPercent = rule.getDiscount();

        // discount = orderTotal * discountPercent
        return orderTotal.multiply(discountPercent);
    }
}
