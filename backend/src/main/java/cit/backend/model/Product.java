package cit.backend.model;

import jakarta.persistence.*;
import lombok.*;
import net.minidev.json.annotate.JsonIgnore;


import java.math.BigDecimal;
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
    private BigDecimal price;

    @Column(name = "costPrice")
    private BigDecimal costPrice;

    @Column(name = "description")
    private String description;

    @Column(name = "stockQuantity")
    private int stockQuantity;

    @Column(name = "size", length = 50)
    private String size;

    @Column(name = "color")
    private String color;

    @Column(name = "imageUrl")
    private String imageUrl;

    @OneToMany(mappedBy = "product")
    @JsonIgnore
    private List<OrderItem> orderItemList;

    @ManyToOne
    @JoinColumn(name = "category_id",referencedColumnName = "id")
    private Category category;


}
