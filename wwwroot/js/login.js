document.addEventListener("DOMContentLoaded", function () {
    const state = '@TempData["FormState"]';
    if (state === 'register') {
        document.querySelector('.container')?.classList.add('active');
    } else {
        document.querySelector('.container')?.classList.remove('active');
    }
});