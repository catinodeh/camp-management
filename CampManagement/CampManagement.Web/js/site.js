// Write your Javascript code.
function formatPhoneNumber(phone) {
    if (phone == null || phone.length != 10) return "";
    return phone.substr(0, 3) + '-' + phone.substr(3, 3) + '-' + phone.substr(6, 4);
}