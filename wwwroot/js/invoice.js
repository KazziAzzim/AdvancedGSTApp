$(function () {
    function reindex() {
        $('#invoiceItems tbody tr').each(function (index) {
            $(this).find('[name]').each(function () {
                this.name = this.name.replace(/Invoice\.Items\[\d+\]/, 'Invoice.Items[' + index + ']');
            });
        });
    }

    function recalc() {
        let taxable = 0;
        let cgst = 0;
        let sgst = 0;
        let igst = 0;
        let grand = 0;
        const sameState = ($('#invoiceItems').data('company-state') || '').toString().toLowerCase() === ($('#placeOfSupply').val() || '').toLowerCase();

        $('.item-row').each(function () {
            const row = $(this);
            const quantity = parseFloat(row.find('.qty').val()) || 0;
            const rate = parseFloat(row.find('.rate').val()) || 0;
            const discountPercentage = parseFloat(row.find('.disc').val()) || 0;
            const gstPercentage = parseFloat(row.find('.gst').val()) || 0;
            const gross = quantity * rate;
            const discount = gross * discountPercentage / 100;
            const taxableAmount = gross - discount;
            const gstAmount = taxableAmount * gstPercentage / 100;
            let rowCgst = 0;
            let rowSgst = 0;
            let rowIgst = 0;

            if (sameState) {
                rowCgst = gstAmount / 2;
                rowSgst = gstAmount / 2;
            } else {
                rowIgst = gstAmount;
            }

            row.find('.line').val((taxableAmount + gstAmount).toFixed(2));
            taxable += taxableAmount;
            cgst += rowCgst;
            sgst += rowSgst;
            igst += rowIgst;
            grand += taxableAmount + gstAmount;
        });

        $('#taxable').val(taxable.toFixed(2));
        $('#cgst').val(cgst.toFixed(2));
        $('#sgst').val(sgst.toFixed(2));
        $('#igst').val(igst.toFixed(2));
        $('#grand').val(grand.toFixed(2));
    }

    $('#customer').on('change', function () {
        const selectedOption = $(this).find(':selected');

        $('#placeOfSupply').val(selectedOption.data('state') || '');
        $('#billing').val(selectedOption.data('billing') || '');
        $('#shipping').val(selectedOption.data('shipping') || '');
        recalc();
    });

    $('#addRow').on('click', function () {
        const clone = $('.item-row:first').clone();

        clone.find('input').val('');
        clone.find('.qty').val('1');
        clone.find('.disc').val('0');
        clone.find('.gst').val('18');
        $('#invoiceItems tbody').append(clone);
        reindex();
        recalc();
    });

    $(document).on('click', '.remove-row', function () {
        if ($('.item-row').length > 1) {
            $(this).closest('tr').remove();
        }

        reindex();
        recalc();
    });

    $(document).on('change keyup', '.qty,.rate,.disc,.gst,#placeOfSupply', recalc);

    $(document).on('change', '.item-select', function () {
        const row = $(this).closest('tr');
        const id = $(this).val();
        const type = $(this).find(':selected').data('type') || row.find('.item-type').val();

        row.find('.item-type').val(type);

        if (!id) {
            return;
        }

        $.getJSON('/SalesInvoices/ItemLookup', { type: type, id: id }, function (data) {
            row.find('.desc').val(data.description);
            row.find('.hsn').val(data.hsnSac);
            row.find('.gst').val(data.gstPercentage);
            row.find('.rate').val(data.rate);
            row.find('.unit').val(data.unit);
            recalc();
        });
    });

    recalc();
});
