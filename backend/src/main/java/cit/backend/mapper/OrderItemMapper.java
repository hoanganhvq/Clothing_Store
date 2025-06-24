package cit.backend.mapper;

import cit.backend.dto.request.OrderItemRequest;
import cit.backend.dto.respone.OrderItemResponse;
import cit.backend.model.OrderItem;
import org.mapstruct.Mapper;
import org.mapstruct.Mapping;

import java.util.List;

@Mapper(componentModel = "spring")
public interface OrderItemMapper {

    @Mapping(source = "product.name", target = "productName")
    @Mapping(source = "id.product", target = "productId")
    @Mapping(source = "id.orderId", target = "orderId")
    OrderItemResponse toResponse(OrderItem item);

    @Mapping(target = "id.orderId", source = "orderId")
    @Mapping(target = "id.productId", source = "productId")
    @Mapping(target = "order", ignore = true)   // set trong service
    @Mapping(target = "product", ignore = true)
    OrderItem toEntity(OrderItemRequest request);

    List<OrderItemResponse> toDtoList(List<OrderItem> items);
}

