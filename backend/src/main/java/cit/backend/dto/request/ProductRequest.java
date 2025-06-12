package cit.backend.dto.request;

import jakarta.persistence.Column;
import lombok.Getter;
import lombok.Setter;

import java.math.BigDecimal;

@Getter
@Setter
public class ProductRequest {

    private String name;


    private BigDecimal price;


    private BigDecimal costPrice;


    private String description;


    private int stockQuantity;


    private String size;


    private String color;


    private String imageUrl;

    private int categoryId;
}
