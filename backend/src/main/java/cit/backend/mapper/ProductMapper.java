package cit.backend.mapper;

import cit.backend.dto.request.ProductRequest;
import cit.backend.dto.respone.ProductResponse;
import cit.backend.model.Product;
import org.mapstruct.Mapper;

import java.util.List;

@Mapper(componentModel = "spring")
public interface ProductMapper {
    ProductResponse toResponse(Product product) ;
    Product toModel(ProductRequest productRequest);
    List<ProductResponse> toProductResponseList(List<Product> products);
}
