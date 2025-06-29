package cit.backend.exception;

public class OrderItemNotFound extends RuntimeException {
    public OrderItemNotFound(String message) {
        super(message);
    }
}
