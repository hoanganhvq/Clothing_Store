package cit.backend.mapper;

import cit.backend.dto.request.PromotionRequest;
import cit.backend.dto.respone.PromotionResponse;
import cit.backend.model.Order;
import cit.backend.model.Promotion;
import org.mapstruct.Mapper;

import java.util.List;

@Mapper(componentModel = "spring")
public interface PromotionMapper {
    PromotionResponse toResponse(Promotion promotion);
    Promotion toPromotion(PromotionRequest promotionRequest);
    List<PromotionResponse> toResponseList(List<Promotion> promotions);
}
