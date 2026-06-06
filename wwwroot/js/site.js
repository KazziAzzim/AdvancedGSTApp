$(function () {
    $('[data-sidebar-toggle]').on('click', function () {
        $('body').toggleClass('sidebar-open');
    });

    $('[data-sidebar-close]').on('click', function () {
        $('body').removeClass('sidebar-open');
    });

    $('.delete-form').on('submit', function () {
        return confirm('Are you sure you want to delete this record?');
    });

    $('.data-table').each(function () {
        const table = $(this);
        const search = $('<input class="form-control table-search" placeholder="Search table..." />');

        table.closest('.table-responsive, .table-card').before(search);

        search.on('keyup', function () {
            const query = this.value.toLowerCase();

            table.find('tbody tr').each(function () {
                $(this).toggle($(this).text().toLowerCase().indexOf(query) >= 0);
            });
        });
    });
});
