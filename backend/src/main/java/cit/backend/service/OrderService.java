package cit.backend.service;

import cit.backend.dto.request.OrderItemRequest;
import cit.backend.dto.request.OrderRequest;
import cit.backend.dto.respone.OrderResponse;
import cit.backend.exception.*;
import cit.backend.mapper.OrderMapper;
import cit.backend.model.*;
import cit.backend.repository.*;
import jakarta.transaction.Transactional;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

@Service
public class OrderService {
    @Autowired
    private OrderRepository orderRepository;

    @Autowired
    private CustomerRepository customerRepository;

    @Autowired
    private PromotionRepository promotionRepository;

    @Autowired
    private StaffRepository staffRepository;

    @Autowired
    private OrderMapper orderMapper;

    @Autowired
    private ProductRepository productRepository;
    @Autowired
    private OrderItemRepository orderItemRepository;


    public List<OrderResponse> getAllOrders() {
        return orderMapper.toResponseList(orderRepository.findAll());
    }


    public OrderResponse getOrderById(int id) {
        Order order = orderRepository.findById(id)
                .orElseThrow(()->new OrderNotFoundException("Order Not Found " + id));
        return orderMapper.toResponse(order);
    }

    @Transactional
    public OrderResponse addOrder(OrderRequest orderRequest) {
        Order order = new Order();
        order.setOrderDate(orderRequest.getOrderDate());
        order.setStatus(orderRequest.getStatus());
        order.setTotalAmount(orderRequest.getTotalAmount());

        Customer customer = customerRepository.findById(orderRequest.getCustomerId())
                .orElseThrow(()->new CustomerNotFoundException("Customer Not found" + orderRequest.getCustomerId()));

        Staff staff = staffRepository.findById(orderRequest.getStaffId())
                .orElseThrow(()-> new StaffNotFoundException("Staff Not Found" + orderRequest.getStaffId()));

        Promotion promotion = null;
        if(orderRequest.getPromotionId() != null){
             promotion = promotionRepository.findById(orderRequest.getPromotionId())
                    .orElseThrow(()->new PromotionNotFoundException("Promotion Not Found" + orderRequest.getPromotionId()));
        }

        order.setCustomer(customer);
        order.setStaff(staff);
        order.setPromotion(promotion);


        Order savedOrder = orderRepository.save(order);
        List<OrderItem> orderItems = new ArrayList<>();

        for(OrderItemRequest itemRequest: orderRequest.getOrderItems()){
            Product product = productRepository.findById(itemRequest.getProductId())
                    .orElseThrow(()->new ProductNotFoundException("Product Not Found" + itemRequest.getProductId()));

            OrderItemKey key = new OrderItemKey(savedOrder.getId(), product.getId());

            OrderItem item = new OrderItem();
            int quantity = itemRequest.getQuantity();
            if(quantity < 0){
                throw  new IllegalArgumentException("Quantity must be greatr than 0 for product ID" + itemRequest.getProductId());
            }
            BigDecimal price = product.getPrice();
            BigDecimal subTotal = price.multiply(BigDecimal.valueOf(quantity));

            //Xem xet bo truong productPrice
            item.setId(key);
            item.setQuantity(quantity);
            item.setSubtotal(subTotal);
            item.setProductPrice(itemRequest.getProductPrice());
            item.setOrder(savedOrder);

            item.setProduct(product);
            orderItems.add(item);
        }

        orderItemRepository.saveAll(orderItems);
        savedOrder.setOrderItemList(orderItems);
        return orderMapper.toResponse(savedOrder);

    }

//    public OrderResponse updateOrder(int id, OrderRequest orderRequest) {
//
//    }


}
