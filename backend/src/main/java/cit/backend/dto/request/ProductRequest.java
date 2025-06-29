package cit.backend.dto.request;

import jakarta.persistence.Column;
import lombok.Getter;
import lombok.Setter;

import java.math.BigDecimal;

@Getter
@Setter
public class ProductRequest {

    private String name;

    private String productCode;

    private BigDecimal price;

    private BigDecimal costPrice;

    private int discount;

    private int stockQuantity;

    private String size;

    private String color;

    private String description;

    private String imageUrl;

    private int categoryId;
}
