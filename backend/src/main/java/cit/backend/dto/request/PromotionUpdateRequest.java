package cit.backend.dto.request;

import cit.backend.Enum.PromotionStatus;
import cit.backend.Enum.PromotionType;
import lombok.Getter;
import lombok.Setter;

import java.math.BigDecimal;
import java.time.LocalDateTime;

@Getter
@Setter
public class PromotionUpdateRequest
{

    private String name;

    private PromotionType type;

    private BigDecimal value;

    private Integer max_uses;

    private Integer used_count;

    private BigDecimal min_order_amount;

    private LocalDateTime startDate;

    private LocalDateTime endDate;

    private PromotionStatus status;
}
