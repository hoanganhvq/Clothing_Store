package cit.backend.dto.respone;

import cit.backend.Enum.OrderStatus;
import cit.backend.dto.request.OrderItemRequest;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.util.List;


@Data
@NoArgsConstructor
@AllArgsConstructor
public class OrderResponse {
    private int id;

    private LocalDate orderDate;

    private OrderStatus status;

    private CustomerResponse customerRespone;

    private StaffResponse staffResponse;

    private PromotionResponse promotionResponse;

    private BigDecimal totalAmount;
}
