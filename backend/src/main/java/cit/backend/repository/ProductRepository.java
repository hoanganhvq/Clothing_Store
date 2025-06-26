package cit.backend.repository;

import cit.backend.model.Category;
import cit.backend.model.Product;
import jakarta.transaction.Transactional;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;

import javax.sound.sampled.Port;
import java.util.List;
import java.util.Optional;

public interface ProductRepository extends JpaRepository <Product, Integer> {
    Page<Product> findByNameContainingIgnoreCase(String name, Pageable pageable);
    Optional<Product> findByProductCode(String productCode);

    @Modifying
    @Transactional
    @Query("UPDATE Product p SET p.stockQuantity = p.stockQuantity - WHERE p.id = :productId")
    void updatePointById(int productId, int point);
}
