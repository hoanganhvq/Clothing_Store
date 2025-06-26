package cit.backend.dto.respone;


import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;

@Data
@AllArgsConstructor
@NoArgsConstructor
public class PointDiscountRuleResponse {
    private int id;

    private int minPoints;

    private int maxPoints;

    private BigDecimal discount;

}
