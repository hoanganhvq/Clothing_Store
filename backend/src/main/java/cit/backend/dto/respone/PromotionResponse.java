package cit.backend.dto.respone;

import cit.backend.Enum.PromotionStatus;
import cit.backend.Enum.PromotionType;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;
import java.time.LocalDateTime;

@Data
@NoArgsConstructor
@AllArgsConstructor
public class PromotionResponse {
    private int id;

    private String code;

    private PromotionType type;

    private BigDecimal value;

    private Integer max_uses;

    private Integer used_count;

    private BigDecimal min_order_amount;

    private LocalDateTime startDate;

    private LocalDateTime endDate;

    private PromotionStatus status;

}
