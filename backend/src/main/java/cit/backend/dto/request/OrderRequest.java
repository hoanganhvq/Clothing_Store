package cit.backend.dto.request;

import cit.backend.Enum.OrderStatus;
import jakarta.validation.constraints.*;
import lombok.Getter;
import lombok.Setter;

import java.math.BigDecimal;
import java.time.LocalDateTime;

@Getter
@Setter
public class OrderRequest {

    @Positive(message = "Customer ID must be greater than 0")
    private int customerId;

    @Positive(message = "Staff ID must be greater than 0")
    private int staffId;

    @Positive(message = "Promotion ID must be greater than 0")
    private Integer promotionId; // Nullable (optional promotion)

    @NotNull(message = "Total amount is required")
    private BigDecimal totalAmount;

    @PastOrPresent(message = "Order date cannot be in the future")
    private LocalDateTime orderDate = LocalDateTime.now();

    @NotNull(message = "Order status is required")
    private OrderStatus status = OrderStatus.values()[2];

    // @NotEmpty(message = "Order must contain at least one item")
    // private List<@Valid OrderItemRequest> orderItems = new ArrayList<>();
}
