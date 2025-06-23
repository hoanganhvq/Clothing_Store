package cit.backend.dto.respone;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.List;

@Data
@AllArgsConstructor
@NoArgsConstructor
public class PageProductResponse<T> {
    public int page;

    public int totalItems;

    public int totalPages;

    public List<T> data;
}
