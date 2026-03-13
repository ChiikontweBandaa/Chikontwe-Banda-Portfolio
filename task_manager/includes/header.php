<?php
session_start();
?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Task Manager</title>
    <link rel="stylesheet" href="css/style.css">
</head>
<body>
    <nav class="navbar">
        <div class="nav-container">
            <h1 class="nav-logo">TaskManager</h1>
            <?php if(isset($_SESSION['user_id'])): ?>
            <div class="nav-links">
                <span>Welcome, <?php echo $_SESSION['username']; ?>!</span>
                <a href="dashboard.php">Dashboard</a>
                <a href="logout.php">Logout</a>
            </div>
            <?php endif; ?>
        </div>
    </nav>
    <div class="container">