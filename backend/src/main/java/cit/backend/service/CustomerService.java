package cit.backend.service;

import cit.backend.dto.request.CustomerRequest;
import cit.backend.dto.request.CustomerUpdateRequest;
import cit.backend.dto.respone.CustomerResponse;
import cit.backend.dto.respone.PageResponse;
import cit.backend.exception.CustomerNotFoundException;
import cit.backend.exception.DuplicateResourceException;
import cit.backend.mapper.CustomerMapper;
import cit.backend.model.Customer;
import cit.backend.repository.CustomerRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class CustomerService {
    @Autowired
    private CustomerRepository customerRepository;

    @Autowired
    private CustomerMapper customerMapper;

    public PageResponse<CustomerResponse> getAllCustomers(Pageable pageable, String search) {
        Page<Customer> pageCustomers = customerRepository.findByNameContainingIgnoreCase(search, pageable);

        List<CustomerResponse> content = pageCustomers.getContent()
                .stream()
                .map(customerMapper::toResponse)
                .toList();

        PageResponse<CustomerResponse> pageResponse = new PageResponse<>();
        pageResponse.setData(content);
        pageResponse.setTotalPages(pageCustomers.getTotalPages());
        pageResponse.setPage(pageCustomers.getNumber() + 1);
        pageResponse.setTotalCount(pageCustomers.getTotalElements());
        return pageResponse;
    }

    public CustomerResponse getCustomerById(Integer id) {
        Customer customer = customerRepository.findById(id)
                .orElseThrow(()-> new CustomerNotFoundException("Customer with id " + id + " not found"));
        return customerMapper.toResponse(customer);
    }
    //------------------------------------------------------

    public CustomerResponse updateCustomer(int id, CustomerUpdateRequest customerRequest) {
        Customer customer = customerRepository.findById(id)
                .orElseThrow(()-> new CustomerNotFoundException("Customer with id " + id + " not found"));

        if(customerRequest.getEmail() != null) {customer.setEmail(customerRequest.getEmail());}
        if(customerRequest.getPoint() != null) {customer.setPoint(customerRequest.getPoint());}
        if(customerRequest.getName() != null) {customer.setName(customerRequest.getName());}
        if(customerRequest.getPhone() != null) {customer.setPhone(customerRequest.getPhone());}

        customerRepository.save(customer);
        return customerMapper.toResponse(customer);
    }

    public CustomerResponse addCustomer(CustomerRequest customerRequest) {
        if (customerRepository.findByPhone(customerRequest.getPhone()).isPresent()) {
            throw new DuplicateResourceException("Phone number already exists");
        }
        if (customerRepository.findByEmail(customerRequest.getEmail()).isPresent()) {
            throw new DuplicateResourceException("Email already exists");
        }



        Customer customer = customerMapper.toModel(customerRequest);
        customerRepository.save(customer);
        return customerMapper.toResponse(customer);
    }

    public CustomerResponse deleteCustomer(Integer id) {
        Customer customer = customerRepository.findById(id)
                .orElseThrow(()-> new CustomerNotFoundException("Customer with id " + id + " not found"));
        customerRepository.delete(customer);
        return customerMapper.toResponse(customer);
    }
}
