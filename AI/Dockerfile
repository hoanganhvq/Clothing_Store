# Sử dụng image base của Python
FROM python:3.10-slim

# Đặt thư mục làm việc
WORKDIR /app

# Copy file requirements.txt và cài đặt các thư viện cần thiết
COPY requirements.txt /app/
RUN pip install -r requirements.txt

# Copy mã nguồn Python vào container
COPY . /app/

# Expose port (nếu cần)
EXPOSE 5000

# Chạy ứng dụng AI (Ví dụ chạy app Flask hoặc FastAPI)
CMD ["python", "main.py"]
