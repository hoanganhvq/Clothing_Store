package cit.backend.service;

import cit.backend.dto.request.OrderItemRequest;
import cit.backend.dto.request.OrderItemRequestList;
import cit.backend.dto.respone.OrderItemResponse;
import cit.backend.dto.respone.OrderResponse;
import cit.backend.exception.OrderItemNotFound;
import cit.backend.exception.ProductNotFoundException;
import cit.backend.mapper.OrderItemMapper;
import cit.backend.model.OrderItem;
import cit.backend.model.Product;
import cit.backend.repository.OrderItemRepository;
import cit.backend.repository.ProductRepository;
import jakarta.transaction.Transactional;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.List;

@Service
public class OrderItemService {
    @Autowired
    private OrderItemRepository orderItemRepository;

    @Autowired
    private ProductService productService;

    @Autowired
    private ProductRepository productRepository;

    @Autowired
    private OrderItemMapper orderItemMapper;

    @Transactional
    public List<OrderItemResponse> createOrderItems(OrderItemRequestList orderItems) {
        List<OrderItemResponse> result = new ArrayList<>();

        for (OrderItemRequest orderItem : orderItems.getOrderItems()) {
            Product product = productRepository.findById(orderItem.getProductId())
                    .orElseThrow(() -> new ProductNotFoundException("Product not found"));

            int currentStock = product.getStockQuantity();
            int orderQuantity = orderItem.getQuantity();

            if (orderQuantity > currentStock) {
                throw new IllegalArgumentException("Không đủ hàng tồn kho cho sản phẩm: " + product.getName());
            }

            // Giảm tồn kho và lưu lại
            product.setStockQuantity(currentStock - orderQuantity);
            productRepository.save(product);

            // Gán thông tin từ Product vào DTO
            orderItem.setProductPrice(product.getPrice());
            orderItem.setQuantity(orderQuantity);
            orderItem.setOrderId(orderItem.getOrderId());
            orderItem.setProductId(product.getId());

            // Convert sang Entity và lưu
            OrderItem entity = orderItemMapper.toEntity(orderItem);
            OrderItem saved = orderItemRepository.save(entity);

            // Convert sang DTO để trả về
            result.add(orderItemMapper.toResponse(saved));
        }
        return result;
    }


    public OrderItemResponse findOrderItemById(int id) {
        OrderItem orderItem = orderItemRepository.findById(id)
                .orElseThrow(()->new OrderItemNotFound("Order Item not found"));

        return orderItemMapper.toResponse(orderItem);
    }



}
