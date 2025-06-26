package cit.backend.dto.request;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;

@Data
@AllArgsConstructor
@NoArgsConstructor
public class PointDiscountRuleRequest {

        private int  minPoints;

        private int maxPoints;

        private BigDecimal discount;


}
