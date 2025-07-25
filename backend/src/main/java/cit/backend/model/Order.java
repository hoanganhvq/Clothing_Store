package cit.backend.model;

import cit.backend.Enum.OrderStatus;
import com.fasterxml.jackson.annotation.JsonManagedReference;
import jakarta.persistence.*;
import lombok.*;
import com.fasterxml.jackson.annotation.JsonIgnore;

import java.math.BigDecimal;
import java.security.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;

@Entity
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@Table(name = "orders")
@ToString(exclude = {"orders", "customer", "products", "orderItemList","category"})
public class Order {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;

    @Column(name = "orderDate")
    private LocalDateTime orderDate;

    @Enumerated(EnumType.STRING)
    @Column(name = "status")
    private OrderStatus status;

    @Column(name = "subTotal")
    private BigDecimal subTotal;

    @Column(name = "totalAmount")
    private BigDecimal totalAmount;

    @Column(name = "is_cash" )
    private Boolean isCash;

    @Column(name = "is_use_customer_point")
    private Boolean isUseCustomerPoint;

    @Column(name = "point_discount")
    private BigDecimal pointDiscount;

    @ManyToOne
    @JsonIgnore
    @JoinColumn(name = "customer_id", referencedColumnName = "id") //Create column customer_id and referrence to id of ustomer
    private Customer customer;


    @ManyToOne
    @JsonIgnore
    @JoinColumn(name = "staff_id", referencedColumnName = "id")
    private Staff staff;

    @ManyToOne
    @JsonIgnore
    @JoinColumn(name = "promotion_id", referencedColumnName = "id")
    private Promotion promotion;



    @OneToMany(mappedBy = "order", cascade = CascadeType.ALL)
    @JsonManagedReference
    private List<OrderItem> items;

}
