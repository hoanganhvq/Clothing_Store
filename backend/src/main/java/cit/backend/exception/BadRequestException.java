package cit.backend.exception;

        public class BadRequestException extends RuntimeException {
            private static final long serialVersionUID = 1L;

            public BadRequestException() {
                super("Request is malformed.");
            }

            public BadRequestException(String message) {
                super("Request is malformed: " + message);
            }
        }