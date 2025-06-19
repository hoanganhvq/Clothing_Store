package cit.backend.dto.request;

import cit.backend.model.OrderItemKey;
import lombok.Getter;
import lombok.Setter;


import java.math.BigDecimal;

@Getter
@Setter
public class OrderItemRequest {
    private int productId;

    private int orderId;

    private int quantity;

    private BigDecimal subtotal;

    private BigDecimal productPrice;


}
