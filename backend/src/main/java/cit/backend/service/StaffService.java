package cit.backend.service;

import cit.backend.dto.request.StaffUpdateDTO;
import cit.backend.dto.respone.PageResponse;
import cit.backend.dto.respone.StaffResponse;
import cit.backend.exception.StaffNotFoundException;
import cit.backend.mapper.StaffMapper;
import cit.backend.model.Staff;
import cit.backend.repository.StaffRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;
import org.springframework.web.bind.annotation.PathVariable;


@Service
public class StaffService {

    @Autowired
    private StaffRepository staffRepository;

    @Autowired
    private StaffMapper staffMapper;
    @Autowired
    private PasswordEncoder passwordEncoder;

    public PageResponse<StaffResponse> getAllStaff(Pageable pageable){
        Page<Staff> pageStaff = staffRepository.findAll(pageable);

        PageResponse<StaffResponse> response = new PageResponse<>();

        response.setData(staffMapper.toResponseList(pageStaff.getContent()));
        response.setPage(pageStaff.getNumber() + 1) ;
        response.setTotalCount(pageStaff.getTotalElements());
        response.setTotalPages(pageStaff.getTotalPages());

        return response;
    }

    public PageResponse<StaffResponse> getStaffByName(String name, Pageable pageable){
        Page<Staff> pageStaff = staffRepository.findStaffByUsername(name, pageable);

        PageResponse<StaffResponse> response = new PageResponse<>();

        response.setData(staffMapper.toResponseList(pageStaff.getContent()));
        response.setPage(pageStaff.getNumber() + 1) ;
        response.setTotalCount(pageStaff.getTotalElements());
        response.setTotalPages(pageStaff.getTotalPages());


        return response;
    }

    public StaffResponse getStaffById(int id){
        Staff staff = staffRepository.findById(id).
        orElseThrow(()-> new StaffNotFoundException("Staff not found " + id));

        StaffResponse response = staffMapper.toResponse(staff);

        return response;
    }

    public StaffResponse updateStaff(int id,StaffUpdateDTO staffRequest){
        Staff staff = staffRepository.findById(id)
                .orElseThrow(()-> new StaffNotFoundException("Staff not found " + id));

        if(staffRequest.getUsername() != null) staff.setUsername(staffRequest.getUsername());
        if(staffRequest.getPassword() != null) {
            String hashedPassword = passwordEncoder.encode(staffRequest.getPassword());
            staff.setPassword(hashedPassword); //luu y
        }
        if(staffRequest.getRole() != null) staff.setRole(staffRequest.getRole());
        if(staffRequest.getPhone() != null) staff.setPhone(staffRequest.getPhone());

        return staffMapper.toResponse(staffRepository.save(staff));
    }

    public StaffResponse deleteStaff(int id){
        Staff staff = staffRepository.findById(id)
                .orElseThrow(()-> new StaffNotFoundException("Staff not found " + id));

        staffRepository.deleteById(id);
        return staffMapper.toResponse(staff);
    }


}
