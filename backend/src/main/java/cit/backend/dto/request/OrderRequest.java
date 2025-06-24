package cit.backend.dto.request;

import cit.backend.Enum.OrderStatus;
import cit.backend.model.OrderItem;
import lombok.Getter;
import lombok.Setter;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.List;

@Getter
@Setter
public class OrderRequest {

    private int customerId;

    private int staffId;

    private Integer promotionId; //Co the co hoac khonf

    private BigDecimal totalAmount;

    private LocalDate orderDate = LocalDate.now();

    private OrderStatus status = OrderStatus.values()[2];


//    private List<OrderItemRequest> orderItems;

}
