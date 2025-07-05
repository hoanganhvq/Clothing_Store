package cit.backend.mapper;

import cit.backend.dto.respone.StaffResponse;
import cit.backend.model.Staff;
import org.mapstruct.Mapper;

import java.util.List;

@Mapper(componentModel = "spring")
public interface StaffMapper {
    StaffResponse toResponse(Staff staff);
    List<StaffResponse> toResponseList(List<Staff> staffList);
}
