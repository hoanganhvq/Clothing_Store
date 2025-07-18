package cit.backend.dto.request;

import cit.backend.Enum.OrderStatus;
import com.fasterxml.jackson.annotation.JsonProperty;
import jakarta.validation.constraints.*;
import lombok.Getter;
import lombok.Setter;

import java.math.BigDecimal;
import java.time.LocalDateTime;

@Getter
@Setter
public class OrderRequest {

    @JsonProperty("customer_id")
    @Positive(message = "Customer ID must be greater than 0")
    private int customerId;

    @JsonProperty("staff_id")
    @Positive(message = "Staff ID must be greater than 0")
    private int staffId;

    @JsonProperty("promotion_id")
//    @Positive(message = "Promotion ID must be greater than 0")
    private Integer promotionId; // Nullable (optional promotion)

    @JsonProperty("total_amount")
    @NotNull(message = "Total amount is required")
    private BigDecimal totalAmount;

    @JsonProperty("sub_total")
    @NotNull(message = "Total amount is required")
    private BigDecimal subTotal;

    @PastOrPresent(message = "Order date cannot be in the future")
    private LocalDateTime orderDate = LocalDateTime.now();

    @NotNull(message = "Order status is required")
    private OrderStatus status = OrderStatus.values()[2];

    @JsonProperty("is_cash")
    private Boolean isCash;

    @JsonProperty("is_use_customer_point")
    private Boolean isUseCustomerPoint;


    @JsonProperty("point_discount")
    private BigDecimal pointDiscount;
    // @NotEmpty(message = "Order must contain at least one item")
    // private List<@Valid OrderItemRequest> orderItems = new ArrayList<>();
}
