// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    $('#expense-form').validate();

    $('#mileage-reimbursements-table').on('click', '.add-mileage-row', addNewRow);

    $('#other-expenses-table').on('click', '.add-other-expense-row', addNewRow);

    $('#mileage-reimbursements-table').on('change', '.mileage-reimbursement-amount', calculateReimbursement);

    $('#other-expenses-table').on('change', '.other-expense-amount', calculateReimbursement);

    function addNewRow() {
        let button = $(this);

        let tableRow = button.parents('tr');

        let newTableRow = tableRow.clone();

        newTableRow.find('input').val('');

        tableRow.after(newTableRow);

        $('.date-masked-input').each(function (index, input) {
            input = $(input);

            input.mask('0000-00-00');
        });

        let table = button.parents('table');

        table.find('tr').each(function (rowIndex, tableRow) {
            tableRow = $(tableRow);

            tableRow.find('input').each(function (inputIndex, input) {
                input = $(input);

                input.prop('name', input.prop('name').replace(/\d/, rowIndex - 2));
            });
        });
    }

    function calculateReimbursement() {
        let amountInput = $(this);

        if (isNaN(parseInt(amountInput.val()))) {
            amountInput.val('');
        }

        let totalMileage = 0;

        $('.mileage-reimbursement-amount').each(function (index, input) {
            let amount = parseFloat(accounting.toFixed($(input).val()));

            if (isNaN(amount)) {
                return false;
            }

            totalMileage = totalMileage + amount;
        });

        let totalMileageAmount = accounting.formatMoney(totalMileage);

        $('#total-mileage').text(totalMileageAmount);

        let otherExpenseTotal = 0;

        $('.other-expense-amount').each(function (index, input) {
            let amount = parseFloat(accounting.toFixed($(input).val()));

            if (isNaN(amount)) {
                return false;
            }

            otherExpenseTotal = otherExpenseTotal + amount;
        });

        let otherExpenseTotalAmount = accounting.formatMoney(otherExpenseTotal);

        $('#total-expenses').text(otherExpenseTotalAmount);

        let total = totalMileage + otherExpenseTotal;

        let totalReimbursementAmount = accounting.formatMoney(total);

        $('#total-reimbursement').text(totalReimbursementAmount);
    }
});
