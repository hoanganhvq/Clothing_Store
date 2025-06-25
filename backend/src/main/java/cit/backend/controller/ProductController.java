package cit.backend.controller;

import cit.backend.dto.request.ProductRequest;
import cit.backend.dto.respone.PageProductResponse;
import cit.backend.dto.respone.PageResponse;
import cit.backend.dto.respone.ProductResponse;
import cit.backend.exception.CategoryNotFoundException;
import cit.backend.exception.ProductNotFoundException;
import cit.backend.service.ProductService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.Authentication;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("products")
public class ProductController {
    @Autowired
    private ProductService productService;

    @GetMapping("/all")
    public ResponseEntity<List<ProductResponse>> getAllProducts(Authentication auth) {
        return ResponseEntity.ok(productService.getAll());
    }

    @GetMapping("/{id}")
    public ResponseEntity<ProductResponse> getProductById(@PathVariable int id) {
        try{
           return ResponseEntity.ok(productService.getProductById(id));
        } catch (ProductNotFoundException e) {
            return ResponseEntity.notFound().build();
        }
    }

    @GetMapping("/search/{productCode}")
    public ResponseEntity<ProductResponse> getProductByProductCode(@PathVariable String productCode) {
        try{
            return ResponseEntity.ok(productService.searchProduct(productCode));
        }catch (ProductNotFoundException e){
            return ResponseEntity.notFound().build();
        }
    }

    @GetMapping()
    public ResponseEntity<PageResponse<ProductResponse>> getProducts(
            @RequestParam(value = "page", defaultValue = "1") int page,
            @RequestParam(value = "search", required = false) String search) {

        try{
            return ResponseEntity.ok(productService.getProducts(page, search));
        } catch (RuntimeException e){
            return ResponseEntity.notFound().build();
        }
    }

    @PostMapping
    public ResponseEntity<ProductResponse> addProduct(@RequestBody ProductRequest productRequest) {
        try{
            return ResponseEntity.ok(productService.createProduct(productRequest));
        }catch(CategoryNotFoundException e){
            return ResponseEntity.notFound().build();
        }
    }

    @PutMapping("/{id}")
    public ResponseEntity<ProductResponse> updateProduct(@PathVariable int id, @RequestBody ProductRequest productRequest) {
        try{
            return ResponseEntity.ok(productService.updateProduct(id, productRequest));
        }catch (ProductNotFoundException e){
            return ResponseEntity.notFound().build();
        } catch (CategoryNotFoundException e){
            return ResponseEntity.notFound().build();
        }
    }



    @DeleteMapping("/{id}")
    public ResponseEntity<ProductResponse> deleteProduct(@PathVariable int id) {
        try{
            productService.deleteProduct(id);
            return ResponseEntity.noContent().build();
        }catch (ProductNotFoundException e){
            return ResponseEntity.notFound().build();
        }
    }
}
