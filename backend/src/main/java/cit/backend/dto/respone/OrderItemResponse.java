package cit.backend.dto.respone;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;

@Data
@AllArgsConstructor
@NoArgsConstructor
public class OrderItemResponse {

    private ProductResponse product;
    private int quantity;
    private BigDecimal productPrice;
}