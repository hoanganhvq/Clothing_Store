package cit.backend.service;

import cit.backend.dto.request.OrderItemRequest;
import cit.backend.dto.request.OrderRequest;
import cit.backend.dto.respone.OrderResponse;
import cit.backend.dto.respone.PageResponse;
import cit.backend.exception.*;
import cit.backend.mapper.OrderMapper;
import cit.backend.model.*;
import cit.backend.repository.*;
import jakarta.transaction.Transactional;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.io.support.ResourcePatternResolver;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.time.LocalDateTime;
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
    private ResourcePatternResolver resourcePatternResolver;


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

        //Add diem them cho Customer
        Order savedOrder = orderRepository.save(order);
        return orderMapper.toResponse(savedOrder);

    }



    public List<OrderResponse> getCustomerOrderByDate(int customerId, LocalDateTime startDate, LocalDateTime endDate) {
        Customer customer = customerRepository.findById(customerId)
                .orElseThrow(()->new CustomerNotFoundException("Customer Not Found " + customerId));

        List<Order> orders = orderRepository.findByCustomerAndOrderDateBetween(customer, startDate, endDate);

        return  orderMapper.toResponseList(orders);

    }

    public PageResponse<OrderResponse> getOrderByDate(LocalDateTime startDate, LocalDateTime endDate, Pageable pageable) {
        Page<Order> orders = orderRepository.findByOrderDateBetween(startDate, endDate, pageable);
        PageResponse<OrderResponse> pageResponse = new PageResponse<>();
        pageResponse.setPage(orders.getNumber() + 1);
        pageResponse.setTotalPages(orders.getTotalPages());
        pageResponse.setTotalCount(orders.getTotalElements());
        pageResponse.setData(orderMapper.toResponseList(orders.getContent()));

        return pageResponse;
    }


    public OrderResponse updateOrder(OrderRequest orderRequest, int orderId) {
        Order order = orderRepository.findById(orderId)
                .orElseThrow(()->new OrderNotFoundException("Order Not Found " + orderId));

        order.setStatus(orderRequest.getStatus());
        order.setTotalAmount(orderRequest.getTotalAmount());
        order.setOrderDate(orderRequest.getOrderDate());

        Customer customer = customerRepository.findById(orderRequest.getCustomerId())
                .orElseThrow(()->new CustomerNotFoundException("Customer Not Found" + orderRequest.getCustomerId()));
        order.setCustomer(customer);

        Staff staff = staffRepository.findById(orderRequest.getStaffId())
                .orElseThrow(()->new StaffNotFoundException("Staff Not Found" + orderRequest.getStaffId()));
        order.setStaff(staff);
        //Tuy theo orderRequest co promotionId
        if (orderRequest.getPromotionId() != null) {
            Promotion promotion = promotionRepository.findById(orderRequest.getPromotionId())
                    .orElseThrow(() -> new PromotionNotFoundException("Promotion Not Found: " + orderRequest.getPromotionId()));
            order.setPromotion(promotion);
        } else {
            order.setPromotion(null);
        }

        Order savedOrder = orderRepository.save(order);
        return orderMapper.toResponse(savedOrder);
    }

    public OrderResponse deleteOrder(int orderId) {
        Order order = orderRepository.findById(orderId)
                .orElseThrow(()->new OrderNotFoundException("Order Not Found " + orderId));
        orderRepository.delete(order);
        return orderMapper.toResponse(order);
    }


    public void sendEmail(int orderId) {
        //Coding here
    }
    


}
