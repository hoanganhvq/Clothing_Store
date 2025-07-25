package cit.backend.service;

import cit.backend.dto.request.ImportProductDTO;
import cit.backend.dto.request.ProductImportDTOList;
import cit.backend.dto.request.ProductRequest;
import cit.backend.dto.request.ProductUpdateRequest;
import cit.backend.dto.respone.PageResponse;
import cit.backend.dto.respone.ProductResponse;
import cit.backend.exception.CategoryNotFoundException;
import cit.backend.exception.ProductNotFoundException;

import cit.backend.mapper.ProductMapper;
import cit.backend.model.Category;
import cit.backend.model.Product;
import cit.backend.repository.CategoryRepository;
import cit.backend.repository.ProductRepository;
import jakarta.mail.MessagingException;
import lombok.Getter;
import lombok.Setter;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.stereotype.Service;
import org.springframework.data.domain.Pageable;

import java.math.BigDecimal;
import java.util.*;
import java.util.logging.Logger;


@Service
public class ProductService {
    @Autowired
    private ProductRepository productRepository;

    @Autowired
    private CategoryRepository categoryRepository;

    @Autowired
    private ProductMapper productMapper;

    @Autowired
    private EmailService emailService;
    private final Logger logger = Logger.getLogger(ProductService.class.getName());

    @Value("${spring.mail.username}")
    private String userEmail;

    public List<ProductResponse> getAll() {
        return productMapper.toProductResponseList(productRepository.findAll());
    }

