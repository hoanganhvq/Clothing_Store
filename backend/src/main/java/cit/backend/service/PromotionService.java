package cit.backend.service;

import cit.backend.dto.request.PromotionRequest;
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
        pageResponse.setPage(pagePromotions.getNumber());
        pageResponse.setTotalCount(pagePromotions.getTotalElements());
        pageResponse.setTotalPages(pagePromotions.getTotalPages());
        pageResponse.setData(promotionMapper.toResponseList(pagePromotions.getContent()));
        return pageResponse;
    }

    public PromotionResponse updatePromotionById(int id, PromotionRequest promotionRequest) {
        Promotion promotion = promotionRepository.findById(id)
                .orElseThrow(() -> new PromotionNotFoundException("Promotion with id " + id + " not found"));
        Promotion result = promotionMapper.toPromotion(promotionRequest);
        promotionRepository.save(promotion);
        return promotionMapper.toResponse(result);
    }

    public Promotion deletePromotionById(int id) {
        Promotion promotion = promotionRepository.findById(id)
                .orElseThrow(() -> new PromotionNotFoundException("Promotion with id " + id + " not found"));
        promotionRepository.delete(promotion);
        return promotion;
    }
}
