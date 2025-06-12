// Checkout functionality
let bookingData = {};
let selectedPaymentMethod = '';

function initializeCheckout(data) {
    bookingData = data;
    
    // Display booking summary
    displayBookingSummary();
    
    // Display order details
    displayOrderDetails();
    
    // Setup event listeners
    setupEventListeners();
    
    // Load user contact info if available
    loadUserContactInfo();
}

function displayBookingSummary() {
    const summaryContainer = document.getElementById('booking-summary');
    
    const html = `
        <div class="trip-info">
            <div class="row">
                <div class="col-md-8">
                    <h6 class="trip-route">${bookingData.departure} → ${bookingData.destination}</h6>
                    <p class="trip-date">Ngày đi: ${formatDate(bookingData.departDate)}</p>
                </div>
                <div class="col-md-4 text-end">
                    <span class="seat-count">${bookingData.selectedSeats.length} ghế</span>
                </div>
            </div>
        </div>
        
        <div class="passenger-list">
            <h6>Danh sách hành khách:</h6>
            ${bookingData.selectedSeats.map(seat => `
                <div class="passenger-item">
                    <div class="d-flex justify-content-between">
                        <div>
                            <strong>Ghế ${seat.seatCode}</strong> - ${seat.name}
                            <br><small class="text-muted">${seat.phone}</small>
                        </div>
                        <div class="text-end">
                            <span class="price">${formatCurrency(seat.price)}</span>
                        </div>
                    </div>
                </div>
            `).join('')}
        </div>
    `;
    
    summaryContainer.innerHTML = html;
}

function displayOrderDetails() {
    const detailsContainer = document.getElementById('order-details');
    const subtotal = bookingData.totalAmount;
    const paymentFee = calculatePaymentFee(subtotal, selectedPaymentMethod);
    const total = subtotal + paymentFee;
    
    document.getElementById('subtotal').textContent = formatCurrency(subtotal);
    document.getElementById('payment-fee').textContent = formatCurrency(paymentFee);
    document.getElementById('total-amount').textContent = formatCurrency(total);
    
    const html = `
        <div class="route-info">
            <strong>${bookingData.departure} → ${bookingData.destination}</strong>
            <br><small>${formatDate(bookingData.departDate)}</small>
        </div>
        <div class="seat-info mt-2">
            <small>Ghế: ${bookingData.selectedSeats.map(s => s.seatCode).join(', ')}</small>
        </div>
    `;
    
    detailsContainer.innerHTML = html;
}

function setupEventListeners() {
    // Payment method selection
    const paymentMethods = document.querySelectorAll('input[name="paymentMethod"]');
    paymentMethods.forEach(method => {
        method.addEventListener('change', function() {
            selectedPaymentMethod = this.value;
            updatePaymentMethodUI();
            displayOrderDetails(); // Recalculate fees
            validateForm();
        });
    });
    
    // Terms checkbox
    const agreeTerms = document.getElementById('agreeTerms');
    agreeTerms.addEventListener('change', validateForm);
    
    // Contact form validation
    const contactInputs = ['contactName', 'contactEmail', 'contactPhone'];
    contactInputs.forEach(inputId => {
        const input = document.getElementById(inputId);
        input.addEventListener('input', validateForm);
        input.addEventListener('blur', validateContactField);
    });
    
    // Process payment button
    document.getElementById('processPayment').addEventListener('click', processPayment);
}

function updatePaymentMethodUI() {
    // Remove active class from all payment methods
    document.querySelectorAll('.payment-method').forEach(method => {
        method.classList.remove('active');
    });
    
    // Add active class to selected method
    const selectedMethod = document.querySelector(`[data-method="${selectedPaymentMethod}"]`);
    if (selectedMethod) {
        selectedMethod.classList.add('active');
    }
}

function validateForm() {
    const contactName = document.getElementById('contactName').value.trim();
    const contactEmail = document.getElementById('contactEmail').value.trim();
    const contactPhone = document.getElementById('contactPhone').value.trim();
    const agreeTerms = document.getElementById('agreeTerms').checked;
    const hasPaymentMethod = selectedPaymentMethod !== '';
    
    const isValid = contactName && contactEmail && contactPhone && agreeTerms && hasPaymentMethod;
    
    document.getElementById('processPayment').disabled = !isValid;
    
    return isValid;
}

function validateContactField(event) {
    const field = event.target;
    const value = field.value.trim();
    
    // Remove existing validation classes
    field.classList.remove('is-valid', 'is-invalid');
    
    let isValid = false;
    
    switch (field.id) {
        case 'contactName':
            isValid = value.length >= 2;
            break;
        case 'contactEmail':
            isValid = /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value);
            break;
        case 'contactPhone':
            isValid = /^[0-9]{10,11}$/.test(value.replace(/\s/g, ''));
            break;
    }
    
    field.classList.add(isValid ? 'is-valid' : 'is-invalid');
}

function calculatePaymentFee(amount, paymentMethod) {
    switch (paymentMethod) {
        case 'vnpay':
            return amount * 0.01; // 1% fee
        case 'momo':
            return amount * 0.015; // 1.5% fee
        case 'banking':
        case 'cash':
            return 0;
        default:
            return 0;
    }
}

async function processPayment() {
    if (!validateForm()) {
        return;
    }
    
    const button = document.getElementById('processPayment');
    const spinner = document.getElementById('paymentSpinner');
    
    // Show loading state
    button.disabled = true;
    spinner.classList.remove('d-none');
    button.innerHTML = '<span class="spinner-border spinner-border-sm"></span> Đang xử lý...';
    
    try {
        const requestData = {
            bookingData: bookingData,
            paymentMethod: selectedPaymentMethod,
            contactInfo: {
                name: document.getElementById('contactName').value.trim(),
                email: document.getElementById('contactEmail').value.trim(),
                phone: document.getElementById('contactPhone').value.trim()
            }
        };
        
        const response = await fetch('/Checkout/ProcessOrder', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(requestData)
        });
        
        const result = await response.json();
        
        if (result.success) {
            // Clear booking data from session storage
            sessionStorage.removeItem('bookingData');
            
            if (result.paymentUrl) {
                // Redirect to payment gateway
                window.location.href = result.paymentUrl;
            } else if (result.redirectUrl) {
                // Direct redirect to success page (for test payments)
                window.location.href = result.redirectUrl;
            } else {
                // Show success message and redirect
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công!',
                    text: result.message,
                    confirmButtonText: 'Xem đơn hàng'
                }).then(() => {
                    window.location.href = `/Checkout/Success?orderId=${result.orderId}`;
                });
            }
        } else {
            throw new Error(result.message || 'Có lỗi xảy ra khi xử lý thanh toán');
        }
    } catch (error) {
        console.error('Payment processing error:', error);
        
        Swal.fire({
            icon: 'error',
            title: 'Lỗi thanh toán',
            text: error.message || 'Có lỗi xảy ra khi xử lý thanh toán. Vui lòng thử lại.',
            confirmButtonText: 'Thử lại'
        });
    } finally {
        // Reset button state
        button.disabled = false;
        spinner.classList.add('d-none');
        button.innerHTML = 'Thanh toán';
    }
}

function loadUserContactInfo() {
    // This would typically load from user session or API
    // For now, we'll leave the fields empty for user to fill
}

function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString('vi-VN', {
        weekday: 'long',
        year: 'numeric',
        month: 'long',
        day: 'numeric'
    });
}

function formatCurrency(amount) {
    return new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
    }).format(amount);
}
