package cit.backend.model;

import com.fasterxml.jackson.annotation.JsonIgnore;
import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;

@Entity
@Data
@NoArgsConstructor
@AllArgsConstructor
@Table(name = "pointDiscountRules")
public class PointDiscountRule {
    @Id
    private int id;

    @Column(nullable=false, name = "minPoints")
    private int minPoints;

    @Column(nullable = false, name = "maxPoints")
    private int maxPoints;

    @Column( nullable = true, name = "discount")
    private BigDecimal discount;

    @OneToOne(mappedBy = "pointDiscountRule")
    @JsonIgnore
    private Customer customer;

}
