package cit.backend.service;

import cit.backend.dto.request.CustomerUpdateRequest;
import cit.backend.dto.request.OrderRequest;
import cit.backend.dto.request.OrderUpdateRequest;
import cit.backend.dto.respone.OrderResponse;
import cit.backend.dto.respone.PageResponse;
import cit.backend.exception.*;
import cit.backend.mapper.OrderMapper;
import cit.backend.model.*;
import cit.backend.repository.*;
import jakarta.mail.MessagingException;
import jakarta.transaction.Transactional;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.domain.Specification;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
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
    private CustomerService customerService;

    @Autowired
    private EmailService emailService;


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
        order.setSubTotal(orderRequest.getSubTotal());
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
            Specification<Order> searchSpec = (root, query, cb) ->cb.equal(root.get("customer").get("id"), search);

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




   public PageResponse<OrderResponse> getCustomerOrder(
           Integer customerId,
           Pageable pageable
   ){
        Customer customer = customerRepository.findById(customerId)
                .orElseThrow(()->new CustomerNotFoundException("Customer not found"));
        Page<Order> orderPage = orderRepository.findAllByCustomer(customer, pageable);

       List<OrderResponse> data = orderPage.getContent().stream()
               .map(orderMapper::toResponse)
               .toList();

       PageResponse<OrderResponse> pageResponse = new PageResponse<>();
       pageResponse.setData(data);
       pageResponse.setTotalPages(orderPage.getTotalPages());
       pageResponse.setTotalCount(orderPage.getTotalElements());
       pageResponse.setPage(orderPage.getNumber() + 1);

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


    public String sendEmail(int orderId) {
        System.out.println("Send email Order");
        Order order = orderRepository.findById(orderId)
                .orElseThrow(() -> new OrderNotFoundException("Order not found: " + orderId));

        String customerEmail = order.getCustomer().getEmail();
        if (customerEmail == null) {
            throw new CustomerEmailNotFound("Customer's Email not found");
        }

        String subject = "Thank you for your order #" + orderId;

        StringBuilder content = new StringBuilder();
        content.append(String.format(
                "<h1>Thank you for your order!</h1>" +
                        "<p>Dear %s,</p>" +
                        "<p><strong>Order Date:</strong> %s</p>" +
                        "<p><strong>Processed by staff:</strong> %s</p>" +
                        "<p>Below are the details of your order:</p>" +
                        "<table border='1' cellpadding='5' cellspacing='0'>" +
                        "<thead><tr><th>Product</th><th>Quantity</th><th>Price</th></tr></thead><tbody>",
                order.getCustomer().getName(),
                order.getOrderDate().format(DateTimeFormatter.ofPattern("dd/MM/yyyy HH:mm:ss")),
                order.getStaff().getUsername()
        ));

        for (OrderItem item : order.getItems()) {
            content.append(String.format(
                    "<tr><td>%s</td><td>%d</td><td>%,.0f₫</td></tr>",
                    item.getProduct().getName(),
                    item.getQuantity(),
                    item.getProductPrice()

            ));
            BigDecimal totalAmount = item.getProductPrice().multiply(new BigDecimal(item.getQuantity()));
        }

        content.append("</tbody></table>");

        BigDecimal promotionDiscount = BigDecimal.ZERO;
        System.out.println("promotion Discount "+order.getPromotion().getValue());
        System.out.println("Total Amout "+ order.getTotalAmount());
        if (order.getPromotion() != null) {
            promotionDiscount = order.getSubTotal().multiply(order.getPromotion().getValue());
        }


        BigDecimal pointDiscount = order.getIsUseCustomerPoint()
                ? order.getPointDiscount()
                : BigDecimal.ZERO;




        content.append(String.format("<p><strong>Original Total:</strong> %,.0f₫</p>",order.getSubTotal()));

        if (promotionDiscount.compareTo(BigDecimal.ZERO) > 0) {
            content.append(String.format("<p><strong>Promotion Discount:</strong> -%,.0f₫ (%s)</p>",
                    promotionDiscount, order.getPromotion().getName()));
        }

        if (pointDiscount.compareTo(BigDecimal.ZERO) > 0) {
            content.append(String.format("<p><strong>Point Discount:</strong> -%,.0f₫</p>", pointDiscount));
        }

        content.append(String.format(
                "<p><strong>Final Total:</strong> <span style='color:green'>%,.0f₫</span></p>",
                order.getTotalAmount()
        ));

        content.append("<p>We hope to serve you again soon!</p>");

        try {
            emailService.sendEmail(customerEmail, subject, content.toString());
        } catch (MessagingException e) {
            e.printStackTrace();
            return "Fail to send email.";
        }

        return "Email sent successfully to " + customerEmail;
    }



}
