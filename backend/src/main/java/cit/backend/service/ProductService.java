package cit.backend.service;

import cit.backend.dto.request.ProductRequest;
import cit.backend.dto.respone.ProductResponse;
import cit.backend.exception.CategoryNotFoundException;
import cit.backend.exception.ProductNotFoundException;

import cit.backend.mapper.ProductMapper;
import cit.backend.model.Category;
import cit.backend.model.Product;
import cit.backend.repository.CategoryRepository;
import cit.backend.repository.ProductRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

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

    public List<Product> getProductsByCategory(String category) {
        return productRepository.findAllByCategory(category);
    }


    public void deleteProduct(int id){
        Product product = productRepository.findById(id).orElseThrow(()-> new ProductNotFoundException("Product not found"));
         productRepository.delete(product);
    }

}
