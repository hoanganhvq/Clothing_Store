package cit.backend.dto.respone;

import lombok.Data;
import java.math.BigDecimal;

@Data
public class OrderItemResponse {
    private int productId;
    private String nameProduct; // Thay productName thành nameProduct để khớp với Report
    private int quantity;
    private BigDecimal price; // Thay productPrice thành price
    private BigDecimal total; // Thay subtotal thành total
}
