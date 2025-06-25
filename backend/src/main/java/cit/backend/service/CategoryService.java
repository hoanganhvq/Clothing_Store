package cit.backend.service;

import cit.backend.dto.request.CategoryRequest;
import cit.backend.dto.respone.CategoryResponse;
import cit.backend.dto.respone.PageResponse;
import cit.backend.exception.CategoryNotFoundException;
import cit.backend.mapper.CategoryMapper;
import cit.backend.model.Category;
import cit.backend.repository.CategoryRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class CategoryService {
    @Autowired
    private CategoryRepository categoryRepository;

    @Autowired
    private CategoryMapper categoryMapper;


    public PageResponse<CategoryResponse> getCategories(Pageable pageable, String search, boolean returnAll) {
       if(returnAll) {
           pageable = Pageable.unpaged();
       }
        Page<Category> categoryPage = categoryRepository.findByNameContainingIgnoreCase(search, pageable);
        List<CategoryResponse> content = categoryPage.getContent()
                .stream()
                .map(categoryMapper::toResponse)
                .toList();

        PageResponse pageResponse = new PageResponse<>();
        pageResponse.setData(content);
        pageResponse.setTotalPages(categoryPage.getTotalPages());
        pageResponse.setTotalCount(categoryPage.getTotalElements());
        pageResponse.setPage(categoryPage.getNumber() + 1); //Vi bat dau tu 0


        return pageResponse;

    }

    public CategoryResponse getCategoryById(int id) {
        Category category = categoryRepository.findById(id)
                .orElseThrow(()-> new CategoryNotFoundException("Category not found"));
        return categoryMapper.toResponse(categoryRepository.findById(id).get());
    }

    public CategoryResponse updateCategory(int id, CategoryRequest categoryRequest) {
        Category category = categoryRepository.findById(id)
                .orElseThrow(() -> new CategoryNotFoundException("Category not found  " + id));
        category.setName(categoryRequest.getName());
        categoryRepository.save(category);
        return categoryMapper.toResponse(category);
    }

    public CategoryResponse addCategory(CategoryRequest categoryRequest) {
        Category category = categoryMapper.toModel(categoryRequest);
        categoryRepository.save(category);
        return categoryMapper.toResponse(category);
    }

    public CategoryResponse deleteCategory(int id) {
         Category category =  categoryRepository.findById(id)
                .orElseThrow(() -> new CategoryNotFoundException("Category not found  " + id));
         categoryRepository.delete(category);
         return categoryMapper.toResponse(category);
    }
}
