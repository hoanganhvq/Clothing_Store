package cit.backend.dto.respone;

import lombok.Data;
import java.util.List;

@Data
public class PagedResponse<T> {
    private List<T> data;
    private int totalPages;
}