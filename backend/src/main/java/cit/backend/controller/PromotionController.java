package cit.backend.controller;

import cit.backend.dto.request.PromotionRequest;
import cit.backend.dto.respone.PageResponse;
import cit.backend.dto.respone.PromotionResponse;
import cit.backend.exception.PromotionNotFoundException;
import cit.backend.model.Promotion;
import cit.backend.service.PromotionService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("promotions")
public class PromotionController {
    @Autowired
    private PromotionService promotionService;

    @PostMapping
    public ResponseEntity<PromotionResponse> createPromotion(@RequestBody PromotionRequest promotionRequest) {
        try{
            return ResponseEntity.status(HttpStatus.CREATED)
                    .body(promotionService.createPromotion(promotionRequest));
        }catch (Exception e){
           return  ResponseEntity.badRequest().build();
        }
    }

    @GetMapping("/{id}")
    public ResponseEntity<PromotionResponse> getPromotionById(@PathVariable Integer id) {
        try{
            return ResponseEntity.ok()
                    .body(promotionService.getPromotionById(id));
        }catch (PromotionNotFoundException e){
            return  ResponseEntity.notFound().build();
        }
    }

    @GetMapping
    public ResponseEntity<PageResponse<PromotionResponse>> getPromotions(
            @RequestParam("page") int page,
            @RequestParam("search") String search
    ) {
        try{


            Pageable pageable = PageRequest.of(page-1, 5);
            return ResponseEntity.ok()
                    .body(promotionService.getPromotions(pageable, search));
        }catch(PromotionNotFoundException e){
            return ResponseEntity.notFound().build();
        }
    }

    @PutMapping("/{id}")
    public ResponseEntity<PromotionResponse> updatePromotion(
            @PathVariable int id,
            @RequestBody PromotionRequest promotionRequest) {
        try{
            return ResponseEntity.status(HttpStatus.ACCEPTED)
                    .body(promotionService.updatePromotionById(id, promotionRequest));
        }catch (PromotionNotFoundException e){
            return ResponseEntity.notFound().build();
        }
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Promotion> deletePromotion(
            @PathVariable int id
    ){
        try{
            return ResponseEntity.status(HttpStatus.ACCEPTED)
                    .body(promotionService.deletePromotionById(id));
        }catch (PromotionNotFoundException e){
            return ResponseEntity.notFound().build();
        }
    }
}
