package cit.backend.model;

import com.fasterxml.jackson.annotation.JsonIgnore;
import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.List;

@Entity
@Data
@NoArgsConstructor
@AllArgsConstructor
@Table(name = "customers")
public class Customer {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;

    @Column(name = "name", nullable = true, length = 50)
    private String name;

    @Column(length = 100, name = "phone", unique = true)
    private String phone;

    @Column(name = "email", unique = true, length = 50)
    private String email;


    @Column(name = "point")
    private int point;

    @JsonIgnore //Tranh lap
    @OneToMany(mappedBy = "customer", cascade = CascadeType.ALL)
    private List<Order> orders;

    @OneToOne(cascade = CascadeType.ALL)
    @JoinColumn(name = "pointDiscountRule_id", referencedColumnName = "id")
    private PointDiscountRule pointDiscountRule;

}
