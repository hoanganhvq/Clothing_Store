package cit.backend.dto.request;

import com.fasterxml.jackson.annotation.JsonProperty;
import jakarta.validation.constraints.*;
import lombok.Getter;
import lombok.Setter;

import java.math.BigDecimal;

@Getter
@Setter
public class OrderItemRequest {

    @JsonProperty("product_id")
    @Positive(message = "Product ID must be greater than 0")
    private int productId;

    @JsonProperty("order_id")
    @Positive(message = "Order ID must be greater than 0")
    private int orderId;

    @JsonProperty("quantity")
    @Positive(message = "Quantity must be greater than 0")
    private int quantity;

    @JsonProperty("price")
    @NotNull(message = "Product price is required")
    @DecimalMin(value = "0.0", inclusive = false, message = "Product price must be greater than 0")
    private BigDecimal productPrice;
}
