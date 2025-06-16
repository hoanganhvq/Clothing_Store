package cit.backend.mapper;

import cit.backend.dto.request.ProductRequest;
import cit.backend.dto.respone.CategoryResponse;
import cit.backend.dto.respone.ProductResponse;
import cit.backend.model.Category;
import cit.backend.model.Product;
import org.mapstruct.Mapper;
import org.mapstruct.Mapping;

import java.util.List;

@Mapper(componentModel = "spring", uses = {CategoryMapper.class})
public interface ProductMapper {
    @Mapping(source = "category", target = "category")
    ProductResponse toResponse(Product product) ;
    Product toModel(ProductRequest productRequest);
    List<ProductResponse> toProductResponseList(List<Product> products);
}
