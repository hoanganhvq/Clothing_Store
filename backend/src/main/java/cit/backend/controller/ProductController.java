package cit.backend.controller;

import cit.backend.dto.request.ImportProductDTO;
import cit.backend.dto.request.ProductRequest;
import cit.backend.dto.request.ProductUpdateRequest;
import cit.backend.dto.respone.PageProductResponse;
import cit.backend.dto.respone.PageResponse;
import cit.backend.dto.respone.ProductResponse;
import cit.backend.exception.CategoryNotFoundException;
import cit.backend.exception.ProductNotFoundException;
import cit.backend.service.ProductService;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.Authentication;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("product")
@Validated
public class ProductController {
    @Autowired
    private ProductService productService;


    @GetMapping("/{id}")
    public ResponseEntity<ProductResponse> getProductById(@PathVariable int id) {

           return ResponseEntity.ok(productService.getProductById(id));

    }


    @GetMapping("/search/{productCode}")
    public ResponseEntity<ProductResponse> getProductByProductCode(@PathVariable String productCode) {

            return ResponseEntity.ok(productService.searchProduct(productCode));

    }

    @GetMapping()
    public ResponseEntity<PageResponse<ProductResponse>> getProducts(
            @RequestParam(value = "page", defaultValue = "1") int page,
            @RequestParam(value = "search", required = false) String search) {

            return ResponseEntity.ok(productService.getProducts(page, search));
    }

    @PostMapping("impport")
    public void importProduct(@RequestBody ProductRequest productRequest) {

    }

    @PostMapping
    public ResponseEntity<ProductResponse> addProduct(
            @Valid @RequestBody ProductRequest productRequest) {
        //Check if product already exits, update for this product
            return ResponseEntity.ok(productService.createProduct(productRequest));

    }

    @PatchMapping("/{id}")
    public ResponseEntity<ProductResponse> updateProduct(
            @PathVariable int id, @RequestBody ProductUpdateRequest productRequest) {
            return ResponseEntity.ok(productService.updateProduct(id, productRequest));
    }



    @DeleteMapping("/{id}")
    public ResponseEntity<ProductResponse> deleteProduct(@PathVariable int id) {
            productService.deleteProduct(id);
            return ResponseEntity.noContent().build();
    }

    @PostMapping("/import")
    public void importProduct (ImportProductDTO importProductDTO){
        productService.importProduct(importProductDTO);
    }
}
