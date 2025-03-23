// Category management functions
function confirmDelete(categoryId) {
    // Get the status of the category from the row
    const row = document.querySelector(`button[onclick="confirmDelete(${categoryId})"]`).closest('tr');
    const isActive = row.querySelector('.badge').textContent.trim() === 'Active';

    Swal.fire({
        title: isActive ? 'Are you sure?' : 'Activate Category?',
        text: isActive ? "You won't be able to revert this!" : "Do you want to activate this category?",
        icon: isActive ? 'warning' : 'question',
        showCancelButton: true,
        confirmButtonColor: isActive ? '#d33' : '#28a745',
        cancelButtonColor: '#3085d6',
        confirmButtonText: isActive ? 'Yes, deactivate it!' : 'Yes, activate it!'
    }).then((result) => {
        if (result.isConfirmed) {
            const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

            fetch(`/Category/ToggleStatus/${categoryId}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': token
                }
            }).then(response => {
                if (response.ok) {
                    Swal.fire({
                        title: isActive ? 'Deactivated!' : 'Activated!',
                        text: isActive ? 'The category has been deactivated successfully.' : 'The category has been activated successfully.',
                        icon: 'success',
                        confirmButtonColor: '#28a745'
                    }).then(() => {
                        window.location.reload();
                    });
                } else {
                    throw new Error('Network response was not ok');
                }
            }).catch(error => {
                Swal.fire({
                    title: 'Error!',
                    text: `There was an issue ${isActive ? 'deactivating' : 'activating'} the category. Please try again.`,
                    icon: 'error',
                    confirmButtonColor: '#d33'
                });
            });
        }
    });
}

// Handle page size changes
document.addEventListener('DOMContentLoaded', function () {
    const pageSizeInput = document.getElementById('pageSizeInput');
    if (pageSizeInput) {
        pageSizeInput.addEventListener('change', function () {
            const pageSize = parseInt(this.value) || 8;
            if (pageSize > 0) {
                const searchTerm = document.querySelector('input[name="searchTerm"]')?.value || '';
                window.location.href = `/Category/CategoryIndex?pageSize=${pageSize}&pageNumber=1&searchTerm=${encodeURIComponent(searchTerm)}`;
            }
        });
    }

    // Search bar code
    const searchForm = document.getElementById('searchForm');
    const searchInput = searchForm.querySelector('input[name="searchTerm"]');

    searchInput.addEventListener('keydown', function (event) {
        if (event.key === 'Enter') {
            event.preventDefault();
            searchForm.submit();
        }
    });

    // Datatable Sorting
    const list = new List('table-default', {
        sortClass: 'table-sort',
        listClass: 'table-tbody',
        valueNames: [
            'sort-name', 'sort-parent', 'sort-description', 'sort-status'
        ]
    });
});
