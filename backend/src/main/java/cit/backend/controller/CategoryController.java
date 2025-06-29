package cit.backend.controller;

import cit.backend.dto.request.CategoryRequest;
import cit.backend.dto.respone.CategoryResponse;
import cit.backend.dto.respone.PageResponse;
import cit.backend.dto.respone.ProductResponse;
import cit.backend.exception.CategoryNotFoundException;
import cit.backend.service.CategoryService;
import jakarta.validation.Valid;
import org.apache.coyote.BadRequestException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@Validated
@RequestMapping("category")
public class CategoryController {
    @Autowired
    private CategoryService categoryService;


    @GetMapping //Ok
    public ResponseEntity<PageResponse<CategoryResponse>> getCategories(
            @RequestParam(value = "page") String page,
            @RequestParam(value = "search", required = false, defaultValue = "") String search,
            @RequestParam(value = "return-all",defaultValue = "false") String returnAll
    ) {
            int pageNumber = Integer.parseInt(page);
            boolean getAll = Boolean.parseBoolean(returnAll);
            if (getAll) {
                List<CategoryResponse> all = categoryService.getAllCategories(); // trả về List
                PageResponse<CategoryResponse> response = new PageResponse<>();
                response.setData(all);
                response.setPage(1);
                response.setTotalPages(1);
                response.setTotalCount(all.size());

                return ResponseEntity.ok(response);
            }
            Pageable pageable = PageRequest.of(pageNumber - 1, 5);
            return ResponseEntity.ok(categoryService.getCategorySearchByName(pageable, search));

    }//Fix bug 500 -> 400 BadRequest

    @GetMapping("/{id}")
    public ResponseEntity<CategoryResponse> getCategoryById(
            @PathVariable int id
    ){
            return ResponseEntity.ok(categoryService.getCategoryById(id));
    }

    @PatchMapping("/{id}")
    public ResponseEntity<CategoryResponse> updateCategory(
            @PathVariable int id,
            @Valid @RequestBody CategoryRequest categoryRequest
    ){
            return ResponseEntity.ok(categoryService.updateCategory(id, categoryRequest));
    }

    @PostMapping
    public ResponseEntity<CategoryResponse> createCategory(
            @Valid @RequestBody CategoryRequest categoryRequest
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
            return ResponseEntity.ok(categoryService.deleteCategory(id));

    }

}
