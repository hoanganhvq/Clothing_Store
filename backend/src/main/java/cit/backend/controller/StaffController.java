package cit.backend.controller;


import cit.backend.dto.request.RegisterRequest;
import cit.backend.dto.request.StaffUpdateDTO;
import cit.backend.dto.respone.AuthRespone;
import cit.backend.dto.respone.PageResponse;
import cit.backend.dto.respone.StaffResponse;
import cit.backend.mapper.StaffMapperImpl;
import cit.backend.repository.StaffRepository;
import cit.backend.service.AuthService;
import cit.backend.service.StaffService;
import jakarta.validation.Valid;
import jakarta.websocket.server.PathParam;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.*;


@RequestMapping("staff")
@RestController
@Validated
public class StaffController {
    @Autowired
    private StaffService staffService;

    @Autowired
    private AuthService authService;

    @GetMapping()
    public PageResponse<StaffResponse> getStaffs(
            @RequestParam(value = "page", defaultValue = "1") int page,
            @RequestParam(value = "search", required = false) String search
    ) {
        Pageable pageable = PageRequest.of(page - 1, 5);

        if (search != null && !search.isEmpty()) {
            return staffService.getStaffByName(search, pageable);
        }

        return staffService.getAllStaff(pageable);
    }

    @GetMapping("/{id}")
    public StaffResponse getStaffById(@PathVariable int id) {
        return staffService.getStaffById(id);
    }

    @PostMapping
    public AuthRespone createStaff(@Valid @RequestBody RegisterRequest registerRequest) {
        return authService.register(registerRequest);
    }

    @PatchMapping("/{id}")
    public StaffResponse updateStaff(
            @PathVariable int id,
            @Valid @RequestBody StaffUpdateDTO staffUpdateDTO
    ) {
        return staffService.updateStaff(id, staffUpdateDTO);
    }

    @DeleteMapping("/{id}")
    public StaffResponse deleteStaff(@PathVariable int id) {
        return staffService.deleteStaff(id);
    }
}
