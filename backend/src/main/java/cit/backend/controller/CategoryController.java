package cit.backend.controller;

import cit.backend.dto.request.CategoryRequest;
import cit.backend.dto.respone.CategoryResponse;
import cit.backend.dto.respone.PageResponse;
import cit.backend.dto.respone.ProductResponse;
import cit.backend.exception.CategoryNotFoundException;
import cit.backend.service.CategoryService;
import org.apache.coyote.BadRequestException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("category")
public class CategoryController {
    @Autowired
    private CategoryService categoryService;


    @GetMapping
    public ResponseEntity<PageResponse<CategoryResponse>> getCategories(
            @RequestParam("page") String page,
            @RequestParam("search") String search,
            @RequestParam(value = "return-all",defaultValue = "false") String returnAll
    ) {
        try{
            int pageNumber = Integer.parseInt(page);
            boolean getAll = Boolean.parseBoolean(returnAll);
            Pageable pageable = PageRequest.of(pageNumber - 1, 5);
            return ResponseEntity.ok(categoryService.getCategories(pageable, search, getAll));
        } catch (Exception e) {
            return ResponseEntity.notFound().build();
        }
    }

    @GetMapping("/{id}")
    public ResponseEntity<CategoryResponse> getCategoryById(
            @PathVariable int id
    ){
        try{
            return ResponseEntity.ok(categoryService.getCategoryById(id));
        }catch (CategoryNotFoundException e){
            return ResponseEntity.notFound().build();
        }

    }

    @PutMapping("/{id}")
    public ResponseEntity<CategoryResponse> updateCategory(
            @PathVariable int id,
            @RequestBody CategoryRequest categoryRequest
    ){
        try{
            return ResponseEntity.ok(categoryService.updateCategory(id, categoryRequest));
        }catch (CategoryNotFoundException e){
            return ResponseEntity.notFound().build();
        }
    }

    @PostMapping
    public ResponseEntity<CategoryResponse> createCategory(
            @RequestBody CategoryRequest categoryRequest
    ){
        try{
            return ResponseEntity.ok(categoryService.addCategory(categoryRequest));
        }catch (IllegalArgumentException e){
            return  ResponseEntity.badRequest().body(null);
        }
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<CategoryResponse> deleteCategory(
            @PathVariable int id
    ){
        try{
            return ResponseEntity.ok(categoryService.deleteCategory(id));
        }catch (CategoryNotFoundException e){
            return ResponseEntity.notFound().build();
        }
    }

}
