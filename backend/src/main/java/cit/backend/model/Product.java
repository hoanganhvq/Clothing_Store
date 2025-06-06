package cit.backend.model;

import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import net.minidev.json.annotate.JsonIgnore;


import java.util.List;
@Data
@Entity
@NoArgsConstructor
@AllArgsConstructor
@Table(name = "products")
public class Product {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;

    @Column(name = "name")
    private String name;

    @Column(name = "price")
    private double price;

    @Column(name = "desciption")
    private String description;

    @Column(name = "stockQuantity")
    private int stockQuantity;

    @Column(name = "size")
    private String size;

    @Column(name = "color")
    private String color;

    @OneToMany(mappedBy = "product")
    @JsonIgnore
    private List<OrderItem> orderItemList;


}
