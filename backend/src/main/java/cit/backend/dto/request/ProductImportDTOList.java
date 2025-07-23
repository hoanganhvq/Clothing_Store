package cit.backend.dto.request;

import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.*;

import java.util.List;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
public class ProductImportDTOList {
    @JsonProperty("items")
    private List<ProductRequest> items;
}
