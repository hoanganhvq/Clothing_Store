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

@Mapper(componentModel = "spring", uses = {OrderItemMapper.class, CustomerMapper.class, StaffMapper.class})
public interface OrderMapper {

    @Mapping(source = "id", target = "orderId")
    @Mapping(source = "items", target = "items") // mapping List<OrderItem> -> List<OrderItemResponse>
    OrderResponse toResponse(Order order);

    @Mapping(source = "customerId", target = "customer.id")
    @Mapping(source = "staffId", target = "staff.id")
    @Mapping(target = "promotion", ignore = true)
    @Mapping(target = "status", ignore = true)
    @Mapping(target = "items", ignore = true) // xử lý riêng nếu cần
    @Mapping(target = "id", ignore = true) // khi tạo mới
    Order toEntity(OrderRequest orderRequest);

    List<OrderResponse> toResponseList(List<Order> orders);
}


