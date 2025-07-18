package cit.backend.service;

import cit.backend.dto.request.OrderItemRequest;
import cit.backend.dto.request.OrderItemRequestList;
import cit.backend.dto.request.OrderItemUpdateRequest;
import cit.backend.dto.request.ProductUpdateRequest;
import cit.backend.dto.respone.OrderItemResponse;
import cit.backend.dto.respone.OrderResponse;
import cit.backend.exception.OrderItemNotFound;
import cit.backend.exception.OrderNotFoundException;
import cit.backend.exception.ProductNotFoundException;
import cit.backend.mapper.OrderItemMapper;
import cit.backend.model.Order;
import cit.backend.model.OrderItem;
import cit.backend.model.Product;
import cit.backend.repository.OrderItemRepository;
import cit.backend.repository.OrderRepository;
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
    private ProductRepository productRepository;

    @Autowired
    private OrderItemMapper orderItemMapper;
    @Autowired
    private OrderRepository orderRepository;
    @Autowired
    private ProductService productService;

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
            ProductUpdateRequest productUpdateRequest = new ProductUpdateRequest();
            productUpdateRequest.setStockQuantity(currentStock - orderQuantity);
            productService.updateProduct(product.getId(), productUpdateRequest);

            // Gán thông tin từ Product vào DTO
            orderItem.setProductPrice(product.getPrice());
            orderItem.setQuantity(orderQuantity);
            orderItem.setOrderId(orderItem.getOrderId());
            orderItem.setProductId(product.getId());

            // Convert sang Entity và lưu
            Order order = orderRepository.findById(orderItem.getOrderId())
                    .orElseThrow(() -> new OrderNotFoundException("Order not found"));

            OrderItem entity = orderItemMapper.toEntity(orderItem);
            entity.setOrder(order);
            entity.setProduct(product);
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

    public OrderItemResponse findOrderItemByProductId(int id) {
        OrderItem orderItem =  orderItemRepository.findById(id)
                .orElseThrow(()->new OrderItemNotFound("Order Item not found"));
        return orderItemMapper.toResponse(orderItem);
    }

    public OrderItemResponse updateOrderItemById(int id, OrderItemUpdateRequest orderItemRequest) {
        OrderItem orderItem = orderItemRepository.findById(id)
                .orElseThrow(()->new OrderItemNotFound("Order Item not found"));

        if(orderItemRequest.getOrderId() != null) {
            Order order = orderRepository.findById(orderItemRequest.getOrderId())
                    .orElseThrow(()->new OrderNotFoundException("Order Item not found"));

            orderItem.setOrder(order);
        }

        if(orderItemRequest.getProductId() != null) {
            Product product = productRepository.findById(orderItemRequest.getProductId())
                    .orElseThrow(()->new ProductNotFoundException("Product not found"));
            orderItem.setProduct(product);
        }

        if(orderItemRequest.getQuantity() != null) {
            orderItem.setQuantity(orderItemRequest.getQuantity());
        }

        if(orderItemRequest.getProductPrice() != null) {
            orderItem.setProductPrice(orderItemRequest.getProductPrice());
        }

        orderItemRepository.save(orderItem);
        return orderItemMapper.toResponse(orderItem);

    }

    public OrderItemResponse deleteOrderItemById(int id) {
        OrderItem orderItem = orderItemRepository.findById(id)
                .orElseThrow(()->new OrderItemNotFound("Order Item Not  FOund"));
        orderItemRepository.delete(orderItem);
        return orderItemMapper.toResponse(orderItem);
    }



}
