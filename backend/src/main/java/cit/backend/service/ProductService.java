package cit.backend.service;

import cit.backend.dto.request.ImportProductDTO;
import cit.backend.dto.request.ProductRequest;
import cit.backend.dto.request.ProductUpdateRequest;
import cit.backend.dto.respone.CategoryResponse;
import cit.backend.dto.respone.PageProductResponse;
import cit.backend.dto.respone.PageResponse;
import cit.backend.dto.respone.ProductResponse;
import cit.backend.exception.CategoryNotFoundException;
import cit.backend.exception.ProductNotFoundException;

import cit.backend.mapper.ProductMapper;
import cit.backend.model.Category;
import cit.backend.model.Product;
import cit.backend.repository.CategoryRepository;
import cit.backend.repository.ProductRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.stereotype.Service;
import org.springframework.util.StringUtils;
import org.springframework.data.domain.Pageable;
import org.springframework.web.servlet.HandlerMapping;

import java.util.List;


@Service
public class ProductService {
    @Autowired
    private ProductRepository productRepository;

    @Autowired
    private CategoryRepository categoryRepository;

    @Autowired
    private ProductMapper productMapper;


    public List<ProductResponse> getAll() {
        return productMapper.toProductResponseList(productRepository.findAll());
    }

    public ProductResponse getProductById(int id) {
        Product product = productRepository.findById(id).orElseThrow(()-> new ProductNotFoundException("Product not found"));

        return productMapper.toResponse(product);
    }

    public ProductResponse createProduct(ProductRequest productRequest) {
        Category category = categoryRepository.findById(productRequest.getCategoryId())
                .orElseThrow(() -> new CategoryNotFoundException("Category not found"));

        Product product = productMapper.toModel(productRequest);

        product.setCategory(category);

        Product savedProduct = productRepository.save(product);

        return productMapper.toResponse(savedProduct);
    }




    public ProductResponse updateProduct (int id, ProductUpdateRequest productRequest) {
        Product product = productRepository.findById(id).orElseThrow(()->new ProductNotFoundException("Product not found"));

        if(productRequest.getName() !=null) {
            product.setName(productRequest.getName());
        }
        if(productRequest.getPrice() != null)
        {
            product.setPrice(productRequest.getPrice());
        }
        if(productRequest.getDescription() !=null) {
            product.setDescription(productRequest.getDescription());
        }
        if(productRequest.getImageUrl() !=null) {
            product.setImageUrl(productRequest.getImageUrl());
        }
        if(productRequest.getCategoryId() !=null) {
            Category category =  categoryRepository.findById(productRequest.getCategoryId())
                    .orElseThrow(() -> new CategoryNotFoundException("Category not found"));

            product.setCategory(category);
        }
        if(productRequest.getProductCode() !=null) {
            product.setProductCode(productRequest.getProductCode());
        }
        if(productRequest.getCostPrice()!=null) {
            product.setCostPrice(productRequest.getCostPrice());
        }
        if(productRequest.getStockQuantity()!=null) {
            product.setStockQuantity(productRequest.getStockQuantity());
        }
        if(productRequest.getSize() !=null) {
            product.setSize(productRequest.getSize());
        }
        if(productRequest.getColor() !=null) {
            product.setColor(productRequest.getColor());
        }


        return productMapper.toResponse(productRepository.save(product));
    }
    
    public void deleteProduct(int id){
        Product product = productRepository.findById(id).orElseThrow(()-> new ProductNotFoundException("Product not found"));
         productRepository.delete(product);
    }


    public PageResponse<ProductResponse> getProducts(int page, String search){
        // Xác định số lượng sản phẩm mỗi trang, ví dụ 5 sản phẩm mỗi trang
        Pageable pageable = PageRequest.of(page - 1, 5);
        Page<Product> productPage;

        if(search != null && !search.isEmpty()){
            //Neu co tham so tim kiem
            productPage = productRepository.findByNameContainingIgnoreCase(search, pageable);

        } else{
            productPage = productRepository.findAll(pageable);
        }
        List<ProductResponse> content = productPage.getContent()
                .stream()
                .map(productMapper::toResponse)
                .toList();

        PageResponse<ProductResponse> response = new PageResponse<>();
        response.setData(content);
        response.setPage(productPage.getNumber() + 1);
        response.setTotalPages(productPage.getTotalPages());
        response.setTotalCount(productPage.getTotalElements());

        return response;
    }


    public ProductResponse searchProduct(String productCode){
        if (productCode == null || productCode.trim().isEmpty()) {
            throw new IllegalArgumentException("Product code must not be null or empty.");
        }

        Product product = productRepository.findByProductCode(productCode)
                .orElseThrow(()->new ProductNotFoundException("Not found product "+ productCode));


        return productMapper.toResponse(product);
    }

    public void  importProduct (ImportProductDTO importProductDTO){

    }
}
