package cit.backend.dto.respone;

import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class ProductResponse {

    private int id;

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
