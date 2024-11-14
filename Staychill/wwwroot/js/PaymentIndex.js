
// ========================================================== Selected, Remove, Keep value Method ========================================================== //

// Set default payment method to Credit Card on page load //
document.addEventListener('DOMContentLoaded', () => {
    const defaultbutton = document.querySelector('.payment-select button:first-child');
    setPaymethod(defaultbutton,'Credit Card');
});

// Toggle Paymethod and show item inside it //
function setPaymethod(button,method) { // onclick="setPaymethod(method)" //

    document.getElementById("SelectedPaymethod").value = method;

    // Hide Payment method details //
    document.getElementById('creditcard_detail').style.display = 'none';
    document.getElementById('banktransfer_detail').style.display = 'none';
    document.getElementById('promptpay_detail').style.display = 'none';

    // Show the selected payment method section //
    if (method === 'Credit Card') {
        document.getElementById('creditcard_detail').style.display = 'block';
    } else if (method === 'Bank transfer') {
        document.getElementById('banktransfer_detail').style.display = 'block';
    } else if (method === 'Prompt Pay') {
        document.getElementById('promptpay_detail').style.display = 'block';
    }

    // Clear values in all payment method sections //
    clearPaymentMethodInputs('creditcard_detail');
    clearPaymentMethodInputs('banktransfer_detail');
    clearPaymentMethodInputs('promptpay_detail');

    document.querySelectorAll('.payment-select button').forEach(btn => {
        btn.classList.remove('payment-button-color');
    });

    // Add 'selected' class to the clicked button
    button.classList.add('payment-button-color');
};

// Remove details values from old selected method after changing paymentmethod //
function clearPaymentMethodInputs(paymethoddetail) { 
    const section = document.getElementById(paymethoddetail);

    const inputs = section.querySelectorAll('input');
    inputs.forEach(input => input.value = '');

    const selects = section.querySelectorAll('select');
    selects.forEach(select => select.selectedIndex = 0); 
};

// Make hidden input value keep the name of the selected Card type to use it as a parameter to save in database //
function updateCreditCardType() {
    // Get the selected value from the dropdown
    const selectedCardType = document.getElementById('cardTypeSelect').value;
    // Set the value of the hidden input to the selected value
    document.getElementById('SelectedCardType').value = selectedCardType;
}
// Make hidden input value keep the name of the selected Bank account to use it as a parameter to save in database //
function updateBankacc() {

    const selectedCardType = document.getElementById('bankaccselect').value;
    document.getElementById('SelectedBankAcc').value = selectedCardType;
}

// ========================================================== Selected, Remove, Keep value Method ========================================================== //

// =============================================================== Handle Text Typing Method =============================================================== //

// Credit card Number text fixing adjustment //
document.getElementById('creditcardNumber').addEventListener('input', function (e) {
    let value = e.target.value.replace(/\D/g, '');
    value = value.replace(/(\d{4})(\d{4})(\d{4})(\d{4})/, '$1-$2-$3-$4');
    e.target.value = value;
});

//Expired Date on Credit card text fixing adjustment //
document.getElementById('creditcardExp').addEventListener('input', function (e) {
    let value = e.target.value.replace(/\D/g, '');
    value = value.replace(/(\d{2})(\d{2})/, '$1/$2');
    e.target.value = value;
});

// BankNumber text fixing adjustment //
document.getElementById('bankNumber').addEventListener('input', function (e) {
    let value = e.target.value.replace(/\D/g, '');
    value = value.replace(/(\d{3})(\d{1})(\d{5})(\d{1})/, '$1-$2-$3-$4');
    e.target.value = value;
});

// =============================================================== Handle Text Typing Method =============================================================== //

//document.querySelector('form').addEventListener('submit', (event) => {
//    // Reset all required attributes before setting the relevant ones
//    clearAllRequiredFields();

//    const selectedMethod = document.getElementById("SelectedPaymethod").value;

//    // Set required fields only for the active method section
//    if (selectedMethod === 'Credit Card') {
//        setRequiredFields(['creditcardName', 'creditcardNumber', 'creditcardExp', 'creditcardCvv']);
//    } else if (selectedMethod === 'Bank transfer') {
//        setRequiredFields(['bankaccselect', 'bankNumber']);
//    } else if (selectedMethod === 'Prompt Pay') {
//        setRequiredFields(['uploadedPic']);
//    }
//});

//function clearAllRequiredFields() {
//    document.querySelectorAll('[required]').forEach(field => {
//        field.removeAttribute('required');
//    });
//}

//function setRequiredFields(fieldIds) {
//    fieldIds.forEach(id => {
//        const field = document.getElementById(id);
//        if (field) {
//            field.setAttribute('required', 'required');
//        }
//    });
//}

// =============================================================== Button Color Selected =============================================================== //
