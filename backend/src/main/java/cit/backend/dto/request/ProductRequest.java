package cit.backend.dto.request;

import jakarta.persistence.Column;
import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class ProductRequest {

    private String name;


    private double price;


    private double costPrice;


    private String description;


    private int stockQuantity;


    private String size;


    private String color;


    private String imageUrl;

    private int categoryId;
}
