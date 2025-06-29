package cit.backend.dto.request;

import lombok.Getter;
import lombok.Setter;

import java.math.BigDecimal;

@Getter
@Setter
public class OrderItemUpdateRequest {
    private Integer productId;

    private Integer orderId;

    private Integer quantity;

    private BigDecimal productPrice;
}
