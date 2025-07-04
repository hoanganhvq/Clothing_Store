package cit.backend.dto.request;

import cit.backend.Enum.PromotionStatus;
import cit.backend.Enum.PromotionType;
import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.Getter;
import lombok.Setter;

import java.math.BigDecimal;
import java.time.LocalDateTime;

@Getter
@Setter
public class PromotionUpdateRequest
{
    @JsonProperty("name")
    private String name;

    private PromotionType type;

    @JsonProperty("discount_percentage")
    private BigDecimal value;

    private Integer max_uses;

    private Integer used_count;

    private BigDecimal min_order_amount;

    @JsonProperty("start_date")
    private LocalDateTime startDate;

    @JsonProperty("end_date")
    private LocalDateTime endDate;

    private PromotionStatus status;
}
