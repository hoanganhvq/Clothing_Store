package cit.backend.repository;

import cit.backend.model.Category;
import cit.backend.model.Product;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;

public interface ProductRepository extends JpaRepository <Product, Integer> {
}
