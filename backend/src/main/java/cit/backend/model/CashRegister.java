package cit.backend.model;

import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;
import java.time.LocalDateTime;

@Entity
@Data
@NoArgsConstructor
@AllArgsConstructor
@Table(name = "cashRegister")
public class CashRegister {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;

    @Column(name = "name")
    private String name;

    @Column(name = "currentBalance")
    private BigDecimal currentBalance;

    @Column(name = "lastClosedAt")
    private LocalDateTime lastClosedAt;

    @Column(name = "isActive")
    private boolean isActive;

    @ManyToOne //Nhieu nhan vien tren mot quay
    @JoinColumn(name = "staff_id", referencedColumnName = "id")
    private Staff staff;
}
