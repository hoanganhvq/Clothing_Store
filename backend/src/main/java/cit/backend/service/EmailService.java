package cit.backend.service;

import jakarta.mail.MessagingException;
import jakarta.mail.internet.MimeMessage;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.mail.javamail.JavaMailSender;
import org.springframework.mail.javamail.MimeMessageHelper;
import org.springframework.stereotype.Service;



@Service
public class EmailService {
    @Autowired
    private JavaMailSender mailSender;

    public void sendEmail(String to, String subject, String text) throws MessagingException {
        MimeMessage mimeMessage = mailSender.createMimeMessage(); //Tao doi tuong email dang MIME. co the chua HTML
        MimeMessageHelper helper = new MimeMessageHelper(mimeMessage, true); //Tao helper de de cau hinh noi dung email
        helper.setFrom("vuaposerver@gmail.com");
        helper.setTo(to);
        helper.setSubject(subject);
        helper.setText(text, true); //Cho gui html
        mailSender.send(mimeMessage);
    }



}
