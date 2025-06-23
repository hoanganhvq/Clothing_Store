package cit.backend.exception;

public class UserAlreadyExit extends RuntimeException {
  public static final long serialVersionUID = 1L;
    public UserAlreadyExit(String message) {

        super( message);
    }
}
