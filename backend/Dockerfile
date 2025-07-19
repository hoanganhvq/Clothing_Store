# Sử dụng JDK 21 image
FROM openjdk:21-jdk-slim

# Đặt thư mục làm việc
WORKDIR /app

# Copy file JAR vào container
COPY target/backend-0.0.1-SNAPSHOT.jar app.jar

# Expose port 8080 (hoặc port bạn dùng trong ứng dụng)
EXPOSE 8080

# Chạy ứng dụng Spring Boot
ENTRYPOINT ["java", "-jar", "app.jar"]
