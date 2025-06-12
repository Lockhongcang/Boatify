namespace Boatify.Models.Dto
{
    /// <summary>
    /// Order data model for external API based on documentation
    /// </summary>
    public class ExternalOrderRequest
    {
        public string Booker { get; set; } = string.Empty; // Tên người đặt
        public string ContactNo { get; set; } = string.Empty; // Số điện thoại liên hệ
        public string Email { get; set; } = string.Empty; // Email liên hệ
        public string Buyer { get; set; } = string.Empty; // Tên xuất hóa đơn
        public string Taxcode { get; set; } = string.Empty; // Mã số thuế
        public string CompNm { get; set; } = string.Empty; // Tên công ty
        public string CompAddress { get; set; } = string.Empty; // Địa chỉ xuất hóa đơn
        public int TotalNumber { get; set; } // Số lượng hành khách
        public decimal TotalAmount { get; set; } // Tổng tiền của đơn hàng
        public int BoatId { get; set; } // Mã tàu
        public int VoyageId { get; set; } // Mã chuyến
        public int ScheduleId { get; set; } // Mã lịch trình tàu chạy
        public decimal PaidAmount { get; set; } // Tổng tiền đã thanh toán
        public int RouteId { get; set; } // Mã tuyến
        public string DepartDate { get; set; } = string.Empty; // Ngày khởi hành
        public List<ExternalTicketRequest> Tickets { get; set; } = new List<ExternalTicketRequest>();
    }

    /// <summary>
    /// Ticket data model for external API based on documentation
    /// </summary>
    public class ExternalTicketRequest
    {
        public string IdNo { get; set; } = string.Empty; // CMND/CCCD/hộ chiếu hành khách
        public string FullNm { get; set; } = string.Empty; // Tên hành khách
        public string POB { get; set; } = string.Empty; // Nơi sinh
        public string YOB { get; set; } = string.Empty; // Ngày sinh
        public int TicketTypeId { get; set; } // Loại vé
        public string TicketClass { get; set; } = string.Empty; // Hạng ghế
        public int NationId { get; set; } = 1; // Mã quốc tịch (default Vietnam)
        public string PhoneNo { get; set; } = string.Empty; // Số điện thoại
        public string Email { get; set; } = string.Empty; // Email
        public int PositionId { get; set; } // Mã vị trí ghế ngồi
        public int TicketPriceId { get; set; } // Mã giá vé
        public decimal PriceWithVAT { get; set; } // Đơn giá của vé
        public int No { get; set; } // Số thứ tự trong danh sách đơn hàng
        public bool IsVIP { get; set; } = false; // Vé vip
        public int Gender { get; set; } // Giới tính (0: Nam, 1: Nữ)
    }

    /// <summary>
    /// VNPay payment data model based on documentation
    /// </summary>
    public class VNPayPaymentData
    {
        public long vnp_Amount { get; set; } // Tổng tiền thanh toán (thêm 2 chữ số 00 ở cuối)
        public string vnp_BankCode { get; set; } = string.Empty; // Đơn vị thanh toán
        public string vnp_BankTranNo { get; set; } = string.Empty; // Mã giao dịch
        public string vnp_CardType { get; set; } = string.Empty; // Loại hình thanh toán
        public string vnp_OrderInfo { get; set; } = string.Empty; // Số đơn hàng
        public string vnp_PayDate { get; set; } = string.Empty; // Ngày thanh toán (yyyyMMddHHmm)
        public string vnp_ResponseCode { get; set; } = string.Empty; // Mã lỗi khi thanh toán
        public string vnp_TransactionNo { get; set; } = string.Empty; // Mã giao dịch
        public string vnp_TxnRef { get; set; } = string.Empty; // Số đơn hàng
        public string vnp_SecureHash { get; set; } = string.Empty; // Chuỗi hash
    }

    /// <summary>
    /// API Query parameters based on documentation
    /// </summary>
    public class VoyageGoQuery
    {
        public int RouteId { get; set; } // Mã tuyến đi
        public string DepartDate { get; set; } = string.Empty; // Ngày đi
        public int NoOfPassenger { get; set; } // Số lượng hành khách
    }

    public class VoyageBackQuery
    {
        public int RouteIdTripGo { get; set; } // Mã tuyến đi
        public string DepartDateBack { get; set; } = string.Empty; // Ngày về
        public int NoOfPassenger { get; set; } // Số lượng hành khách
    }

    public class SeatEmptyQuery
    {
        public int VoyageId { get; set; } // Mã chuyến
        public string DepartDate { get; set; } = string.Empty; // Ngày tàu chạy
    }

    public class TicketPriceQuery
    {
        public int RouteId { get; set; } // Mã tuyến
        public int BoatTypeId { get; set; } // Mã loại tàu
        public string DepartDate { get; set; } = string.Empty; // Ngày khởi hành
    }

    /// <summary>
    /// Extended voyage result with additional fields
    /// </summary>
    public class ExtendedVoyageResult : VoyageResult
    {
        public new int BoatTypeId { get; set; }
        public new int ScheduleId { get; set; }
        public new int RouteId { get; set; }
        public decimal BasePrice { get; set; }
        public List<string> AvailableClasses { get; set; } = new List<string>();
        public int AvailableSeats { get; set; }
    }

    /// <summary>
    /// Extended seat result with pricing information
    /// </summary>
    public class ExtendedSeatResult : SeatEmptyResult
    {
        public new int PositionId { get; set; }
        public int TicketPriceId { get; set; }
        public decimal PriceWithVAT { get; set; }
        public int TicketTypeId { get; set; }
        public new bool IsVIP { get; set; }
        public bool IsAvailable { get; set; } = true;
    }

    /// <summary>
    /// Booking summary for order creation
    /// </summary>
    public class BookingSummary
    {
        public int RouteId { get; set; }
        public int VoyageId { get; set; }
        public int ScheduleId { get; set; }
        public int BoatId { get; set; }
        public string DepartDate { get; set; } = string.Empty;
        public string DepartTime { get; set; } = string.Empty;
        public string Departure { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public List<ExternalSelectedSeat> SelectedSeats { get; set; } = new List<ExternalSelectedSeat>();
        public decimal TotalAmount { get; set; }
        public int TotalPassengers { get; set; }
    }

    public class ExternalSelectedSeat
    {
        public int SeatId { get; set; }
        public int PositionId { get; set; }
        public string SeatCode { get; set; } = string.Empty;
        public string TicketClass { get; set; } = string.Empty;
        public int TicketPriceId { get; set; }
        public decimal Price { get; set; }
        public int TicketTypeId { get; set; }
        public bool IsVIP { get; set; }

        // Passenger information
        public string PassengerName { get; set; } = string.Empty;
        public string PassengerIdCard { get; set; } = string.Empty;
        public string PassengerPhone { get; set; } = string.Empty;
        public string PassengerBirthDate { get; set; } = string.Empty;
        public string PassengerBirthPlace { get; set; } = string.Empty;
        public int PassengerGender { get; set; } // 0: Nam, 1: Nữ
    }
}
