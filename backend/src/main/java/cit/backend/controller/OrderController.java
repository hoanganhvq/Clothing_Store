package cit.backend.controller;

import cit.backend.dto.request.OrderRequest;
import cit.backend.dto.respone.OrderResponse;
import cit.backend.exception.*;
import cit.backend.service.OrderService;
import org.springframework.data.domain.Page;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.time.LocalDateTime;
import java.util.List;

@RestController
@RequestMapping("api/orders")
public class OrderController {
    @Autowired
    private OrderService orderService;

    @GetMapping
    public List<OrderResponse> getAllOrders() {
        try{
            return orderService.getAllOrders();
        }catch(Exception e) {
            return null;
        }
    }

    @GetMapping("/{id}")
    public ResponseEntity<OrderResponse> getOrderById(@PathVariable int id){
        try{
            return ResponseEntity.ok(orderService.getOrderById(id));
        }catch(OrderNotFoundException e){
            return ResponseEntity.notFound().build();
        }
    }

    @PostMapping
    public ResponseEntity<OrderResponse> createOrder(@RequestBody OrderRequest orderRequest) {
        try {
            return ResponseEntity.status(HttpStatus.CREATED)
                    .body(orderService.addOrder(orderRequest));
        } catch (CustomerNotFoundException |
                 StaffNotFoundException |
                 PromotionNotFoundException |
                 ProductNotFoundException e) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).body(null);
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(null); // HTTP 400 nếu dữ liệu sai
        }
    }

    @GetMapping
    public ResponseEntity<List<OrderResponse>> getCustomerOrdersByDate(
            @RequestParam("search") int customerId,
            @RequestParam("startDate") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) LocalDateTime startDate,
            @RequestParam("endDate") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) LocalDateTime endDate
    ) {
        try{
            List<OrderResponse> orders = orderService.getCustomerOrderByDate(customerId, startDate, endDate);
            return ResponseEntity.ok(orders);
        }catch(CustomerNotFoundException e){
            return ResponseEntity.notFound().build();
        }

    }

    @GetMapping
    public ResponseEntity<org.springframework.data.domain.Page<OrderResponse>> getOrderByDate(
            @RequestParam("startDate") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) LocalDateTime startDate,
            @RequestParam("endDate") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) LocalDateTime endDate,
            @RequestParam(value = "page", defaultValue = "1") int page
    ){
        try{
            Pageable pageable = PageRequest.of(page - 1, 10);
            Page<OrderResponse> orders = orderService.getOrderByDate(startDate, endDate,pageable);
            return ResponseEntity.ok(orders);
        }catch(CustomerNotFoundException e){
            return ResponseEntity.notFound().build();
        }
    }


    @PutMapping("/{orderId}")
    public ResponseEntity<OrderResponse> updateOrder(
            @PathVariable int orderId,
            @RequestBody OrderRequest orderRequest) {
        try{
            OrderResponse orderResponse = orderService.updateOrder(orderRequest, orderId);
            return ResponseEntity.ok(orderResponse);
        }catch(OrderNotFoundException  | CustomerNotFoundException | StaffNotFoundException e
        ){
            return ResponseEntity.notFound().build();
        }

    }

    @DeleteMapping("/{orderId}")
    public ResponseEntity<OrderResponse> deleteOrder(
            @PathVariable int orderId
    ){
        OrderResponse orderResponse = orderService.deleteOrder(orderId);
        return ResponseEntity.ok(orderResponse);
    }

    
}
