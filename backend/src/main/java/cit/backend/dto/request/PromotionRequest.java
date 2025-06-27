package cit.backend.dto.request;

import cit.backend.Enum.PromotionStatus;
import cit.backend.Enum.PromotionType;
import jakarta.persistence.*;
import jakarta.validation.Valid;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;
import java.time.LocalDateTime;

@Data
@NoArgsConstructor
@AllArgsConstructor
public class PromotionRequest {

    private String name;

    private PromotionType type;

    private BigDecimal value;

    private int max_uses;

    private int used_count;

    private BigDecimal min_order_amount;

    private LocalDateTime startDate;

    private LocalDateTime endDate;

    private PromotionStatus status;
}
