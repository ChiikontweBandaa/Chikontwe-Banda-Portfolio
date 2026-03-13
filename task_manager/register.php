<?php
include 'includes/header.php';
include 'config/database.php';

if($_SERVER['REQUEST_METHOD'] == 'POST') {
    $username = trim($_POST['username']);
    $email = trim($_POST['email']);
    $password = $_POST['password'];
    $confirm_password = $_POST['confirm_password'];
    
    $errors = [];
    
    // Validation
    if(empty($username)) $errors[] = "Username is required";
    if(empty($email)) $errors[] = "Email is required";
    if(empty($password)) $errors[] = "Password is required";
    if($password !== $confirm_password) $errors[] = "Passwords do not match";
    
    // Check if the user exists
    $stmt = $pdo->prepare("SELECT id FROM users WHERE username = ? OR email = ?");
    $stmt->execute([$username, $email]);
    if($stmt->rowCount() > 0) {
        $errors[] = "Username or email already exists";
    }
    
    if(empty($errors)) {
        $hashed_password = password_hash($password, PASSWORD_DEFAULT);
        $stmt = $pdo->prepare("INSERT INTO users (username, email, password) VALUES (?, ?, ?)");
        
        if($stmt->execute([$username, $email, $hashed_password])) {
            $_SESSION['success'] = "Registration successful! Please login.";
            header("Location: login.php");
            exit();
        } else {
            $errors[] = "Registration failed. Please try again.";
        }
    }
}
?>

<div class="auth-container">
    <div class="auth-form">
        <h2>Register</h2>
        
        <?php if(!empty($errors)): ?>
            <div class="error-box">
                <?php foreach($errors as $error): ?>
                    <p><?php echo $error; ?></p>
                <?php endforeach; ?>
            </div>
        <?php endif; ?>
        
        <form method="POST" action="">
            <div class="form-group">
                <label>Username:</label>
                <input type="text" name="username" value="<?php echo $_POST['username'] ?? ''; ?>" required>
            </div>
            
            <div class="form-group">
                <label>Email:</label>
                <input type="email" name="email" value="<?php echo $_POST['email'] ?? ''; ?>" required>
            </div>
            
            <div class="form-group">
                <label>Password:</label>
                <input type="password" name="password" required>
            </div>
            
            <div class="form-group">
                <label>Confirm Password:</label>
                <input type="password" name="confirm_password" required>
            </div>
            
            <button type="submit" class="btn btn-primary">Register</button>
        </form>
        
        <p>Already have an account? <a href="login.php">Login here</a></p>
    </div>
</div>

<?php include 'includes/footer.php'; ?>