package cit.backend.dto.request;

import cit.backend.Enum.OrderStatus;
import lombok.Getter;
import lombok.Setter;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.util.List;

@Getter
@Setter
public class OrderRequest {

    private int customerId;

    private int staffId;

    private Integer promotionId;

    private BigDecimal totalAmount;
}
