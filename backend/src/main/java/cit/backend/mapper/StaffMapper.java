package cit.backend.mapper;

import cit.backend.Enum.Role;
import cit.backend.dto.request.RegisterRequest;
import cit.backend.dto.request.StaffRequest;
import cit.backend.dto.respone.StaffResponse;
import cit.backend.model.Staff;
import jakarta.persistence.*;
import org.mapstruct.Mapper;

import java.util.List;

@Mapper(componentModel = "spring")
public interface StaffMapper {
    StaffResponse toResponse(Staff staff);
    Staff toModel (StaffRequest staffRequest);
    List<StaffResponse> toResponseList(List<Staff> staffList);
}
