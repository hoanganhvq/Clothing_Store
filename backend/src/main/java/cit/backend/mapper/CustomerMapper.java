package cit.backend.mapper;


import cit.backend.dto.request.CustomerRequest;
import cit.backend.dto.respone.CustomerResponse;
import cit.backend.model.Customer;
import org.mapstruct.Mapper;


import java.util.List;

@Mapper(componentModel = "spring")
public interface CustomerMapper {
    CustomerResponse toResponse(Customer customer);
    Customer toModel (CustomerRequest customerRequest);
    List<CustomerResponse> toResponseList(List<Customer> customerList);
}
