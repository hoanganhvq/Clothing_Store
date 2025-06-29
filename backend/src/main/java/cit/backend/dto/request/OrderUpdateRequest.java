package cit.backend.dto.request;

import cit.backend.Enum.OrderStatus;
import jakarta.validation.constraints.NotNull;
import jakarta.validation.constraints.PastOrPresent;
import jakarta.validation.constraints.Positive;
import lombok.Getter;
import lombok.Setter;

import java.math.BigDecimal;
import java.time.LocalDateTime;

@Getter
@Setter
public class OrderUpdateRequest {
    private Integer customerId;

    private Integer staffId;

    private Integer promotionId; // Nullable (optional promotion)

    private BigDecimal totalAmount;


}
