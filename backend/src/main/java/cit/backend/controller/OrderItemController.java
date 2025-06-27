package cit.backend.controller;

import cit.backend.dto.request.OrderItemRequestList;
import cit.backend.dto.respone.OrderItemResponse;
import cit.backend.exception.OrderItemNotFound;
import cit.backend.exception.OrderNotFoundException;
import cit.backend.model.OrderItem;
import cit.backend.service.OrderItemService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("orderitems")
public class OrderItemController {
    @Autowired
    private OrderItemService orderItemService;

    @PostMapping
    public ResponseEntity<List<OrderItemResponse>> createOrderItem(OrderItemRequestList orderItems) {
        try{
            return ResponseEntity.status(HttpStatus.CREATED)
                    .body(orderItemService.createOrderItems(orderItems));
        }catch(OrderItemNotFound | IllegalArgumentException e){
            return ResponseEntity.badRequest().build();
        }
    }

    @GetMapping("/{id}")
    public ResponseEntity<OrderItemResponse> getOrderItem(@PathVariable int id) {
        try{
            return ResponseEntity.status(HttpStatus.FOUND)
                    .body(orderItemService.findOrderItemById(id));
        } catch (OrderItemNotFound e){
            return ResponseEntity.notFound().build();
        }
    }


}
