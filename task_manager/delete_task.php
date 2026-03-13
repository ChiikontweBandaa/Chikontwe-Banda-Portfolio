<?php
include 'includes/header.php';
include 'includes/auth_check.php';
include 'config/database.php';

if(isset($_GET['id'])) {
    $task_id = $_GET['id'];
    
    // Verify task belongs to current user before deleting
    $stmt = $pdo->prepare("DELETE FROM tasks WHERE id = ? AND user_id = ?");
    
    if($stmt->execute([$task_id, $_SESSION['user_id']])) {
        $_SESSION['success'] = "Task deleted successfully!";
    } else {
        $_SESSION['error'] = "Failed to delete task.";
    }
}

header("Location: dashboard.php");
exit();
?>