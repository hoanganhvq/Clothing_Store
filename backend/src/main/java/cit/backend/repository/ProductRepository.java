package cit.backend.repository;

import cit.backend.model.Category;
import cit.backend.model.Product;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;

import javax.sound.sampled.Port;
import java.util.List;
import java.util.Optional;

public interface ProductRepository extends JpaRepository <Product, Integer> {
    Page<Product> findByNameContainingIgnoreCase(String name, Pageable pageable);
    Optional<Product> findByProductCode(String productCode);
}
