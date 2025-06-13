package cit.backend.mapper;

import cit.backend.dto.request.ProductRequest;
import cit.backend.dto.respone.ProductResponse;
import cit.backend.model.Product;
import org.mapstruct.Mapper;
import org.mapstruct.Mapping;

import java.util.List;

@Mapper(componentModel = "spring")
public interface ProductMapper {
    @Mapping(source = "category.id", target = "categoryId")
    ProductResponse toResponse(Product product) ;
    Product toModel(ProductRequest productRequest);
    List<ProductResponse> toProductResponseList(List<Product> products);
}
