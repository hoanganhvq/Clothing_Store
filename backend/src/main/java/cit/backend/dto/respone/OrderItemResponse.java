package cit.backend.dto.respone;

import com.fasterxml.jackson.annotation.JsonIgnore;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;

@Data
@AllArgsConstructor
@NoArgsConstructor
public class OrderItemResponse {
    private ProductResponse product;
    private int productId;
    private int orderId;
    private int quantity;
    private BigDecimal productPrice;
}