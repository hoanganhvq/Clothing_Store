package cit.backend.dto.request;

import cit.backend.Enum.PromotionStatus;
import cit.backend.Enum.PromotionType;
import com.fasterxml.jackson.annotation.JsonProperty;
import jakarta.validation.constraints.*;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;
import java.time.LocalDateTime;

@Data
@NoArgsConstructor
@AllArgsConstructor
public class PromotionRequest {

    @JsonProperty("name")
    @NotBlank(message = "Promotion name is required")
    @Size(max = 255, message = "Promotion name must not exceed 255 characters")
    private String name;

    private PromotionType type = PromotionType.Percent;

    @JsonProperty("discount_percentage")
    @NotNull(message = "Promotion value is required")
    @DecimalMin(value = "0.0", inclusive = false, message = "Value must be greater than 0")
    private BigDecimal value;

    @Min(value = 0, message = "Max uses must be 0 or more")
    private int max_uses;

    @Min(value = 0, message = "Used count must be 0 or more")
    private int used_count;

    @DecimalMin(value = "0.0", inclusive = true, message = "Minimum order amount must be 0 or greater")
    private BigDecimal min_order_amount;

    @JsonProperty("start_date")
    @NotNull(message = "Start date is required")
    private LocalDateTime startDate;

    @JsonProperty("end_date")
    @NotNull(message = "End date is required")
    @Future(message = "End date must be in the future")
    private LocalDateTime endDate;

    private PromotionStatus status = PromotionStatus.Active;
}
