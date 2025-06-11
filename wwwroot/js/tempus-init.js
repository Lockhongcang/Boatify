function initDatepicker(el) {
    const picker = new tempusDominus.TempusDominus(el, {
        display: {
            // Chỉ hiện calendar, tắt toàn bộ phần giờ
            components: {
                calendar: true,
                clock: false,
                hours: false,
                minutes: false,
                seconds: false
            }
        },
        localization: {
            // Ngôn ngữ và format parse/hiển thị
            locale: 'vi',
            format: 'dd/MM/yyyy'
        }
    });

    // Override formatInput: date ở đây là JS Date
    picker.dates.formatInput = date => {
        // Chỉ lấy phần ngày theo locale vi
        return date.toLocaleDateString('vi-VN');
    };
}

// Đăng ký toàn cục để có thể re-init khi thêm DOM động
window.initDatepicker = initDatepicker;

// Khởi tạo cho tất cả wrapper .input-group.datetimepicker trên trang
document.addEventListener('DOMContentLoaded', () => {
    document.querySelectorAll('.input-group.datetimepicker').forEach(initDatepicker);
});
