// Set default payment method to Credit Card on page load //
document.addEventListener('DOMContentLoaded', () => {
    setPaymethod('Credit Card');
});


// Toggle Paymethod and show item inside it //
function setPaymethod(method) { // onclick="setPaymethod(method)" //

    document.getElementById("SelectedPaymethod").value = method;

    // Hide all sections initially
    document.getElementById('creditcard_detail').style.display = 'none';
    document.getElementById('banktransfer_detail').style.display = 'none';
    document.getElementById('promptpay_detail').style.display = 'none';

    // Show the selected payment method section
    if (method === 'Credit Card') {
        document.getElementById('creditcard_detail').style.display = 'block';
    } else if (method === 'Bank transfer') {
        document.getElementById('banktransfer_detail').style.display = 'block';
    } else if (method === 'Prompt Pay') {
        document.getElementById('promptpay_detail').style.display = 'block';
    }
};

function updateCreditCardType() {
    // Get the selected value from the dropdown
    const selectedCardType = document.getElementById('cardTypeSelect').value;
    // Set the value of the hidden input to the selected value
    document.getElementById('SelectedCardType').value = selectedCardType;
}
function updateBankacc() {

    const selectedCardType = document.getElementById('bankaccselect').value;
    document.getElementById('SelectedBankAcc').value = selectedCardType;
}