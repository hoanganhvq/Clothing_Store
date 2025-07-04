package cit.backend.dto.respone;

import cit.backend.Enum.OrderStatus;
import cit.backend.dto.request.OrderItemRequest;

import cit.backend.model.OrderItem;
import com.fasterxml.jackson.annotation.JsonProperty;
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
    private CustomerResponse customer;
    private StaffResponse staff;
    private BigDecimal totalAmount;
    private PromotionResponse promotion;
//    @JsonProperty("is_cash")
    private Boolean isCash;

//    @JsonProperty("is_use_customer_point")    private Boolean isUseCustomerPoint;

//    @JsonProperty("point_discount")
    private BigDecimal pointDiscount;
    private List<OrderItemResponse> items;
//    private PromotionResponse promotionResponse;

} //Dung roi
