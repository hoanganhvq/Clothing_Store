package cit.backend.exception;

public class CustomerEmailNotFound extends RuntimeException {
  public CustomerEmailNotFound(String message) {
    super(message);
  }
}
