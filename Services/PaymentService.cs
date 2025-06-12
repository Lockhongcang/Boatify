using Boatify.Models;
using Boatify.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Boatify.Services
{
    public class PaymentService
    {
        private readonly BoatifyContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PaymentService> _logger;
        private readonly HttpClient _httpClient;

        public PaymentService(
            BoatifyContext context,
            IConfiguration configuration,
            ILogger<PaymentService> logger,
            IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
        {
            try
            {
                _logger.LogInformation("🔄 Processing payment for order {OrderId} with method {PaymentMethod}",
                    request.OrderId, request.PaymentMethod);

                // Validate request
                if (request.OrderId <= 0)
                {
                    _logger.LogError("❌ Invalid OrderId: {OrderId}", request.OrderId);
                    return new PaymentResult { Success = false, ErrorMessage = "Mã đơn hàng không hợp lệ." };
                }

                if (request.Amount <= 0)
                {
                    _logger.LogError("❌ Invalid Amount: {Amount}", request.Amount);
                    return new PaymentResult { Success = false, ErrorMessage = "Số tiền thanh toán không hợp lệ." };
                }

                if (string.IsNullOrEmpty(request.PaymentMethod))
                {
                    _logger.LogError("❌ Payment method is empty");
                    return new PaymentResult { Success = false, ErrorMessage = "Phương thức thanh toán không được chọn." };
                }

                // Check if we're in test mode
                var testMode = _configuration.GetValue<bool>("PaymentSettings:TestMode", true);
                _logger.LogInformation("💡 Payment mode: {Mode}", testMode ? "Test Mode" : "Production Mode");

                if (testMode)
                {
                    _logger.LogInformation("Running in test mode - using simulated payments");
                    switch (request.PaymentMethod.ToLower())
                    {
                        case "vnpay":
                            return await ProcessTestPaymentAsync(request, "VNPay");
                        case "momo":
                            return await ProcessTestPaymentAsync(request, "MoMo");
                        case "banking":
                            return await ProcessBankingPaymentAsync(request);
                        case "cash":
                            return await ProcessCashPaymentAsync(request);
                        default:
                            return new PaymentResult { Success = false, ErrorMessage = "Phương thức thanh toán không được hỗ trợ." };
                    }
                }
                else
                {
                    _logger.LogInformation("Running in production mode - using real payment gateways");
                    switch (request.PaymentMethod.ToLower())
                    {
                        case "vnpay":
                            return await ProcessVNPayPaymentAsync(request);
                        case "momo":
                            return await ProcessMoMoPaymentAsync(request);
                        case "banking":
                            return await ProcessBankingPaymentAsync(request);
                        case "cash":
                            return await ProcessCashPaymentAsync(request);
                        default:
                            return new PaymentResult { Success = false, ErrorMessage = "Phương thức thanh toán không được hỗ trợ." };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment for order {OrderId}", request.OrderId);
                return new PaymentResult { Success = false, ErrorMessage = "Có lỗi xảy ra khi xử lý thanh toán." };
            }
        }

        private async Task<PaymentResult> ProcessTestPaymentAsync(PaymentRequest request, string gateway)
        {
            try
            {
                _logger.LogInformation("🧪 Processing test payment for order {OrderId} via {Gateway}",
                    request.OrderId, gateway);

                // Simulate a successful payment for testing
                await Task.Delay(500); // Simulate processing time

                var transactionId = $"{gateway}_{DateTime.Now.Ticks}";

                _logger.LogInformation("✅ Test payment successful for order {OrderId} via {Gateway}. TransactionId: {TransactionId}",
                    request.OrderId, gateway, transactionId);

                return new PaymentResult
                {
                    Success = true,
                    TransactionId = transactionId,
                    PaymentUrl = null, // No redirect needed for test payments
                    Message = $"Thanh toán thành công qua {gateway} (Chế độ test)"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error in test payment for order {OrderId}", request.OrderId);
                return new PaymentResult
                {
                    Success = false,
                    ErrorMessage = "Lỗi trong quá trình test thanh toán."
                };
            }
        }

        private async Task<PaymentResult> ProcessVNPayPaymentAsync(PaymentRequest request)
        {
            try
            {
                var vnpayConfig = _configuration.GetSection("VNPay");
                var tmnCode = vnpayConfig["TmnCode"] ?? "";
                var hashSecret = vnpayConfig["HashSecret"] ?? "";
                var baseUrl = vnpayConfig["BaseUrl"] ?? "";
                var returnUrl = vnpayConfig["ReturnUrl"] ?? "";

                var vnpay = new VNPayLibrary();
                vnpay.AddRequestData("vnp_Version", "2.1.0");
                vnpay.AddRequestData("vnp_Command", "pay");
                vnpay.AddRequestData("vnp_TmnCode", tmnCode);
                vnpay.AddRequestData("vnp_Amount", ((long)(request.Amount * 100)).ToString());
                vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
                vnpay.AddRequestData("vnp_CurrCode", "VND");
                vnpay.AddRequestData("vnp_IpAddr", "127.0.0.1");
                vnpay.AddRequestData("vnp_Locale", "vn");
                vnpay.AddRequestData("vnp_OrderInfo", request.OrderInfo);
                vnpay.AddRequestData("vnp_OrderType", "other");
                vnpay.AddRequestData("vnp_ReturnUrl", returnUrl);
                vnpay.AddRequestData("vnp_TxnRef", request.OrderId.ToString());

                var paymentUrl = vnpay.CreateRequestUrl(baseUrl, hashSecret);

                // Create payment record
                await CreatePaymentRecordAsync(request.OrderId, "VNPay", request.Amount, "Pending");

                return new PaymentResult
                {
                    Success = true,
                    PaymentUrl = paymentUrl,
                    TransactionId = request.OrderId.ToString()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing VNPay payment");
                return new PaymentResult { Success = false, ErrorMessage = "Lỗi khi tạo thanh toán VNPay." };
            }
        }

        private async Task<PaymentResult> ProcessMoMoPaymentAsync(PaymentRequest request)
        {
            try
            {
                var momoConfig = _configuration.GetSection("MoMo");
                var partnerCode = momoConfig["PartnerCode"] ?? "";
                var accessKey = momoConfig["AccessKey"] ?? "";
                var secretKey = momoConfig["SecretKey"] ?? "";
                var endpoint = momoConfig["Endpoint"] ?? "";
                var returnUrl = momoConfig["ReturnUrl"] ?? "";
                var notifyUrl = momoConfig["NotifyUrl"] ?? "";

                var orderId = request.OrderId.ToString();
                var requestId = Guid.NewGuid().ToString();
                var amount = request.Amount.ToString("0");
                var orderInfo = request.OrderInfo;
                var requestType = "captureWallet";
                var extraData = "";

                // Create signature
                var rawSignature = $"accessKey={accessKey}&amount={amount}&extraData={extraData}&ipnUrl={notifyUrl}&orderId={orderId}&orderInfo={orderInfo}&partnerCode={partnerCode}&redirectUrl={returnUrl}&requestId={requestId}&requestType={requestType}";
                var signature = ComputeHmacSha256(rawSignature, secretKey);

                var requestData = new
                {
                    partnerCode,
                    accessKey,
                    requestId,
                    amount,
                    orderId,
                    orderInfo,
                    redirectUrl = returnUrl,
                    ipnUrl = notifyUrl,
                    extraData,
                    requestType,
                    signature,
                    lang = "vi"
                };

                var jsonRequest = System.Text.Json.JsonSerializer.Serialize(requestData);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(endpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var momoResponse = System.Text.Json.JsonSerializer.Deserialize<MoMoResponse>(responseContent);
                    
                    if (momoResponse?.ResultCode == 0)
                    {
                        await CreatePaymentRecordAsync(request.OrderId, "MoMo", request.Amount, "Pending");
                        
                        return new PaymentResult
                        {
                            Success = true,
                            PaymentUrl = momoResponse.PayUrl,
                            TransactionId = requestId
                        };
                    }
                }

                return new PaymentResult { Success = false, ErrorMessage = "Lỗi khi tạo thanh toán MoMo." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing MoMo payment");
                return new PaymentResult { Success = false, ErrorMessage = "Lỗi khi tạo thanh toán MoMo." };
            }
        }

        private async Task<PaymentResult> ProcessBankingPaymentAsync(PaymentRequest request)
        {
            // For banking, we'll create a pending payment and provide bank transfer instructions
            await CreatePaymentRecordAsync(request.OrderId, "Banking", request.Amount, "Pending");

            return new PaymentResult
            {
                Success = true,
                PaymentUrl = $"/Checkout/BankingInstructions?orderId={request.OrderId}",
                TransactionId = request.OrderId.ToString()
            };
        }

        private async Task<PaymentResult> ProcessCashPaymentAsync(PaymentRequest request)
        {
            // For cash payment, create a pending payment record
            await CreatePaymentRecordAsync(request.OrderId, "Cash", request.Amount, "Pending");

            return new PaymentResult
            {
                Success = true,
                PaymentUrl = $"/Checkout/CashInstructions?orderId={request.OrderId}",
                TransactionId = request.OrderId.ToString()
            };
        }

        private async Task CreatePaymentRecordAsync(int orderId, string provider, decimal amount, string status)
        {
            var payment = new Payment
            {
                OrderId = orderId,
                PaymentMethod = provider,
                Amount = amount,
                Status = status,
                CreatedAt = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
        }

        private string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            using (var hmac = new HMACSHA256(keyBytes))
            {
                var hashBytes = hmac.ComputeHash(messageBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        public Task<bool> VerifyPaymentAsync(string provider, Dictionary<string, string> parameters)
        {
            switch (provider.ToLower())
            {
                case "vnpay":
                    return Task.FromResult(VerifyVNPayPayment(parameters));
                case "momo":
                    return Task.FromResult(VerifyMoMoPayment(parameters));
                default:
                    return Task.FromResult(false);
            }
        }

        private bool VerifyVNPayPayment(Dictionary<string, string> parameters)
        {
            var vnpayConfig = _configuration.GetSection("VNPay");
            var hashSecret = vnpayConfig["HashSecret"] ?? "";

            var vnpay = new VNPayLibrary();
            foreach (var param in parameters)
            {
                if (!string.IsNullOrEmpty(param.Value) && param.Key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(param.Key, param.Value);
                }
            }

            var orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            var vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            var vnpSecureHash = parameters["vnp_SecureHash"];
            var vnpResponseCode = vnpay.GetResponseData("vnp_ResponseCode");

            bool checkSignature = vnpay.ValidateSignature(vnpSecureHash, hashSecret);

            return checkSignature && vnpResponseCode == "00";
        }

        private bool VerifyMoMoPayment(Dictionary<string, string> parameters)
        {
            // Implement MoMo payment verification logic
            return parameters.ContainsKey("resultCode") && parameters["resultCode"] == "0";
        }
    }
}
