package cit.backend.dto.request;


import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
public class CustomerRequest {

    private int id;

    private String fname;

    private String lname;

    private String phone;

    private String email;

    private String address;

    private int point = 0;

}
