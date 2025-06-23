package cit.backend.dto.request;

import cit.backend.Enum.PromotionStatus;
import cit.backend.Enum.PromotionType;
import jakarta.persistence.*;
import lombok.Data;

import java.math.BigDecimal;
import java.time.LocalDateTime;

@Data
public class PromotionRequest {

    private int id;

    private String code;

    private PromotionType type;


    private BigDecimal value;

    private int max_uses;

    private int used_count;

    private BigDecimal min_order_amount;

    private LocalDateTime startDate;

    private LocalDateTime endDate;

    private PromotionStatus status;
}
