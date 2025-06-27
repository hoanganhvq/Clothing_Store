package cit.backend.service;

import cit.backend.dto.request.CustomerRequest;
import cit.backend.dto.respone.CustomerResponse;
import cit.backend.dto.respone.PageResponse;
import cit.backend.exception.CustomerNotFoundException;
import cit.backend.mapper.CustomerMapper;
import cit.backend.model.Customer;
import cit.backend.repository.CustomerRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;

@Service
public class CustomerService {
    @Autowired
    private CustomerRepository customerRepository;

    @Autowired
    private CustomerMapper customerMapper;

    public PageResponse<CustomerResponse> getAllCustomers(Pageable pageable, String search) {
        Page<Customer> pageCustomers = customerRepository.findByNameContainingIgnoreCase(search, pageable);
        PageResponse<CustomerResponse> response = new PageResponse<>();
        response.setPage(pageCustomers.getNumber() + 1);
        response.setData(customerMapper.toResponseList(pageCustomers.getContent()));
        response.setTotalCount(pageCustomers.getTotalElements());
        response.setTotalPages(pageCustomers.getTotalPages());

        return response;
    }

    public CustomerResponse getCustomerById(Integer id) {
        Customer customer = customerRepository.findById(id)
                .orElseThrow(()-> new CustomerNotFoundException("Customer with id " + id + " not found"));
        return customerMapper.toResponse(customer);
    }

    public CustomerResponse updateCustomer(int id, CustomerRequest customerRequest) {
        Customer customer = customerRepository.findById(id)
                .orElseThrow(()-> new CustomerNotFoundException("Customer with id " + id + " not found"));
        customer.setName(customerRequest.getName());
        customer.setEmail(customerRequest.getEmail());
        customer.setPhone(customerRequest.getPhone());
        customer.setPoint(customerRequest.getPoint());

        customerRepository.save(customer);
        return customerMapper.toResponse(customer);
    }

    public CustomerResponse addCustomer(CustomerRequest customerRequest) {
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
