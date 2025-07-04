package cit.backend.service;

import cit.backend.dto.request.PromotionRequest;
import cit.backend.dto.request.PromotionUpdateRequest;
import cit.backend.dto.respone.PageResponse;
import cit.backend.dto.respone.PromotionResponse;
import cit.backend.exception.PromotionNotFoundException;
import cit.backend.mapper.PromotionMapper;
import cit.backend.model.Promotion;
import cit.backend.repository.PromotionRepository;
import jakarta.persistence.EntityNotFoundException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;

@Service
public class PromotionService {
    @Autowired
    private PromotionRepository promotionRepository;

    @Autowired
    private PromotionMapper promotionMapper;

    public PromotionResponse createPromotion(PromotionRequest promotionRequest) {
        Promotion promotion = promotionMapper.toPromotion(promotionRequest);
        Promotion result = promotionRepository.save(promotion);
        return promotionMapper.toResponse(result);
    }

    public PromotionResponse getPromotionById(Integer id) {
        Promotion promotion = promotionRepository.findById(id)
                .orElseThrow(() -> new PromotionNotFoundException("Promotion with id " + id + " not found"));
        return promotionMapper.toResponse(promotion);
    }

    public PageResponse<PromotionResponse> getPromotions(Pageable pageable, String search) {
        Page<Promotion> pagePromotions = promotionRepository.findByNameContainingIgnoreCase(search, pageable);

        PageResponse<PromotionResponse> pageResponse = new PageResponse<>();
        pageResponse.setPage(pagePromotions.getNumber() + 1);
        pageResponse.setTotalCount(pagePromotions.getTotalElements());
        pageResponse.setTotalPages(pagePromotions.getTotalPages());
        pageResponse.setData(promotionMapper.toResponseList(pagePromotions.getContent()));
        return pageResponse;
    }

    public PromotionResponse updatePromotionById(int id, PromotionUpdateRequest promotionRequest) {
        Promotion promotion = promotionRepository.findById(id)
                .orElseThrow(() -> new PromotionNotFoundException("Promotion with id " + id + " not found"));

        System.out.println("Hello ");

        if (promotionRequest.getName() != null) {
            promotion.setName(promotionRequest.getName());
        }

        if (promotionRequest.getType() != null) {
            promotion.setType(promotionRequest.getType());
        }

        if (promotionRequest.getValue() != null) {
            promotion.setValue(promotionRequest.getValue());
        }

        if (promotionRequest.getMax_uses() != null) {
            promotion.setMax_uses(promotionRequest.getMax_uses());
        }

        if (promotionRequest.getUsed_count() != null) {
            promotion.setUsed_count(promotionRequest.getUsed_count());
        }

        if (promotionRequest.getMin_order_amount() != null) {
            promotion.setMin_order_amount(promotionRequest.getMin_order_amount());
        }

        if (promotionRequest.getStartDate() != null) {
            promotion.setStartDate(promotionRequest.getStartDate());
        }

        if (promotionRequest.getEndDate() != null) {
            promotion.setEndDate(promotionRequest.getEndDate());
        }

        if (promotionRequest.getStatus() != null) {
            promotion.setStatus(promotionRequest.getStatus());
        }

        System.out.println("Bye ");

        promotionRepository.save(promotion);
        return promotionMapper.toResponse(promotion);
    }

    public Promotion deletePromotionById(int id) {
        Promotion promotion = promotionRepository.findById(id)
                .orElseThrow(() -> new PromotionNotFoundException("Promotion with id " + id + " not found"));
        promotionRepository.delete(promotion);
        return promotion;
    }
}
