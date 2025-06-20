package cit.backend.dto.respone;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import java.math.BigDecimal;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
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

    private CategoryResponse category;
}
