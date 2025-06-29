package cit.backend.dto.request;

import lombok.Getter;
import lombok.Setter;

import java.math.BigDecimal;

@Getter
@Setter
public class ProductUpdateRequest {
    private String name;

    private String productCode;

    private BigDecimal price;

    private BigDecimal costPrice;

    private String description;

    private Integer stockQuantity;

    private String size;

    private String color;

    private String imageUrl;

    private Integer categoryId;

    private Integer discount;
}
