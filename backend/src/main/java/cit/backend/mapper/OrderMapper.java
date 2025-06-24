package cit.backend.mapper;

import cit.backend.dto.request.OrderRequest;
import cit.backend.dto.respone.OrderResponse;
import cit.backend.dto.respone.ProductResponse;
import cit.backend.dto.respone.PromotionResponse;
import cit.backend.model.Order;
import cit.backend.model.Product;
import org.mapstruct.Mapper;
import org.mapstruct.Mapping;

import java.util.List;

@Mapper(componentModel = "spring", uses = {OrderItemMapper.class})
public interface OrderMapper {

    @Mapping(source = "id", target = "orderId")
    @Mapping(source = "customer.id", target = "customerId")
    @Mapping(source = "staff.id", target = "staffId")
    OrderResponse toResponse(Order order);

    @Mapping(source = "customerId", target = "customer.id")
    @Mapping(source = "staffId", target = "staff.id")
    @Mapping(target = "status", ignore = true) // có thể set default trong service
    @Mapping(target = "orderItems", ignore = true) // xử lý thủ công nếu cần
    Order toEntity(OrderRequest orderRequest);

    List<OrderResponse> toResponseList(List<Order> orders);
}

