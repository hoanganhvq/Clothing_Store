package cit.backend.model;

import cit.backend.Enum.PromotionStatus;
import cit.backend.Enum.PromotionType;
import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;
import java.time.LocalDateTime;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Entity
@Table(name = "promotions")
public class Promotion {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;

    @Column(name = "code")
    private String code;

    @Enumerated(EnumType.STRING)
    @Column(name = "type")
    private PromotionType type;


    @Column(name = "discount")
    private BigDecimal value;

    @Column(name = "max_uses")
    private int max_uses;

    @Column(name = "used_count")
    private int used_count;

    @Column(name = "min_order_amount")
    private BigDecimal min_order_amount;

    @Column(name = "startDate")
    private LocalDateTime startDate;

    @Column(name = "endDate")
    private LocalDateTime endDate;

    @Enumerated(EnumType.STRING)
    @Column(name = "status")
    private PromotionStatus status;

    //Chua them quan he voi Order

}
