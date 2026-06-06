$(function () {
  $('.delete-form').on('submit', function () { return confirm('Are you sure you want to delete this record?'); });
  $('.data-table').each(function () {
    const table = $(this); const search = $('<input class="form-control mb-2" placeholder="Search table..."/>'); table.before(search);
    search.on('keyup', function () { const q = this.value.toLowerCase(); table.find('tbody tr').each(function () { $(this).toggle($(this).text().toLowerCase().indexOf(q) >= 0); }); });
  });
});
