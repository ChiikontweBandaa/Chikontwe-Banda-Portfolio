<?php
include 'includes/header.php';
include 'includes/auth_check.php';
include 'config/database.php';

if($_SERVER['REQUEST_METHOD'] == 'POST') {
    $title = trim($_POST['title']);
    $description = trim($_POST['description']);
    $priority = $_POST['priority'];
    $due_date = $_POST['due_date'];
    
    $errors = [];
    
    if(empty($title)) {
        $errors[] = "Title is required";
    }
    
    if(empty($errors)) {
        $stmt = $pdo->prepare("INSERT INTO tasks (user_id, title, description, priority, due_date) 
        VALUES (?, ?, ?, ?, ?)");
        
        if($stmt->execute([$_SESSION['user_id'], $title, $description, $priority, $due_date])) {
            $_SESSION['success'] = "Task added successfully!";
            header("Location: dashboard.php");
            exit();
        } else {
            $errors[] = "Failed to add task. Please try again.";
        }
    }
}
?>

<div class="form-container">
    <div class="form-card">
        <h2>Add New Task</h2>
        
        <?php if(!empty($errors)): ?>
            <div class="error-box">
                <?php foreach($errors as $error): ?>
                    <p><?php echo $error; ?></p>
                <?php endforeach; ?>
            </div>
        <?php endif; ?>
        
        <form method="POST" action="">
            <div class="form-group">
                <label>Title *</label>
                <input type="text" name="title" value="<?php echo $_POST['title'] ?? ''; ?>" required>
            </div>
            
            <div class="form-group">
                <label>Description</label>
                <textarea name="description" rows="4"><?php echo $_POST['description'] ?? ''; ?></textarea>
            </div>
            
            <div class="form-group">
                <label>Priority</label>
                <select name="priority">
                    <option value="low">Low</option>
                    <option value="medium" selected>Medium</option>
                    <option value="high">High</option>
                </select>
            </div>
            
            <div class="form-group">
                <label>Due Date</label>
                <input type="date" name="due_date" value="<?php echo $_POST['due_date'] ?? ''; ?>">
            </div>
            
            <div class="form-actions">
                <button type="submit" class="btn btn-primary">Add Task</button>
                <a href="dashboard.php" class="btn btn-secondary">Cancel</a>
            </div>
        </form>
    </div>
</div>

<?php include 'includes/footer.php'; ?>