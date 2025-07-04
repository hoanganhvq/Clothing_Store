package cit.backend.controller;

import cit.backend.dto.request.CustomerRequest;
import cit.backend.dto.request.CustomerUpdateRequest;
import cit.backend.dto.respone.CustomerResponse;
import cit.backend.dto.respone.PageResponse;
import cit.backend.exception.CustomerNotFoundException;
import cit.backend.model.Customer;
import cit.backend.service.CustomerService;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.*;

@RestController
@Validated
@RequestMapping("customer")
public class CustomerController {
    @Autowired
    private CustomerService customerService;

    @GetMapping()
    public ResponseEntity<PageResponse<CustomerResponse>> getAllCustomers(
            @RequestParam(value = "page", required = false,defaultValue = "1") Integer page,
            @RequestParam(value = "search", required = false, defaultValue = "") String search
    ){

            Pageable pageRequest = PageRequest.of(page - 1, 5);
            return ResponseEntity.ok(customerService.getAllCustomers(pageRequest, search));
    }


    @GetMapping("/{id}")
    public ResponseEntity<CustomerResponse> getCustomerById(@PathVariable int id){
            return ResponseEntity.ok(customerService.getCustomerById(id));
    }

    @PatchMapping("/{id}")
    public ResponseEntity<CustomerResponse> updateCustomer(
            @PathVariable String id,
            @RequestBody CustomerUpdateRequest customerRequest){
        int formattedId = Integer.parseInt(id);
        return ResponseEntity.ok(customerService.updateCustomer(formattedId, customerRequest));
    }

    @PostMapping
    public ResponseEntity<CustomerResponse> createCustomer(
            @Valid @RequestBody CustomerRequest customerRequest){
            return ResponseEntity.ok(customerService.addCustomer(customerRequest));

    }

    @DeleteMapping("/{id}")
    public ResponseEntity<CustomerResponse> deleteCustomer(@PathVariable String id){
            int formattedId = Integer.parseInt(id);
            return ResponseEntity.ok(customerService.deleteCustomer(formattedId));
    }


}
