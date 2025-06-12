package cit.backend.model;

import jakarta.persistence.Column;
import jakarta.persistence.Embeddable;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.io.Serializable;

@Embeddable
@Data
@NoArgsConstructor
@AllArgsConstructor
public class OrderItemKey implements Serializable {
    @Column(name = "order_id")
    private int orderId;

    @Column(name = "product_id")
    private int productId;
}
