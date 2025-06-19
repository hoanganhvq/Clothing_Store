package cit.backend.dto.respone;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.List;

public class OrderDetailResponse {
    private int orderId;
    private int quantity;
    private int staffID;
    private int customerID;

    private BigDecimal subtotal;
    private LocalDateTime orderDate;
    private List<OrderDetailResponse> orderDetailList;
}
