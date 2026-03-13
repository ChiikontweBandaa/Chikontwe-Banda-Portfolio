
document.addEventListener('DOMContentLoaded', function() {
    // Add confirmation for delete actions
    const deleteButtons = document.querySelectorAll('.btn-delete');
    deleteButtons.forEach(button => {
        button.addEventListener('click', function(e) {
            if (!confirm('Are you sure you want to delete this task?')) {
                e.preventDefault();
            }
        });
    });
    
    // date validation for due dates
    const dueDateInputs = document.querySelectorAll('input[type="date"]');
    dueDateInputs.forEach(input => {
        const today = new Date().toISOString().split('T')[0];
        input.min = today;
    });
    
    // Auto-hide success/error messages after 5 seconds
    const messages = document.querySelectorAll('.error-box, .success-box');
    messages.forEach(message => {
        setTimeout(() => {
            message.style.opacity = '0';
            message.style.transition = 'opacity 0.5s';
            setTimeout(() => message.remove(), 500);
        }, 5000);
    });
});