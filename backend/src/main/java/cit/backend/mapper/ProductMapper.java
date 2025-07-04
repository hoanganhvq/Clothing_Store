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
    @Mapping(source = "category.id", target = "categoryId")
    @Mapping(source = "category.name", target = "categoryName")
    ProductResponse toResponse(Product product);

    @Mapping(target = "category", ignore = true)
    Product toModel(ProductRequest productRequest);
    List<ProductResponse> toProductResponseList(List<Product> products);
}
