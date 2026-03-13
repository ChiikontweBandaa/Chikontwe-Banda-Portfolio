<?php
include 'includes/header.php';

if(isset($_SESSION['user_id'])) {
    header("Location: dashboard.php");
    exit();
}
?>

<div class="hero-section">
    <div class="hero-content">
        <h1>Welcome to TaskManager</h1>
        <p>Organize your tasks efficiently and boost your productivity</p>
        <div class="hero-buttons">
            <a href="register.php" class="btn btn-primary">Get Started</a>
            <a href="login.php" class="btn btn-secondary">Login</a>
        </div>
    </div>
</div>

<div class="features-section">
    <div class="features-container">
        <div class="feature">
            <h3>📝 Create Tasks</h3>
            <p>Add new tasks with titles, descriptions, and due dates</p>
        </div>
        <div class="feature">
            <h3>📊 Track Progress</h3>
            <p>Monitor your task status from pending to completed</p>
        </div>
        <div class="feature">
            <h3>🎯 Set Priorities</h3>
            <p>Organize tasks by priority levels</p>
        </div>
        <div class="feature">
            <h3>👤 User Management</h3>
            <p>Secure user registration and authentication</p>
        </div>
    </div>
</div>

<?php include 'includes/footer.php'; ?>