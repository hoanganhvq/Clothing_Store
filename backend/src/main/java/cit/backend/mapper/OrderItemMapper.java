package cit.backend.mapper;

import cit.backend.dto.request.OrderItemRequest;
import cit.backend.dto.respone.OrderItemResponse;
import cit.backend.model.OrderItem;
import org.mapstruct.Mapper;
import org.mapstruct.Mapping;

import java.util.List;

@Mapper(componentModel = "spring", uses = {ProductMapper.class})
public interface OrderItemMapper {

    @Mapping(source = "product", target = "product")
    OrderItemResponse toResponse(OrderItem item);

    @Mapping(target = "id.orderId", source = "orderId")
    @Mapping(target = "id.productId", source = "productId")
    @Mapping(target = "order", ignore = true)
    @Mapping(target = "product", ignore = true)
    OrderItem toEntity(OrderItemRequest request);

    List<OrderItemResponse> toDtoList(List<OrderItem> items);
}


