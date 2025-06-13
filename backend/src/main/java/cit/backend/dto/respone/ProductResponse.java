package cit.backend.dto.respone;

import cit.backend.model.Category;
import lombok.Getter;
import lombok.Setter;

import java.math.BigDecimal;

@Getter
@Setter
public class ProductResponse {

    private int id;

    private String name;


    private BigDecimal price;


    private BigDecimal costPrice;


    private String description;


    private int stockQuantity;


    private String size;


    private String color;


    private String imageUrl;

    private Category category;
}
