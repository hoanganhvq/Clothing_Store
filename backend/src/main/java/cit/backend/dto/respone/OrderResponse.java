package cit.backend.dto.respone;

import cit.backend.Enum.OrderStatus;
import cit.backend.dto.request.OrderItemRequest;

import lombok.Data;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.util.List;


@Data
public class OrderResponse {
    private int id;

    private LocalDate orderDate;

    private OrderStatus status;

    private CustomerResponse customerRespone;

    private StaffResponse staffResponse;

    private PromotionResponse promotionResponse;

    private BigDecimal totalAmount;

    private List<OrderItemRequest> orderItems;
}
