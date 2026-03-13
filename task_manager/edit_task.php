<?php
include 'includes/header.php';
include 'includes/auth_check.php';
include 'config/database.php';

// Check if task ID is provided
if(!isset($_GET['id'])) {
    header("Location: dashboard.php");
    exit();
}

$task_id = $_GET['id'];

// Verify task belongs to current user
$stmt = $pdo->prepare("SELECT * FROM tasks WHERE id = ? AND user_id = ?");
$stmt->execute([$task_id, $_SESSION['user_id']]);
$task = $stmt->fetch();

if(!$task) {
    header("Location: dashboard.php");
    exit();
}

// Check for errors from update_task.php
$errors = [];
$form_data = [];

if(isset($_SESSION['errors'])) {
    $errors = $_SESSION['errors'];
    unset($_SESSION['errors']);
}

if(isset($_SESSION['form_data'])) {
    $form_data = $_SESSION['form_data'];
    unset($_SESSION['form_data']);
    
    // Use form data from failed submission, if not use database values
    $title = $form_data['title'];
    $description = $form_data['description'];
    $priority = $form_data['priority'];
    $status = $form_data['status'];
    $due_date = $form_data['due_date'];
} else {
    // Use database values
    $title = $task['title'];
    $description = $task['description'];
    $priority = $task['priority'];
    $status = $task['status'];
    $due_date = $task['due_date'];
}
?>

<div class="form-container">
    <div class="form-card">
        <h2>Edit Task</h2>
        
        <?php if(!empty($errors)): ?>
            <div class="error-box">
                <?php foreach($errors as $error): ?>
                    <p><?php echo $error; ?></p>
                <?php endforeach; ?>
            </div>
        <?php endif; ?>
        
        <form method="POST" action="update_task.php">
            <input type="hidden" name="task_id" value="<?php echo $task_id; ?>">
            
            <div class="form-group">
                <label>Title *</label>
                <input type="text" name="title" value="<?php echo htmlspecialchars($title); ?>" required>
            </div>
            
            <div class="form-group">
                <label>Description</label>
                <textarea name="description" rows="4"><?php echo htmlspecialchars($description); ?></textarea>
            </div>
            
            <div class="form-group">
                <label>Priority</label>
                <select name="priority">
                    <option value="low" <?php echo $priority == 'low' ? 'selected' : ''; ?>>Low</option>
                    <option value="medium" <?php echo $priority == 'medium' ? 'selected' : ''; ?>>Medium</option>
                    <option value="high" <?php echo $priority == 'high' ? 'selected' : ''; ?>>High</option>
                </select>
            </div>
            
            <div class="form-group">
                <label>Status</label>
                <select name="status">
                    <option value="pending" <?php echo $status == 'pending' ? 'selected' : ''; ?>>Pending</option>
                    <option value="in progress" <?php echo $status == 'in progress' ? 'selected' : ''; ?>>In Progress</option>
                    <option value="completed" <?php echo $status == 'completed' ? 'selected' : ''; ?>>Completed</option>
                </select>
            </div>
            
            <div class="form-group">
                <label>Due Date</label>
                <input type="date" name="due_date" value="<?php echo $due_date; ?>">
            </div>
            
            <div class="form-actions">
                <button type="submit" class="btn btn-primary">Update Task</button>
                <a href="dashboard.php" class="btn btn-secondary">Cancel</a>
            </div>
        </form>
    </div>
</div>

<?php include 'includes/footer.php'; ?>