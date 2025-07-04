package cit.backend.controller;

import cit.backend.dto.request.PromotionRequest;
import cit.backend.dto.request.PromotionUpdateRequest;
import cit.backend.dto.respone.PageResponse;
import cit.backend.dto.respone.PromotionResponse;
import cit.backend.exception.PromotionNotFoundException;
import cit.backend.model.Promotion;
import cit.backend.service.PromotionService;
import jakarta.validation.Valid;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("promotions")
@Validated
public class PromotionController {
    @Autowired
    private PromotionService promotionService;

    @PostMapping
    public ResponseEntity<PromotionResponse> createPromotion(
            @Valid @RequestBody PromotionRequest promotionRequest) {

            System.out.println("name :" + promotionRequest.getName());
            System.out.println("discount: " + promotionRequest.getValue());
            System.out.println("start: " + promotionRequest.getStartDate());
            System.out.println("end: " + promotionRequest.getEndDate());
            return ResponseEntity.status(HttpStatus.CREATED)
                    .body(promotionService.createPromotion(promotionRequest));

    }

    @GetMapping("/{id}")
    public ResponseEntity<PromotionResponse> getPromotionById(@PathVariable Integer id) {

            return ResponseEntity.ok()
                    .body(promotionService.getPromotionById(id));

    }

    @GetMapping
    public ResponseEntity<PageResponse<PromotionResponse>> getPromotions(
            @RequestParam(value = "page", defaultValue = "1") int page,
            @RequestParam(value = "search", defaultValue = "", required = false) String search
    ) {



            Pageable pageable = PageRequest.of(page-1, 5);
            return ResponseEntity.ok()
                    .body(promotionService.getPromotions(pageable, search));
    }

    @PatchMapping("/{id}")
    public ResponseEntity<PromotionResponse> updatePromotion(
            @PathVariable int id,
            @RequestBody PromotionUpdateRequest promotionRequest) {

            System.out.println("Dât:" + promotionRequest.getName());
            System.out.println("value : " + promotionRequest.getValue());
        System.out.println("start_Date: " + promotionRequest.getStartDate());
            System.out.println("end_Date: " + promotionRequest.getEndDate());
            return ResponseEntity.status(HttpStatus.ACCEPTED)
                    .body(promotionService.updatePromotionById(id, promotionRequest));

    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Promotion> deletePromotion(
            @PathVariable int id
    ){

            return ResponseEntity.status(HttpStatus.ACCEPTED)
                    .body(promotionService.deletePromotionById(id));
    }
}
