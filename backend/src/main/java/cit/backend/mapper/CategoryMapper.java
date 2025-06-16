package cit.backend.mapper;

import cit.backend.dto.request.CategoryRequest;
import cit.backend.dto.respone.CategoryResponse;
import cit.backend.model.Category;
import org.mapstruct.Mapper;

import java.util.List;

@Mapper(componentModel = "spring")
public interface CategoryMapper {
    CategoryResponse toResponse(Category category);
    Category toModel(CategoryRequest categoryRequest);
    List<CategoryResponse> toResponseList(List<Category> categories);
}
