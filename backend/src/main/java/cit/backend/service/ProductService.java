package cit.backend.service;

import cit.backend.dto.request.ProductRequest;
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
        Product product = new Product();

        product.setName(productRequest.getName());
        product.setProductCode(productRequest.getProductCode());
        product.setPrice(productRequest.getPrice());
        product.setCostPrice(productRequest.getCostPrice());
        product.setDescription(productRequest.getDescription());
        product.setStockQuantity(productRequest.getStockQuantity());
        product.setSize(productRequest.getSize());
        product.setColor(productRequest.getColor());
        product.setImageUrl(productRequest.getImageUrl());

        // Tìm category
        Category category = categoryRepository.findById(productRequest.getCategoryId())
                .orElseThrow(() -> new CategoryNotFoundException("Category not found"));

        product.setCategory(category);

        // Lưu và trả về response
        return productMapper.toResponse(productRepository.save(product));
    }

    public void importProduct(ProductRequest productRequest) {
        //Write code here
    }



    public ProductResponse updateProduct (int id, ProductRequest productRequest) {
        Product product = productRepository.findById(id).orElseThrow(()->new ProductNotFoundException("Product not found"));

        product.setName(productRequest.getName());
        product.setPrice(productRequest.getPrice());
        product.setCostPrice(productRequest.getCostPrice());
        product.setDescription(productRequest.getDescription());
        product.setStockQuantity(productRequest.getStockQuantity());
        product.setSize(productRequest.getSize());
        product.setColor(productRequest.getColor());
        product.setImageUrl(productRequest.getImageUrl());
        Category category = categoryRepository.findById(productRequest.getCategoryId())
                .orElseThrow(()-> new RuntimeException("Category not found"));

        product.setCategory(category);

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

}
