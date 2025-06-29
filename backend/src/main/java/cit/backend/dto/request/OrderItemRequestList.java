package cit.backend.dto.request;

import cit.backend.model.OrderItem;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.List;

@Data
@AllArgsConstructor
@NoArgsConstructor
public class OrderItemRequestList {
    private List<OrderItemRequest> orderItems;

}
