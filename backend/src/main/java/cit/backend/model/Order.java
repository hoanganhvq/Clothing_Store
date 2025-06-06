package cit.backend.model;

import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import net.minidev.json.annotate.JsonIgnore;

import java.time.LocalDate;
import java.util.List;

@Entity
@Data
@NoArgsConstructor
@AllArgsConstructor
@Table(name = "orders")
public class Order {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;

    @Column(name = "orderDate")
    private LocalDate orderDate;

    @Column(name = "status")
    private String status;

    @ManyToOne
    @JoinColumn(name = "customer_id", referencedColumnName = "id") //Create column customer_id and referrence to id of ustomer
    private Customer customer;


    @OneToMany(mappedBy = "order", cascade = CascadeType.ALL)
    private List<OrderItem> orderItemList;

    @ManyToOne
    @JoinColumn(name = "staff_id", referencedColumnName = "id")
    private Staff staff;



}
