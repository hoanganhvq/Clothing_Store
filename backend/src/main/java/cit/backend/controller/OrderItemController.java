package cit.backend.controller;

import cit.backend.dto.request.OrderItemRequest;
import cit.backend.dto.request.OrderItemRequestList;
import cit.backend.dto.request.OrderItemUpdateRequest;
import cit.backend.dto.respone.OrderItemResponse;
import cit.backend.exception.OrderItemNotFound;
import cit.backend.exception.OrderNotFoundException;
import cit.backend.model.OrderItem;
import cit.backend.service.OrderItemService;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("order-detail")
@Validated
public class OrderItemController {
    @Autowired
    private OrderItemService orderItemService;

    @GetMapping
    public ResponseEntity<List<OrderItemResponse>> getAllOrderItems() {
            return ResponseEntity.ok(orderItemService.getOrderItems());
    }

    @PostMapping
    public ResponseEntity<List<OrderItemResponse>> createOrderItem(
            @Valid @RequestBody OrderItemRequestList orderItems) {
            return ResponseEntity.status(HttpStatus.CREATED)
                    .body(orderItemService.createOrderItems(orderItems));
    }

    @GetMapping("/{id}")
    public ResponseEntity<OrderItemResponse> getOrderItem(@PathVariable int id) {
            return ResponseEntity.status(HttpStatus.FOUND)
                    .body(orderItemService.findOrderItemById(id));

    }


    @PatchMapping("/{id}")
    public ResponseEntity<OrderItemResponse> updateOrderItem(
            @PathVariable int id,
            @RequestBody OrderItemUpdateRequest orderItems) {
        return ResponseEntity.ok(orderItemService.updateOrderItemById(id, orderItems));
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<OrderItemResponse> deleteOrderItem(@PathVariable int id) {
        return  ResponseEntity.ok(orderItemService.deleteOrderItemById(id));
    }


}
