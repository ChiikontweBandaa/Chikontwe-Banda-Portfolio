<?php
include 'includes/header.php';
include 'includes/auth_check.php';
include 'config/database.php';

if($_SERVER['REQUEST_METHOD'] == 'POST') {
    $task_id = $_POST['task_id'];
    $title = trim($_POST['title']);
    $description = trim($_POST['description']);
    $priority = $_POST['priority'];
    $status = $_POST['status'];
    $due_date = $_POST['due_date'];
    
    $errors = [];
    
    // Validation
    if(empty($title)) {
        $errors[] = "Title is required";
    }
    
    if(empty($errors)) {
        // Verify task belongs to current user before updating
        $stmt = $pdo->prepare
        ("UPDATE tasks SET title = ?, description = ?, priority = ?, status = ?, due_date = ? WHERE id = ? AND user_id = ?");
        
        if($stmt->execute([$title, $description, $priority, $status, $due_date, $task_id, $_SESSION['user_id']])) 
        {
            $_SESSION['success'] = "Task updated successfully!";
            header("Location: dashboard.php");
            exit();
        } else {
            $errors[] = "Failed to update task. Please try again.";
        }
    }
    
    // If there are errors, store them in session and redirect back to edit form
    if(!empty($errors)) {
        $_SESSION['errors'] = $errors;
        $_SESSION['form_data'] = [
            'title' => $title,
            'description' => $description,
            'priority' => $priority,
            'status' => $status,
            'due_date' => $due_date
        ];
        header("Location: edit_task.php?id=" . $task_id);
        exit();
    }
} else {
    // If someone tries to access this page directly without POST
    header("Location: dashboard.php");
    exit();
}
?>