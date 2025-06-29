package cit.backend.dto.request;

import jakarta.validation.constraints.*;
import lombok.Getter;
import lombok.Setter;

import java.math.BigDecimal;

@Getter
@Setter
public class OrderItemRequest {

    @Positive(message = "Product ID must be greater than 0")
    private int productId;

    @Positive(message = "Order ID must be greater than 0")
    private int orderId;

    @Positive(message = "Quantity must be greater than 0")
    private int quantity;

    @NotNull(message = "Product price is required")
    @DecimalMin(value = "0.0", inclusive = false, message = "Product price must be greater than 0")
    private BigDecimal productPrice;
}
