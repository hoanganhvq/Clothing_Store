package cit.backend.controller;

import cit.backend.dto.request.OrderRequest;
import cit.backend.dto.request.OrderUpdateRequest;
import cit.backend.dto.respone.OrderResponse;
import cit.backend.dto.respone.PageResponse;
import cit.backend.exception.*;
import cit.backend.service.OrderService;
import jakarta.validation.Valid;
import org.hibernate.annotations.Parameter;
import org.springframework.data.domain.Page;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.format.annotation.DateTimeFormat;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.*;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;

@RestController
@RequestMapping("order")
@Validated
public class OrderController {
    @Autowired
    private OrderService orderService;

//    @GetMapping
//    public List<OrderResponse> getAllOrders() {
//            return orderService.getAllOrders();
//    }

    @GetMapping("/{id}")
    public ResponseEntity<OrderResponse> getOrderById(@PathVariable int id){
            return ResponseEntity.ok(orderService.getOrderById(id));
    }

    @PostMapping
    public ResponseEntity<OrderResponse> createOrder(
            @Valid @RequestBody OrderRequest orderRequest) {
            return ResponseEntity.status(HttpStatus.CREATED)
                    .body(orderService.addOrder(orderRequest));
    }
    //--
    @GetMapping()
    public ResponseEntity<PageResponse<OrderResponse>> getAllOrders(
            @RequestParam(value = "page", required = false) Integer page,
            @RequestParam(value = "search", defaultValue = "", required = false) Integer id ,
            @RequestParam(value = "startDate", required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate startDate,
            @RequestParam(value = "endDate", required = false) @DateTimeFormat(iso = DateTimeFormat.ISO.DATE) LocalDate endDate
    ) {
            Pageable pageable = PageRequest.of(page - 1, 5);
            PageResponse<OrderResponse> orders = orderService.getAllOrders(id, startDate, endDate, pageable);
            return ResponseEntity.ok(orders);
    }

    // /order/by-date?startDate=...&endDate=...&page=1
//    @GetMapping("/by-date")
//    public ResponseEntity<PageResponse<OrderResponse>> getOrderByDate(
//            @RequestParam("startDate") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) LocalDateTime startDate,
//            @RequestParam("endDate") @DateTimeFormat(iso = DateTimeFormat.ISO.DATE_TIME) LocalDateTime endDate,
//            @RequestParam(value = "page", defaultValue = "1") String page
//    ) {
//
//            int pageNumber = Integer.parseInt(page);
//            Pageable pageable = PageRequest.of(pageNumber - 1, 10);
//            PageResponse<OrderResponse> orders = orderService.getOrderByDate(startDate, endDate, pageable);
//            return ResponseEntity.ok(orders);
//
//    }


    @PatchMapping("/{orderId}")
    public ResponseEntity<OrderResponse> updateOrder(
            @PathVariable int orderId,
            @RequestBody OrderUpdateRequest orderRequest) {

            OrderResponse orderResponse = orderService.updateOrder(orderRequest, orderId);
            return ResponseEntity.ok(orderResponse);
    }

    @DeleteMapping("/{orderId}")
    public ResponseEntity<OrderResponse> deleteOrder(
            @PathVariable int orderId
    ){

            OrderResponse orderResponse = orderService.deleteOrder(orderId);
            return ResponseEntity.ok(orderResponse);
    }

    @PutMapping("/{id}/send-email")
    public void sendEmail(
        @PathVariable int id
    ){
        orderService.sendEmail(id);
    }
}
