package cit.backend.dto.request;

import lombok.Getter;
import lombok.Setter;
import com.fasterxml.jackson.annotation.JsonProperty;
import java.math.BigDecimal;

@Getter
@Setter
public class ProductUpdateRequest {
    @JsonProperty("product_name")
    private String name;

    @JsonProperty("product_code")
    private String productCode;

    @JsonProperty("price")
    private BigDecimal price;

    @JsonProperty("cost_price")
    private BigDecimal costPrice;

    @JsonProperty("description")
    private String description;

    @JsonProperty("stock_quantity")
    private Integer stockQuantity;

    @JsonProperty("size")
    private String size;

    @JsonProperty("color")
    private String color;

    @JsonProperty("image_path")
    private String imageUrl;

    @JsonProperty("category_id")
    private Integer categoryId;

    @JsonProperty("discount")
    private Integer discount;
}