    public ProductResponse getProductById(int id) {
        Product product = productRepository.findById(id).orElseThrow(() -> new ProductNotFoundException("Product not found"));

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


    public ProductResponse updateProduct(int id, ProductUpdateRequest productRequest) {
        Product product = productRepository.findById(id).orElseThrow(() -> new ProductNotFoundException("Product not found"));

        if (productRequest.getName() != null) {
            product.setName(productRequest.getName());
        }
        if (productRequest.getPrice() != null) {
            product.setPrice(productRequest.getPrice());
        }
        if (productRequest.getDescription() != null) {
            product.setDescription(productRequest.getDescription());
        }
        if (productRequest.getImageUrl() != null) {
            product.setImageUrl(productRequest.getImageUrl());
        }
        if (productRequest.getCategoryId() != null) {
            Category category = categoryRepository.findById(productRequest.getCategoryId())
                    .orElseThrow(() -> new CategoryNotFoundException("Category not found"));

            product.setCategory(category);
        }
        if (productRequest.getProductCode() != null) {
            product.setProductCode(productRequest.getProductCode());
        }
        if (productRequest.getCostPrice() != null) {
            product.setCostPrice(productRequest.getCostPrice());
        }
        if (productRequest.getStockQuantity() != null) {

            product.setStockQuantity(productRequest.getStockQuantity());

            if (productRequest.getStockQuantity() <= 5) {
                sendLowWarningQuantityEmail(userEmail, product.getName(), productRequest.getStockQuantity());
            }
        }



        return productMapper.toResponse(productRepository.save(product));
    }

    public String sendLowWarningQuantityEmail(String to, String productName, int quantity) {
        try {
            System.out.println("Send email warning");
            String subject = "Warning: Low Product Quantity";

            String content = String.format(
                    "<p>Dear admin,</p>" +
                            "<p>We would like to inform you that the product \"<strong>%s</strong>\" has a low quantity of <strong>%d</strong>.</p>" +
                            "<p>Best regards,<br>VuaPos</p>",
                    productName, quantity
            );

            emailService.sendEmail(to, subject, content);
            return "Success to send email warning";
        } catch (MessagingException e) {
            e.printStackTrace();
            return "Fail to send low warning quantity";
        }
    }

    public void deleteProduct(int id) {
        Product product = productRepository.findById(id).orElseThrow(() -> new ProductNotFoundException("Product not found"));
        productRepository.delete(product);
    }


    public PageResponse<ProductResponse> getProducts(int page, String search) {
        // Xác định số lượng sản phẩm mỗi trang, ví dụ 5 sản phẩm mỗi trang
        Pageable pageable = PageRequest.of(page - 1, 5);
        Page<Product> productPage;

        if (search != null && !search.isEmpty()) {
            //Neu co tham so tim kiem
            productPage = productRepository.findByNameContainingIgnoreCase(search, pageable);

        } else {
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


    public ProductResponse searchProduct(String productCode) {
        if (productCode == null || productCode.trim().isEmpty()) {
            throw new IllegalArgumentException("Product code must not be null or empty.");
        }

        Product product = productRepository.findByProductCode(productCode)
                .orElseThrow(() -> new ProductNotFoundException("Not found product " + productCode));


        return productMapper.toResponse(product);
    }

    public void importProduct(ImportProductDTO importProductDTO) {

    }

    @Getter
    @Setter
    public class ImportError {
        private String product_code;
        private Object inputData;
        private String error;
        public ImportError(Object inputData, String error) {
            this.inputData = inputData;
            this.error = error;
        }

    }

    @Getter
    @Setter
    public class ImportSummary {
        private Integer createdCount;
        private Integer updatedCount;
        private Integer errorCount;
        public ImportSummary(int createdCount, int updatedCount, int errorCount) {
            this.createdCount = createdCount;
            this.updatedCount = updatedCount;
            this.errorCount = errorCount;
        }

    }

    @Getter
    @Setter
    public class ImportResult {
        List<ProductResponse> created;
        List<ProductResponse> updated;
        List<ImportError> error;
        ImportSummary summary;
        public ImportResult() {
            this.created = new ArrayList<>();
            this.updated = new ArrayList<>();
            this.error = new ArrayList<>();
            this.summary = new ImportSummary(0, 0, 0);
        }

        // Helper methods to add to lists and increment summary counts
        public void addCreated(ProductResponse product) {
            this.created.add(product);
            this.summary.createdCount++;
        }

        public void addUpdated(ProductResponse product) {
            this.updated.add(product);
            this.summary.updatedCount++;
        }

        public void addError(ImportError error) {
            this.error.add(error);
            this.summary.errorCount++;
        }

    }

    public ImportResult importProducts(ProductImportDTOList productRequest) {
        List<ProductRequest> productsToImport = productRequest.getItems();
        ImportResult results = new ImportResult();

        if (productsToImport == null || productsToImport.isEmpty()) {
            logger.warning("No products provided for import.");
            return results;
        }

        List<String> productCodes = new ArrayList<>();
        for (ProductRequest product : productsToImport) {
            if (product.getProductCode() != null) {
                productCodes.add(product.getProductCode());
            }
        }

        if (productCodes.isEmpty()) {
            logger.warning("Import data contains no valid product codes.");
            return results;
        }

        List<Product> existingProducts = productRepository.findByProductCodeIn(productCodes);
        Map<String, Product> existingProductMap = new HashMap<>();
        for (Product product : existingProducts) {
            existingProductMap.put(product.getProductCode(), product);
        }

        List<Product> productsToCreate = new ArrayList<>();
        List<Product> productsToUpdate = new ArrayList<>();

        for (ProductRequest productDto : productsToImport) {
            if (productDto.getProductCode() == null) {
                logger.warning(String.format("Skipping item due to missing product_code: %s", productDto.getProductCode()));
                results.addError(new ImportError(productDto, "Missing product_code"));
                continue;
            }

            Product existingProduct = existingProductMap.get(productDto.getProductCode());

            if (existingProduct != null) {
                int newQuantity = existingProduct.getStockQuantity() + productDto.getStockQuantity();
                existingProduct.setStockQuantity(newQuantity);
                productsToUpdate.add(existingProduct);
            } else {
                Product newProduct = new Product();
                newProduct.setName(productDto.getName());
                newProduct.setProductCode(productDto.getProductCode());
                newProduct.setPrice(productDto.getPrice());
                newProduct.setCostPrice(productDto.getCostPrice());
                newProduct.setStockQuantity(productDto.getStockQuantity());
                newProduct.setImageUrl(productDto.getImageUrl());
                Category category = categoryRepository.findById(productDto.getCategoryId())
                                .orElseThrow(()-> new CategoryNotFoundException("Category not found"));
                newProduct.setCategory(category);
                productsToCreate.add(newProduct);
            }
        }

        try {
            if (!productsToCreate.isEmpty()) {
                productRepository.saveAll(productsToCreate);
                for (Product product : productsToCreate) {
                    results.addCreated(productMapper.toResponse(product));
                }
            }

            if (!productsToUpdate.isEmpty()) {
                productRepository.saveAll(productsToUpdate);
                for (Product product : productsToUpdate) {
                    results.addUpdated(productMapper.toResponse(product));
                }
            }

        } catch (Exception e) {
            logger.warning("Error during product import");
            results.addError(new ImportError("Transaction Level", e.getMessage()));
        }


        return results;
    }

}
