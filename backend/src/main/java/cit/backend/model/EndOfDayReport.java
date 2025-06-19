package cit.backend.model;

import jakarta.persistence.*;
import lombok.*;
import java.math.BigDecimal;
import java.time.LocalDateTime;

@Entity
@Table(name = "end_of_day_report")
@Data
@NoArgsConstructor
@AllArgsConstructor
public class EndOfDayReport {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;

    @Column(name = "report_date", nullable = false)
    private LocalDateTime reportDate;

    @Column(name = "system_balance")
    private BigDecimal systemBalance;

    @Column(name = "actual_balance")
    private BigDecimal actualBalance;

    private BigDecimal difference;

    private String notes;

    @ManyToOne
    @JoinColumn(name = "cash_register_id", referencedColumnName = "id")
    private CashRegister cashRegister;

    @ManyToOne
    @JoinColumn(name = "staff_id", referencedColumnName = "id")
    private Staff employee;

    @ManyToOne
    @JoinColumn(name = "approved_by_employee_id", referencedColumnName = "id")
    private Staff approvedBy;

    private boolean isApproved;
}
