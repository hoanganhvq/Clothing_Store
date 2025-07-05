package cit.backend.service;

import cit.backend.dto.request.CustomerUpdateRequest;
import cit.backend.dto.request.OrderItemRequest;
import cit.backend.dto.request.OrderRequest;
import cit.backend.dto.request.OrderUpdateRequest;
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
import org.springframework.data.jpa.domain.Specification;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

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
    private CustomerService customerService;

    @Autowired
    private PointDiscountService pointDiscountService;

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
        order.setIsCash(orderRequest.getIsCash());
        order.setPointDiscount(orderRequest.getPointDiscount());
        order.setPointDiscount(orderRequest.getPointDiscount());
        order.setIsUseCustomerPoint(orderRequest.getIsUseCustomerPoint());


        Customer customer = customerRepository.findById(orderRequest.getCustomerId())
                .orElseThrow(()->new CustomerNotFoundException("Customer Not found" + orderRequest.getCustomerId()));

        Staff staff = staffRepository.findById(orderRequest.getStaffId())
                .orElseThrow(()-> new StaffNotFoundException("Staff Not Found" + orderRequest.getStaffId()));

        Promotion promotion = null;
        if (orderRequest.getPromotionId() != null && orderRequest.getPromotionId() > 0 ) {
            promotion = promotionRepository.findById(orderRequest.getPromotionId())
                    .orElseThrow(() -> new PromotionNotFoundException("Promotion Not Found " + orderRequest.getPromotionId()));

        }



        order.setCustomer(customer);
        order.setStaff(staff);
        order.setPromotion(promotion);



        //Cong diem
        BigDecimal pointPerAmount = BigDecimal.valueOf(10_000); // mỗi 10k VNĐ
        int pointPerUnit = 1000; // được 1000 điểm

        BigDecimal totalAmount = orderRequest.getTotalAmount();

        //Neu kh su dung diem thi cong them
        if (!orderRequest.getIsUseCustomerPoint()) { // nếu đơn hàng ≥ 200k

            int unitCount = totalAmount.divide(pointPerAmount, RoundingMode.FLOOR).intValue();
            int addedPoint = unitCount * pointPerUnit;

            if (addedPoint > 0) {
                CustomerUpdateRequest updatePointRequest = new CustomerUpdateRequest();
                updatePointRequest.setPoint(customer.getPoint() + addedPoint);
                customerService.updateCustomer(customer.getId(), updatePointRequest);
            }


        } else{ //Neu su dung diem thi diem ve 0
//            BigDecimal discount = pointDiscountService.calculateDiscount(totalAmount, customer.getPoint());
//            order.setPointDiscount(discount);
            CustomerUpdateRequest request = new CustomerUpdateRequest();
            request.setPoint(0);
            customerService.updateCustomer(customer.getId(), request);
        }

//

        Order savedOrder = orderRepository.save(order);


        return orderMapper.toResponse(savedOrder);
    }



    public PageResponse<OrderResponse> getAllOrders(
            Integer search,
            LocalDate startDate,
            LocalDate endDate,
            Pageable pageable
    ) {

        Specification<Order> spec = null;

        if (search != null) {
            Specification<Order> searchSpec = (root, query, cb) -> cb.or(
                    cb.equal(root.get("id"), search),
                    cb.equal(root.get("customer").get("id"), search)
            );
            spec = (spec == null) ? searchSpec : spec.and(searchSpec);
            System.out.println("Search không null");
        }

        if (startDate != null && endDate != null) {
            Specification<Order> dateSpec = (root, query, cb) ->
                    cb.between(root.get("orderDate"), startDate, endDate);
            spec = (spec == null) ? dateSpec : spec.and(dateSpec);
        }


        Page<Order> orderPage = orderRepository.findAll(spec, pageable);

        List<OrderResponse> data = orderPage.getContent().stream()
                .map(orderMapper::toResponse)
                .toList();

        PageResponse<OrderResponse> pageResponse = new PageResponse<>();
        pageResponse.setData(data);
        pageResponse.setTotalPages(orderPage.getTotalPages());
        pageResponse.setTotalCount(orderPage.getTotalElements());
        pageResponse.setPage(orderPage.getNumber() + 1);

        System.out.println("page: " + pageResponse.getPage());
        System.out.println("in ra: " + pageResponse.getData().size() + " orders");

        return pageResponse;
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


    public OrderResponse updateOrder(OrderUpdateRequest orderRequest, int orderId) {
        Order order = orderRepository.findById(orderId)
                .orElseThrow(()->new OrderNotFoundException("Order Not Found " + orderId));

        if(orderRequest.getTotalAmount() == null ) order.setTotalAmount(orderRequest.getTotalAmount());
        if(orderRequest.getCustomerId() != null){
            Customer customer = customerRepository.findById(orderRequest.getCustomerId())
                    .orElseThrow(()->new CustomerNotFoundException("Customer Not Found" + orderRequest.getCustomerId()));
            order.setCustomer(customer);
        }

        if(orderRequest.getStaffId() != null){
            Staff staff = staffRepository.findById(orderRequest.getStaffId())
                    .orElseThrow(()->new StaffNotFoundException("Staff Not Found" + orderRequest.getStaffId()));
            order.setStaff(staff);
        }

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
