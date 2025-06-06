package cit.backend.model;

import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import net.minidev.json.annotate.JsonIgnore;

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

    @Column(nullable = true, name = "fname", length = 50)
    private String fname;

    @Column(name = "lname", nullable = true, length = 50)
    private String lname;

    @Column(length = 100, name = "phone")
    private String phone;

    @Column(name = "email", unique = true, length = 50)
    private String email;

    @Column(name = "address", length = 255)
    private String address;

    @Column(name = "point")
    private int point;

    @JsonIgnore //Tranh lap
    @OneToMany(mappedBy = "customer", cascade = CascadeType.ALL)
    private List<Order> orders;

    @OneToOne(cascade = CascadeType.ALL)
    @JoinColumn(name = "pointDisCountRule_id", referencedColumnName = "id")
    private PointDiscountRule pointDiscountRule;

}
