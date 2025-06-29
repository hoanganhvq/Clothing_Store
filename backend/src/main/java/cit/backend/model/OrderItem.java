package cit.backend.model;

import com.fasterxml.jackson.annotation.JsonBackReference;
import com.fasterxml.jackson.annotation.JsonIgnore;
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
@Table(name = "order_items")
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
    @JsonBackReference
    private Order order;

    @ManyToOne
    @MapsId("productId") //EmbeddedID
    @JoinColumn(name = "product_id")
    @JsonIgnore
    private Product product;
}
