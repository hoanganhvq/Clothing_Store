package cit.backend.mapper;

import cit.backend.dto.request.OrderRequest;
import cit.backend.dto.respone.OrderResponse;
import cit.backend.dto.respone.ProductResponse;
import cit.backend.model.Order;
import cit.backend.model.Product;
import org.mapstruct.Mapper;

import java.util.List;

@Mapper(componentModel = "spring")
public interface OrderMapper {
    OrderResponse toResponse(Order order);
    Order toProduct(OrderRequest orderRequest);
    List<OrderResponse> toResponseList(List<Order> orders);
}
