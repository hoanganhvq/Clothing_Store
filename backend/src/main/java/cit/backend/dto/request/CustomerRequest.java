package cit.backend.dto.request;

import jakarta.validation.constraints.Email;
import jakarta.validation.constraints.Min;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.Pattern;
import jakarta.validation.constraints.Size;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
public class CustomerRequest {

    @NotBlank(message = "Customer name must not be blank")
    private String name;

    @NotBlank(message = "Phone number must not be blank")
    @Size(min = 9, max = 15, message = "Phone number must be between 9 and 15 characters")
    @Pattern(regexp = "^[0-9+\\-()\\s]*$", message = "Phone number contains invalid characters")
    private String phone;

    @NotBlank(message = "Email must not be blank")
    @Email(message = "Email is invalid")
    private String email;

    @Min(value = 0, message = "Point must be a non-negative number")
    private int point = 0;
}
