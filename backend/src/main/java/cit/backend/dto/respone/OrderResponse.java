package cit.backend.dto.respone;

import cit.backend.Enum.OrderStatus;
import cit.backend.dto.request.OrderItemRequest;

import cit.backend.model.OrderItem;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;


@Data
@NoArgsConstructor
@AllArgsConstructor
public class OrderResponse {
    private int orderId;
    private LocalDateTime orderDate;
    private int customerId;
    private int staffId;
    private BigDecimal totalAmount;
    private List<OrderItemResponse> items;
//    private PromotionResponse promotionResponse;

}
