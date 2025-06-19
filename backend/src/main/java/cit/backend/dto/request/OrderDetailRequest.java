package cit.backend.dto.request;

import cit.backend.model.Order;
import cit.backend.model.OrderItemKey;
import cit.backend.model.Product;
import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;

@Data
@NoArgsConstructor
@AllArgsConstructor
public class OrderDetailRequest {

    private int quantity;

    private BigDecimal subtotal;

    private BigDecimal productPrice;

    private int orderId;

    private int productId;
}