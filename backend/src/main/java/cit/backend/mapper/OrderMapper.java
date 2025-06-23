package cit.backend.mapper;

import cit.backend.dto.request.OrderItemRequest;
import cit.backend.dto.request.OrderRequest;
import cit.backend.dto.respone.OrderItemResponse;
import cit.backend.dto.respone.OrderResponse;
import cit.backend.dto.respone.ProductResponse;
import cit.backend.model.Order;
import cit.backend.model.OrderItem;
import cit.backend.model.Product;
import org.mapstruct.Mapper;
import org.mapstruct.Mapping;

import java.util.List;

@Mapper(componentModel = "spring", uses = {CustomerMapper.class, StaffMapper.class, PromotionMapper.class})
public interface OrderMapper {
    @Mapping(source = "customer", target = "customerRespone") //lay tu truong customer trong Order toi customerResponse trong DTO
    @Mapping(source = "staff", target = "staffResponse")
    @Mapping(source = "promotion", target = "promotionResponse")
    OrderResponse toResponse(Order order);
    Order toProduct(OrderRequest orderRequest);
    List<OrderResponse> toResponseList(List<Order> orders);

    // Mới
    @Mapping(source = "product.name", target = "nameProduct")
    @Mapping(source = "quantity", target = "quantity")
    @Mapping(source = "productPrice", target = "price")
    @Mapping(source = "subtotal", target = "total")
    OrderItemResponse toOrderItemResponse(OrderItem orderItem);

    @Mapping(source = "orderItems", target = "orderDetails")
    List<OrderItemResponse> toOrderItemResponseList(List<OrderItem> orderItems);

}
