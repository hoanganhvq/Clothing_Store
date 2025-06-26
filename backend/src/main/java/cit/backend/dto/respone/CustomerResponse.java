package cit.backend.dto.respone;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@NoArgsConstructor
@AllArgsConstructor
public class CustomerResponse {

    private int id;

    private String name;

    private String phone;

    private String email;

    private String address;

    private int point;

    private PointDiscountRuleResponse pointDiscountRule;

}
