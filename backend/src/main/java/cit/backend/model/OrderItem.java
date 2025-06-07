package cit.backend.model;

import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;
import java.util.List;


@Entity
@Data
@NoArgsConstructor
@AllArgsConstructor
@Table(name = "orderItems")
public class OrderItem {
    @EmbeddedId
    private OrderItemKey id;

    @Column(name = "quantity")
    private int quantity;

    @Column(name = "subTotal")
    private BigDecimal subtotal;

    @Column(name = "productPrice")
    private BigDecimal productPrice;

    @ManyToOne
    @MapsId("orderId")
    @JoinColumn(name = "order_id")
    private Order order;

    @ManyToOne
    @MapsId("productId") //EmbeddedID
    @JoinColumn(name = "product_id")
    private Product product;

}
