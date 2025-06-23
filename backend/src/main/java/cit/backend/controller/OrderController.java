package cit.backend.controller;

import cit.backend.dto.request.OrderRequest;
import cit.backend.dto.respone.OrderResponse;
import cit.backend.exception.*;
import cit.backend.service.OrderService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.time.LocalDateTime;
import java.util.List;

@RestController
@RequestMapping("order")
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

//    @GetMapping("")
//    public ResponseEntity<List<OrderResponse>> getCustomerOrderByDate(
//            @RequestParam int customerId,
//            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDateTime startDate,
//            @RequestParam @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDateTime endDate){
//        try{
//
//        }catch (){
//
//        }
//    }
    
}
