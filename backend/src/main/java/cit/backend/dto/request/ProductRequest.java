package cit.backend.dto.request;

import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.Getter;
import lombok.Setter;

import java.math.BigDecimal;

@Getter
@Setter
public class ProductRequest {

    @JsonProperty("product_name")
    private String name;

    @JsonProperty("product_code")
    private String productCode;

    @JsonProperty("price")
    private BigDecimal price;

    @JsonProperty("cost_price")
    private BigDecimal costPrice;

    @JsonProperty("discount")
    private int discount;

    @JsonProperty("stock_quantity")
    private int stockQuantity;

    @JsonProperty("size")
    private String size;

    @JsonProperty("color")
    private String color;

    @JsonProperty("description")
    private String description;

    @JsonProperty("image_path")
    private String imageUrl;

    @JsonProperty("category_id")
    private int categoryId;
}
