// wwwroot/js/ticketbox.js

document.addEventListener("DOMContentLoaded", function () {
    // 1. Lấy các element chung
    const departure = document.querySelector("select[name='departure']");
    const destination = document.querySelector("select[name='destination']");
    const swapBtn = document.querySelector(".swap_horiz");
    const form = document.querySelector("form#ticketForm") || document.querySelector("form");

    // 2. Lấy input ngày đi & ngày về (theo tên bạn đặt trong view: name="date", name="returnDate")
    const dateInput = document.querySelector("input[name='date']");
    const returnDateInput = document.querySelector("input[name='returnDate']");
    const returnDateGroup = document.getElementById("returnDateGroup");

    // 3. Radio Một chiều / Khứ hồi
    const oneWay = document.getElementById("oneWay");
    const roundTrip = document.getElementById("roundTrip");

    if (!departure || !destination || !dateInput || !form) return;

    // Helper: "dd/MM/yyyy" → "yyyy-MM-dd"
    function toIso(str) {
        const parts = str.split("/");
        if (parts.length !== 3) return str;
        const [d, m, y] = parts.map(p => p.padStart(2, "0"));
        return `${y}-${m}-${d}`;
    }

    // 4. Toggle ẩn/disable ngày về khi Một chiều
    function toggleReturnDate() {
        const iconSpan = returnDateGroup.querySelector("span.input-group-text");
        if (oneWay.checked) {
            returnDateInput.setAttribute("disabled", "disabled");
            iconSpan.removeAttribute("data-td-toggle");
            returnDateInput.value = "";
        } else {
            returnDateInput.removeAttribute("disabled");
            iconSpan.setAttribute("data-td-toggle", "datetimepicker");
            if (!returnDateInput.value) {
                // gán ngày mai theo ISO, picker sẽ override format UI
                const tm = new Date(Date.now() + 864e5).toISOString().slice(0, 10);
                returnDateInput.value = tm;
            }
        }
    }

    // 5. Lưu lịch sử tìm kiếm (bao gồm returnDate nếu Khứ hồi)
    function saveHistory() {
        const current = {
            from: departure.value,
            to: destination.value,
            date: dateInput.value,
            returnDate: roundTrip.checked ? returnDateInput.value : ""
        };
        if (current.from && current.to && current.date) {
            let history = JSON.parse(localStorage.getItem("searchHistory") || "[]");
            // loại bỏ duplicate
            history = history.filter(h =>
                !(h.from === current.from && h.to === current.to && h.date === current.date && h.returnDate === current.returnDate)
            );
            history.unshift(current);
            localStorage.setItem("searchHistory", JSON.stringify(history.slice(0, 5)));
        }
    }

    // 6. Load lịch sử vào select và set giá trị lần cuối
    function loadHistory() {
        const history = JSON.parse(localStorage.getItem("searchHistory") || "[]");
        [departure, destination].forEach(sel => {
            const existing = new Set([...sel.options].map(o => o.value));
            history.forEach(h => {
                if (!existing.has(h.from)) { sel.add(new Option(h.from, h.from)); existing.add(h.from); }
                if (!existing.has(h.to)) { sel.add(new Option(h.to, h.to)); existing.add(h.to); }
            });
        });
        if (history.length) {
            const last = history[0];
            departure.value = last.from;
            destination.value = last.to;
            dateInput.value = last.date;
            if (!oneWay.checked && last.returnDate) {
                returnDateInput.value = last.returnDate;
            }
        }
    }

    // 7. Render Recent Searches
    function renderRecent() {
        const container = document.getElementById("recentSearchList");
        if (!container) return;
        const history = JSON.parse(localStorage.getItem("searchHistory") || "[]");
        container.innerHTML = "";
        history.forEach(h => {
            const btn = document.createElement("button");
            btn.type = "button";
            btn.className = "btn cancel-button me-2 fw-medium text-start";
            btn.innerHTML = `
        ${h.from} - ${h.to}<br>
        <small class="fw-normal date-search">
          ${h.date}${h.returnDate ? " → " + h.returnDate : ""}
        </small>`;
            btn.addEventListener("click", () => {
                departure.value = h.from;
                destination.value = h.to;
                dateInput.value = h.date;
                if (!oneWay.checked && h.returnDate) {
                    returnDateInput.value = h.returnDate;
                }
            });
            container.appendChild(btn);
        });
    }

    // 8. Swap departure/destination
    swapBtn?.addEventListener("click", () => {
        [departure.value, destination.value] = [destination.value, departure.value];
        swapBtn.classList.toggle("rotate");
    });

    // 9. Trước submit: format về ISO + saveHistory
    form.addEventListener("submit", function () {
        dateInput.value = toIso(dateInput.value);
        if (!oneWay.checked) {
            returnDateInput.value = toIso(returnDateInput.value);
        }
        saveHistory();
    });

    // 10. Gắn sự kiện radio và khởi tạo ban đầu
    oneWay.addEventListener("change", toggleReturnDate);
    roundTrip.addEventListener("change", toggleReturnDate);
    toggleReturnDate();

    // 11. Khởi tạo lịch sử và recent
    loadHistory();
    renderRecent();
});
