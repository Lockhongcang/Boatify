// wwwroot/js/filter.js

document.addEventListener("DOMContentLoaded", function () {
    // Lấy tất cả .trip-card
    const tripCards = Array.from(document.querySelectorAll(".trip-card"));

    // Checkbox “Giờ đi”
    const timeCheckboxes = Array.from(document.querySelectorAll(".filter-time"));

    // Button “Loại tàu”
    const boatTypeButtons = Array.from(document.querySelectorAll(".filter-boat-type"));

    // Button “Tầng”
    const deckButtons = Array.from(document.querySelectorAll(".filter-deck"));

    // Nút Clear filter (Desktop + Mobile)
    const btnClearDesktop = document.getElementById("clearFilterBtn");
    const btnClearMobile = document.getElementById("clearFilterBtnMobile");

    // 1. Lấy tất cả khoảng giờ đã tick
    function getSelectedTimeRanges() {
        return timeCheckboxes
            .filter(chk => chk.checked)
            .map(chk => {
                const [startH, endH] = chk.value.split("-").map(x => parseInt(x, 10));
                return [startH * 60, endH * 60]; // chuyển sang phút
            });
    }

    // 2. Lấy giá trị Loại tàu (nếu có nút active)
    function getSelectedBoatType() {
        const activeBtn = boatTypeButtons.find(btn => btn.classList.contains("active"));
        return activeBtn ? activeBtn.dataset.boatType : null;
    }

    // 3. Lấy giá trị Tầng (nếu có nút active)
    function getSelectedDeck() {
        const activeBtn = deckButtons.find(btn => btn.classList.contains("active"));
        return activeBtn ? activeBtn.dataset.deck : null;
    }

    // 4. Hàm chính: áp bộ lọc
    function applyFilters() {
        const timeRanges = getSelectedTimeRanges();    // mảng [[startMin,endMin], …]
        const selectedBoatType = getSelectedBoatType(); // string hoặc null
        const selectedDeck = getSelectedDeck();         // "upper", "lower", hoặc null

        tripCards.forEach(card => {
            // Đọc data attributes
            const departMin = parseInt(card.dataset.departMinutes, 10);
            const boatType = card.dataset.boatType;
            const deck = card.dataset.deck; // "upper"/"lower"/"single"

            // 4.1. Kiểm tra điều kiện “Giờ đi”
            let timeMatch = true;
            if (timeRanges.length > 0) {
                timeMatch = timeRanges.some(([start, end]) => {
                    return departMin >= start && departMin < end;
                });
            }

            // 4.2. Kiểm tra điều kiện “Loại tàu”
            let typeMatch = true;
            if (selectedBoatType) {
                typeMatch = (boatType === selectedBoatType);
            }

            // 4.3. Kiểm tra điều kiện “Tầng”
            let deckMatch = true;
            if (selectedDeck) {
                // Nếu chuyến có data-deck = "single" (tức không có tầng), thì không thoả khi chọn "upper"/"lower".
                deckMatch = (deck === selectedDeck);
            }

            // Nếu tất cả 3 điều kiện đều đúng → hiển thị, ngược lại ẩn
            if (timeMatch && typeMatch && deckMatch) {
                card.style.display = "";
            } else {
                card.style.display = "none";
            }
        });
    }

    // 5. Gắn event cho các checkbox “Giờ đi”
    timeCheckboxes.forEach(chk => {
        chk.addEventListener("change", applyFilters);
    });

    // 6. Gắn event cho các button “Loại tàu” (chỉ chọn 1 lần)
    boatTypeButtons.forEach(btn => {
        btn.addEventListener("click", function () {
            // Tắt hết active, rồi toggle chính nó
            boatTypeButtons.forEach(x => x.classList.remove("active"));
            this.classList.toggle("active");
            applyFilters();
        });
    });

    // 7. Gắn event cho các button “Tầng”
    deckButtons.forEach(btn => {
        btn.addEventListener("click", function () {
            // Tắt hết active, rồi toggle chính nó
            deckButtons.forEach(x => x.classList.remove("active"));
            this.classList.toggle("active");
            applyFilters();
        });
    });

    // 8. Clear filter
    function clearAllFilters() {
        timeCheckboxes.forEach(chk => chk.checked = false);
        boatTypeButtons.forEach(btn => btn.classList.remove("active"));
        deckButtons.forEach(btn => btn.classList.remove("active"));
        applyFilters();
    }

    if (btnClearDesktop) {
        btnClearDesktop.addEventListener("click", clearAllFilters);
    }
    if (btnClearMobile) {
        btnClearMobile.addEventListener("click", clearAllFilters);
    }

    // 9. Chạy filter lần đầu (khi load page)
    applyFilters();
});
