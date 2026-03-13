<?php
include 'includes/header.php';
include 'includes/auth_check.php';
include 'config/database.php';

// Get tasks for the current user
$stmt = $pdo->prepare("SELECT * FROM tasks WHERE user_id = ? ORDER BY 
    CASE priority 
        WHEN 'high' THEN 1 
        WHEN 'medium' THEN 2 
        WHEN 'low' THEN 3 
    END, due_date ASC");
$stmt->execute([$_SESSION['user_id']]);
$tasks = $stmt->fetchAll();

// Task statistics
$total_tasks = count($tasks);
$completed_tasks = 0;
$pending_tasks = 0;

foreach($tasks as $task) {
    if($task['status'] == 'completed') {
        $completed_tasks++;
    } else {
        $pending_tasks++;
    }
}
?>

<div class="dashboard-header">
    <h2>Your Dashboard</h2>
    <a href="add_task.php" class="btn btn-primary">+ Add New Task</a>
</div>

<div class="stats-container">
    <div class="stat-card">
        <h3>Total Tasks</h3>
        <p class="stat-number"><?php echo $total_tasks; ?></p>
    </div>
    <div class="stat-card">
        <h3>Completed</h3>
        <p class="stat-number"><?php echo $completed_tasks; ?></p>
    </div>
    <div class="stat-card">
        <h3>Pending</h3>
        <p class="stat-number"><?php echo $pending_tasks; ?></p>
    </div>
</div>

<div class="tasks-container">
    <h3>Your Tasks</h3>
    
    <?php if(isset($_SESSION['success'])): ?>
        <div class="success-box">
            <p><?php echo $_SESSION['success']; unset($_SESSION['success']); ?></p>
        </div>
    <?php endif; ?>
    
    <?php if(empty($tasks)): ?>
        <div class="empty-state">
            <p>No tasks found. <a href="add_task.php">Create your first task!</a></p>
        </div>
    <?php else: ?>
        <div class="tasks-grid">
            <?php foreach($tasks as $task): ?>
                <div class="task-card <?php echo $task['status']; ?>">
                    <div class="task-header">
                        <h4><?php echo htmlspecialchars($task['title']); ?></h4>
                        <span class="priority-badge <?php echo $task['priority']; ?>">
                            <?php echo ucfirst($task['priority']); ?>
                        </span>
                    </div>
                    
                    <?php if(!empty($task['description'])): ?>
                        <p class="task-description"><?php echo htmlspecialchars($task['description']); ?></p>
                    <?php endif; ?>
                    
                    <div class="task-meta">
                        <?php if($task['due_date']): ?>
                            <span class="due-date">
                                📅 <?php echo date('M j, Y', strtotime($task['due_date'])); ?>
                            </span>
                        <?php endif; ?>
                        <span class="status-badge <?php echo $task['status']; ?>">
                            <?php echo ucfirst($task['status']); ?>
                        </span>
                    </div>
                    
                    <div class="task-actions">
                        <a href="edit_task.php?id=<?php echo $task['id']; ?>" class="btn btn-sm btn-edit">Edit</a>
                        <a href="delete_task.php?id=<?php echo $task['id']; ?>" 
                           class="btn btn-sm btn-delete" 
                           onclick="return confirm('Are you sure you want to delete this task?')">Delete</a>
                    </div>
                </div>
            <?php endforeach; ?>
        </div>
    <?php endif; ?>
</div>

<?php include 'includes/footer.php'; ?>